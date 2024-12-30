import { Component, inject, input, Signal, resource } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { WordsDataService } from '../../service/words-data.service';
import { Word } from '../../domain/model/word';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-search',
  imports: [FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
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

}
