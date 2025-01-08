export interface Person {
  id: number;
  name: string;
  currentRank: string;
  currentDutyTitle: string;
  careerStartDate: Date;
  careerEndDate?: Date;
}

export interface PersonResponse {
  people: Person[];
  success: boolean;
  message: string;
  responseCode: number;
}
