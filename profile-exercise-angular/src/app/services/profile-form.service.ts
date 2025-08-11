import { Injectable } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import {
  Profile,
  SocialAccount,
  SocialSkill,
  SocialMediaTypes,
} from '../models/profile.model';
import { ProfileForm, SocialAccountForm } from '../models/profile-form.types';

@Injectable({ providedIn: 'root' })
export class ProfileFormService {
  createProfileForm(profile?: Profile): ProfileForm {
    return new FormGroup({
      id: new FormControl<string>(profile?.id ?? '', { nonNullable: true }),
      firstName: new FormControl<string>(profile?.firstName ?? '', {
        nonNullable: true,
        validators: [Validators.required],
      }),
      lastName: new FormControl<string>(profile?.lastName ?? '', {
        nonNullable: true,
        validators: [Validators.required],
      }),
      socialSkills: new FormArray<FormControl<string>>(
        (profile?.socialSkills ?? []).map((s) =>
          this.createSocialSkillControl(s)
        )
      ),
      socialAccounts: new FormArray<SocialAccountForm>(
        (profile?.socialAccounts ?? []).map((a) =>
          this.createSocialAccountGroup(a)
        )
      ),
    });
  }

  createSocialSkillControl(skill?: SocialSkill): FormControl<string> {
    return new FormControl<string>(skill?.value ?? '', {
      nonNullable: true,
      validators: [Validators.required],
    });
  }

  createSocialAccountGroup(account?: SocialAccount): SocialAccountForm {
    return new FormGroup({
      type: new FormControl<SocialMediaTypes>(
        account?.type ?? SocialMediaTypes.None,
        { nonNullable: true, validators: [Validators.required] }
      ),
      address: new FormControl<string>(account?.address ?? '', {
        nonNullable: true,
        validators: [Validators.required],
      }),
    });
  }

  getSocialSkillsArray(form: ProfileForm): FormArray<FormControl<string>> {
    return form.controls.socialSkills;
  }

  getSocialAccountsArray(form: ProfileForm): FormArray<SocialAccountForm> {
    return form.controls.socialAccounts;
  }

  patchProfileForm(form: ProfileForm, profile: Profile): void {
    form.controls.id.setValue(profile.id ?? '');
    form.controls.firstName.setValue(profile.firstName ?? '');
    form.controls.lastName.setValue(profile.lastName ?? '');

    const skills = this.getSocialSkillsArray(form);
    while (skills.length) skills.removeAt(0);
    (profile.socialSkills ?? []).forEach((s) =>
      skills.push(this.createSocialSkillControl(s))
    );

    const accounts = this.getSocialAccountsArray(form);
    while (accounts.length) accounts.removeAt(0);
    (profile.socialAccounts ?? []).forEach((a) =>
      accounts.push(this.createSocialAccountGroup(a))
    );

    form.markAsPristine();
    form.markAsUntouched();
    form.updateValueAndValidity({ emitEvent: false });
  }
}
