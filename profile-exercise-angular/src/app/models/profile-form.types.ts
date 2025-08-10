import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { SocialMediaTypes } from './profile.model';

export type SocialAccountForm = FormGroup<{
  type: FormControl<SocialMediaTypes>;
  address: FormControl<string>;
}>;

export type ProfileForm = FormGroup<{
  id: FormControl<string>;
  firstName: FormControl<string>;
  lastName: FormControl<string>;
  socialSkills: FormArray<FormControl<string>>;
  socialAccounts: FormArray<SocialAccountForm>;
}>;
