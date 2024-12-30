import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Word } from '../domain/model/word';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WordsDataService {
  #baseUrl = environment.apiUrl;
  #http = inject(HttpClient);
  constructor() { }

  searchWords(word: string|undefined): Observable<Word[]> {
    return this.#http.get<Word[]>(`${this.#baseUrl}/words/${word}`);
  }
}
