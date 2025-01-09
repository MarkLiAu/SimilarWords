export class Word {
  name!: string;
  pronounciation: string | undefined;          // British
  pronounciationAm: string | undefined;        // American
  frequency: number | undefined;
  similarWords: string | undefined;
  meaningShort: string | undefined;
  meaningLong: string | undefined;
  meaningOther: string | undefined;            // meaning in other language
  soundUrl: string | undefined;
  exampleSoundUrl: string | undefined;
  id: number | undefined;

  // fields for viewing
  viewTime: Date | undefined;
  easiness: number | undefined;                // user choose the easiness:: not started yet,  -2, -1:hard, 0:normal, 1:easy ,2 
  viewInterval: number | undefined;            // viewTime + viewInterval = next view time
  totalViewed: number | undefined;
  startTime: Date | undefined;
}

export class WordStudyModel {
  id?: number;
  userName?: string | null;
  wordName?: string | null;
  startTimeUtc?: Date;
  lastStudyTimeUtc?: Date;
  studyCount?: number;
  isClosed?: boolean;
  daysToStudy?: number; // lastStudyTimeUtc + daysToStudy = nextStudyTimeUtc to remind user to study
  word?: Word | null;
}
