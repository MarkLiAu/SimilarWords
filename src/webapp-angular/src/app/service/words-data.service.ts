import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { WordStudyModel } from '../domain/model/word';
import { Observable } from 'rxjs';
import { rxResource } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root'
})
export class WordsDataService {
  #baseUrl = environment.apiUrl;
  #http = inject(HttpClient);
  constructor() { }

  userWordStudyData = rxResource({
    loader:() => this.#http.get<WordStudyModel[]>(`${this.#baseUrl}/wordstudy`)
  })

  searchWords(wordName: string|undefined): Observable<WordStudyModel[]> {
    return this.#http.get<WordStudyModel[]>(`${this.#baseUrl}/words/${wordName}`);
  }

  UpdateWordStudy(wordName: string, daysToStudy: number =0) : Observable<number> {
    return this.#http.post<number>(`${this.#baseUrl}/study/${wordName}?daysToStudy=${daysToStudy}`,null);
  }
}
