import { TestBed } from '@angular/core/testing';

import { ProfileFormService } from './profile-form.service';

describe('Profile', () => {
  let service: ProfileFormService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProfileFormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
