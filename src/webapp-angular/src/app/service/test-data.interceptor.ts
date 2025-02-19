import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { of } from 'rxjs';
import { Word } from '../domain/model/word';
import { dateTimestampProvider } from 'rxjs/internal/scheduler/dateTimestampProvider';

export const testDataInterceptor: HttpInterceptorFn = (req, next) => {
  const testWordList: Word[] = [
    {
        "name": "truck",
        "pronunciation": "[trʌk]",
        "pronunciationAm": "",
        "frequency": 1118,
        "similarWords": " track trick trunk tuck",
        "meaningShort": new Date().toISOString(),
        "meaningLong": new Date().toISOString(),
        "example": "",
        "meaningOther": "",
        "soundUrl": "http://www.gstatic.com/dictionary/static/sounds/20200429/truck--_us_1.mp3",
        "exampleSoundUrl": "",
        "id": 1160,
        "viewTime": new Date("0001-01-01T00:00:00"),
        "easiness": -2147483648,
        "viewInterval": -2147483648,
        "totalViewed": -2147483648,
        "startTime": new Date("0001-01-01T00:00:00")
    },
    {
        "name": "track",
        "pronunciation": "[træk]",
        "pronunciationAm": "",
        "frequency": 1031,
        "similarWords": " truck crack trace trick rack tract tack",
        "meaningShort": new Date().toISOString(),
        "meaningLong":  "If you document your study time and record all your exam scores, then you track your progress in school. In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something.  In this case the verb track shows that you're following the path of something. The noun track can refer to a path of a more literal kind.",
        "example": "",
        "meaningOther": "",
        "soundUrl": "",
        "exampleSoundUrl": "",
        "id": 1070,
        "viewTime": new Date(new Date("0001-01-01T00:00:00")),
        "easiness": -2147483648,
        "viewInterval": -2147483648,
        "totalViewed": -2147483648,
        "startTime": new Date(new Date("0001-01-01T00:00:00"))
    },
    {
        "name": "trick",
        "pronunciation": "[trɪk]",
        "pronunciationAm": "",
        "frequency": 2710,
        "similarWords": " tricky track truck thick brick tick trickle",
        "meaningShort": new Date().toISOString(),
        "meaningLong": new Date().toISOString(),
        "example": "",
        "meaningOther": "",
        "soundUrl": "",
        "exampleSoundUrl": "",
        "id": 2803,
        "viewTime": new Date(new Date("0001-01-01T00:00:00")),
        "easiness": -2147483648,
        "viewInterval": -2147483648,
        "totalViewed": -2147483648,
        "startTime": new Date(new Date("0001-01-01T00:00:00"))
    },
    {
        "name": "trunk",
        "pronunciation": "[trʌŋk]",
        "pronunciationAm": "",
        "frequency": 3116,
        "similarWords": " truck drunk",
        "meaningShort": "The noun trunk refers to the main stem of a tree. If you want to make maple syrup, you need to tap the trunk of the maple tree and collect the tree's sap, which can then be boiled into a sticky syrup.",
        "meaningLong": new Date().toISOString(),
        "example": "",
        "meaningOther": "",
        "soundUrl": "",
        "exampleSoundUrl": "",
        "id": 3219,
        "viewTime": new Date(new Date("0001-01-01T00:00:00")),
        "easiness": -2147483648,
        "viewInterval": -2147483648,
        "totalViewed": -2147483648,
        "startTime": new Date(new Date("0001-01-01T00:00:00"))
    },
    {
        "name": "tuck",
        "pronunciation": "[tʌk]",
        "pronunciationAm": "",
        "frequency": 3601,
        "similarWords": "  stuck luck buck duck suck fuck tick truck",
        "meaningShort": "Push something in. Jack tucked his shirt in.\nSchool tuck shop",
        "meaningLong": new Date().toISOString(),
        "example": "",
        "meaningOther": "",
        "soundUrl": "",
        "exampleSoundUrl": "",
        "id": 3736,
        "viewTime": new Date(new Date("0001-01-01T00:00:00")),
        "easiness": -2147483648,
        "viewInterval": -2147483648,
        "totalViewed": -2147483648,
        "startTime": new Date(new Date("0001-01-01T00:00:00"))
    }
  ];

  if(!environment.useTestData) {
    return next(req);
  } 

  if(req.url.includes('api/words')) {
    return of(new HttpResponse({body: testWordList}));
  }
  
  return next(req);
};
