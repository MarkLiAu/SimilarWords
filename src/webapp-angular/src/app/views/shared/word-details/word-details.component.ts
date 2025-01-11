import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Word, WordStudyModel } from '../../../domain/model/word';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import {MatBadgeModule} from '@angular/material/badge';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-word-details',
  imports: [NgClass, MatIconModule, RouterLink, MatDividerModule, MatCardModule, MatTooltipModule, MatChipsModule, MatBadgeModule],
  templateUrl: './word-details.component.html',
  styleUrl: './word-details.component.scss'
})
export class WordDetailsComponent {
  @Input() wordStudy: WordStudyModel | undefined ;
  @Output() bookmarkClicked = new EventEmitter<boolean>();
  get similarWords() { 
    return this.wordStudy?.word?.similarWords?.trim().split(" ")
  };
  get word() {
    return this.wordStudy?.word;
  }
  get dictionaryLinks() {
    const name = this.wordStudy?.word?.name;
    if(!name) return [];
    return [
    { name: "Collins", url: `https://www.collinsdictionary.com/dictionary/english/${name}` },
    { name: "Longman", url: `https://www.ldoceonline.com/dictionary/${name}` },
    { name: "Cambridge", url: `https://dictionary.cambridge.org/dictionary/english/${name}` },
    { name: "Oxford", url: `https://www.oxfordlearnersdictionaries.com/definition/english/${name}` },
    { name: "Merriam-Webster", url: `https://www.merriam-webster.com/dictionary/${name}` },
    { name: "iCIBA", url: `https://www.iciba.com/word?w=${name}` },
  ]
  };
  get getFrequencyStyle() {
    const word = this.wordStudy?.word;
    if (!word?.frequency) {
      return [];
    }

    return [
      word.frequency <= 10000 ? 'color-on' : 'color-off',
      word.frequency <= 8000 ? 'color-on' : 'color-off',
      word.frequency <= 6000 ? 'color-on' : 'color-off',
      word.frequency <= 4000 ? 'color-on' : 'color-off',
      word.frequency <= 2000 ? 'color-on' : 'color-off'
    ];
  }


  playSound = (url:string|undefined) => {
    if(!url) return;
    let mySound = new Audio(url);
    mySound.play().catch(e=>console.log('failed to play sound'));
  };

  onBookmarkClicked = () => {
    // buble click to parent component
    this.bookmarkClicked.emit(true);
  }

}
