import { Profile } from "./profile.model";

export interface ProcessedName {
  vowelCount: number;
  consonantCount: number;
  reversedFirstName: string;
  reversedLastName: string;
}

export interface ProfileResponse {
  profile: Profile;
  processedName: ProcessedName;
}
