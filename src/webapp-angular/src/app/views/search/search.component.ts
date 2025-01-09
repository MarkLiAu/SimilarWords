import { Component, inject, input, Signal, resource } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatGridList, MatGridListModule } from '@angular/material/grid-list';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { WordsDataService } from '../../service/words-data.service';
import { WordStudyModel } from '../../domain/model/word';
import { firstValueFrom, Observable } from 'rxjs';
import { WordDetailsComponent } from "./word-details/word-details.component";

@Component({
  selector: 'app-search',
  imports: [FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatGridListModule, WordDetailsComponent],
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent {
  #dataService = inject(WordsDataService);
  searchText = input<string|undefined>();
  dataResource = rxResource({
    request:() => this.searchText(),
    loader:() => this.#dataService.searchWords(this.searchText()),
  })

  async onBookmarkClicked(wordStudy : WordStudyModel) {
    console.log('onBookmarkClicked in search.component.ts', wordStudy);
    console.log(wordStudy);
    const result = await firstValueFrom(this.#dataService.bookmarkWord(wordStudy.wordName!));
    this.dataResource.reload();
  }


}
