import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Observable, of, switchMap, map, catchError, startWith } from 'rxjs';

import { Profile } from '../../models/profile.model';
import { ProfileHttpService } from '../../services/profile-http.service';

/** ViewModel voor de template */
type Vm = {
  loading: boolean;
  error: string | null;
  profiles: Profile[];
};

@Component({
  selector: 'app-profile-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile-list.component.html',
  styleUrls: ['./profile-list.component.sass'],
})
export class ProfileListComponent implements OnInit {
  private api = inject(ProfileHttpService);
  private router = inject(Router);

  vm$!: Observable<Vm>;

  ngOnInit(): void {
    this.vm$ = this.loadVm$();
  }

  private normalizeProfile = (dto: any): Profile => ({
    id: dto.id ?? dto.Id ?? undefined,
    firstName: dto.firstName ?? dto.FirstName ?? '',
    lastName: dto.lastName ?? dto.LastName ?? '',
    socialSkills: (dto.socialSkills ?? dto.SocialSkills ?? []).map((s: any) => ({
      value: s.value ?? s.Value ?? '',
    })),
    socialAccounts: (dto.socialAccounts ?? dto.SocialAccounts ?? []).map((a: any) => ({
      type: a.type ?? a.Type ?? 0,
      address: a.address ?? a.Address ?? '',
    })),
  });

  private loadVm$(): Observable<Vm> {
    return of(null).pipe(
      switchMap(() =>
        this.api.getProfiles().pipe(
          // unwrap: ProfileResponse[] -> Profile[]
          map(responses => (responses ?? []).map(r => {
            const rawProfile = r?.profile ?? r?.profile ?? r; // fallback just-in-case
            return this.normalizeProfile(rawProfile);
          })),
          map(list => ({ loading: false, error: null, profiles: list } as Vm)),
          startWith({ loading: true, error: null, profiles: [] } as Vm),
          catchError(() => of({ loading: false, error: 'Laden mislukt', profiles: [] } as Vm)),
        )
      )
    );
  }

  /** Actions */
  create(): void {
    this.router.navigate(['/profiles/new']);
  }

  open(p: Profile): void {
    const id = p?.id?.trim();
    if (id) this.router.navigate(['/profiles', id]);
  }

  delete(p: Profile, ev?: MouseEvent): void {
    ev?.stopPropagation();
    if (!p?.id) return;

    const confirmed = confirm(`Weet je zeker dat je "${p.firstName} ${p.lastName}" wil verwijderen?`);
    if (!confirmed) return;

    this.api.deleteProfile(p.id).subscribe({
      next: () => {
        alert('✅ Verwijderd');
        // reload vm
        this.vm$ = this.loadVm$();
      },
      error: () => alert('❌ Verwijderen mislukt'),
    });
  }

  trackById(index: number, p: Profile): string {
    // Fallback om NG0955 te voorkomen als id ontbreekt
    return p.id?.trim() || `idx-${index}`;
  }

  getSocialSkillsString(p: Profile): string {
    return p.socialSkills?.map(s => s.value).join(', ') || '—';
  }
}