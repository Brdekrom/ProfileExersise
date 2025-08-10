export interface Profile {
  id?: string;
  firstName: string;
  lastName: string;
  socialSkills: SocialSkill[];
  socialAccounts: SocialAccount[];
}

export interface SocialSkill {
  value: string;
}

export interface SocialAccount {
  type: SocialMediaTypes; 
  address: string;
}

export enum SocialMediaTypes {
  None = 0,
  Facebook = 1,
  Twitter = 2,
  LinkedIn = 3,
  Instagram = 4,
  GitHub = 5,
  YouTube = 6,
  TikTok = 7,
  Snapchat = 8,
  Pinterest = 9
}