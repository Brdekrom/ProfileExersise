import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Profile } from '../models/profile.model';
import { ProfileResponse } from '../models/profile-response.model';

@Injectable({ providedIn: 'root' })
export class ProfileHttpService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/profile';

  getProfiles(): Observable<ProfileResponse[]> {
    return this.http.get<ProfileResponse[]>(this.baseUrl);
  }

  getProfile(id: string): Observable<ProfileResponse> {
    return this.http.get<ProfileResponse>(`${this.baseUrl}/${id}`);
  }

  createProfile(profile: Profile): Observable<ProfileResponse> {
  return this.http.post<ProfileResponse>(this.baseUrl, profile);
}


  updateProfile(id: string, profile: Profile): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, profile);
  }

  deleteProfile(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
