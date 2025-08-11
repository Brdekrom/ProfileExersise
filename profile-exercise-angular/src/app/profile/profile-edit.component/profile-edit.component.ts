import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import {
  catchError,
  EMPTY,
  finalize,
  map,
  Observable,
  switchMap,
  tap,
} from 'rxjs';

import { ProfileForm } from '../../models/profile-form.types';
import { Profile, SocialMediaTypes } from '../../models/profile.model';
import {
  ProcessedName,
  ProfileResponse,
} from '../../models/profile-response.model';

import { ProfileFormService } from '../../services/profile-form.service';
import { ProfileHttpService } from '../../services/profile-http.service';

@Component({
  selector: 'app-profile-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.sass'],
})
export class ProfileEditComponent implements OnInit {
  form!: ProfileForm;

  readonly SocialMediaTypes = SocialMediaTypes;

  mediaTypes = Object.values(SocialMediaTypes).filter(
    (typeValue) => typeof typeValue === 'number'
  ) as number[];

  saving = false;
  saved = false;
  profile: Profile | null = null;
  error: { status?: number; message: string } | null = null;

  processedName: ProcessedName | null = null;

  constructor(
    private formService: ProfileFormService,
    private profileClient: ProfileHttpService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.formService.createProfileForm();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadForEdit(id);
    }
  }

  private unwrapAndNormalize = (
    dto: ProfileResponse
  ): { profile: Profile; processedName: ProcessedName | null } => {
    const raw = dto?.profile ?? dto;

    const normalized: Profile = {
      id: raw?.id ?? '',
      firstName: raw?.firstName ?? '',
      lastName: raw?.lastName ?? '',
      socialSkills: (raw?.socialSkills ?? []).map((s: any) => ({
        value: s?.value ?? s?.Value ?? '',
      })),
      socialAccounts: (raw?.socialAccounts ?? []).map((a: any) => ({
        type: a?.type ?? a?.Type ?? 0,
        address: a?.address ?? a?.Address ?? '',
      })),
    };

    const pn: ProcessedName | null =
      dto?.processedName ?? dto?.processedName ?? null;
    return { profile: normalized, processedName: pn };
  };

  private loadForEdit(id: string): void {
    this.saving = true;
    this.profileClient
      .getProfile(id)
      .pipe(
        map((res) => this.unwrapAndNormalize(res)),
        finalize(() => (this.saving = false))
      )
      .subscribe({
        next: ({ profile, processedName }) => {
          this.profile = profile;
          this.processedName = processedName;
          this.formService.patchProfileForm(this.form, profile);
        },
        error: (_) => {
          this.error = { message: 'Profile laden mislukt' };
        },
      });
  }

  get socialSkills() {
    return this.form.controls.socialSkills;
  }
  get socialAccounts() {
    return this.form.controls.socialAccounts;
  }

  addSkill() {
    this.socialSkills.push(this.formService.createSocialSkillControl());
  }
  removeSkill(i: number) {
    this.socialSkills.removeAt(i);
  }

  addAccount() {
    this.socialAccounts.push(this.formService.createSocialAccountGroup());
  }
  removeAccount(i: number) {
    this.socialAccounts.removeAt(i);
  }

  private buildPayload(): Profile {
    const raw = this.form.getRawValue();
    return {
      ...raw,
      socialSkills: raw.socialSkills.map((value) => ({ value })),
      socialAccounts: raw.socialAccounts,
    };
  }

  private upsert$(payload: Profile): Observable<{
    profile: Profile;
    processedName: ProcessedName | null;
    created: boolean;
  }> {
    const id = payload.id?.trim();
    if (id) {
      return this.profileClient.updateProfile(id, payload).pipe(
        switchMap(() => this.profileClient.getProfile(id)),
        map((res) => {
          const unwrapped = this.unwrapAndNormalize(res);
          return {
            profile: unwrapped.profile,
            processedName: null,
            created: false,
          };
        })
      );
    } else {
      return this.profileClient.createProfile(payload).pipe(
        map((res: ProfileResponse) => {
          const unwrapped = this.unwrapAndNormalize(res);
          return {
            profile: unwrapped.profile,
            processedName: unwrapped.processedName,
            created: true,
          };
        })
      );
    }
  }

  save(): void {
    this.saved = false;
    this.error = null;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;

    const payload = this.buildPayload();

    this.upsert$(payload)
      .pipe(
        tap(({ profile, processedName, created }) => {
          this.profile = profile;
          this.processedName = processedName;
          this.saved = true;

          if (created && profile?.id) {
            this.router.navigate(['/profiles'], { replaceUrl: true });
          } else {
            this.formService.patchProfileForm(this.form, profile);
            this.router.navigate(['/profiles'], { replaceUrl: true });
          }
        }),
        catchError((err: HttpErrorResponse) => {
          this.handleHttpError(err);
          return EMPTY;
        }),
        finalize(() => (this.saving = false))
      )
      .subscribe();
  }

  private handleHttpError(err: HttpErrorResponse): void {
    this.error = {
      status: err.status,
      message: err.error?.title || err.message || 'Opslaan mislukt',
    };
  }
}
