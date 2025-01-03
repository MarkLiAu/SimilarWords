import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Word } from '../../../domain/model/word';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import {MatBadgeModule} from '@angular/material/badge';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-word-details',
  imports: [NgClass, MatIconModule, RouterLink, MatDividerModule, MatCardModule,MatTooltipModule,MatChipsModule,MatBadgeModule],
  templateUrl: './word-details.component.html',
  styleUrl: './word-details.component.scss'
})
export class WordDetailsComponent {
  @Input() word: Word | undefined ;
  get similarWords() { 
    return this.word?.similarWords?.trim().split(" ")
  };
  get dictionaryLinks() {
    return this.word ===undefined ? []
    : [
    { name: "Collins", url: `https://www.collinsdictionary.com/dictionary/english/${this.word.name}` },
    { name: "Longman", url: `https://www.ldoceonline.com/dictionary/${this.word.name}` },
    { name: "Cambridge", url: `https://dictionary.cambridge.org/dictionary/english/${this.word.name}` },
    { name: "Oxford", url: `https://www.oxfordlearnersdictionaries.com/definition/english/${this.word.name}` },
    { name: "Merriam-Webster", url: `https://www.merriam-webster.com/dictionary/${this.word.name}` },
    { name: "iCIBA", url: `https://www.iciba.com/word?w=${this.word.name}` },
  ]
  };
  get getFrequencyStyle() {
    return this.word ===undefined || !this.word.frequency ? []
    : [
      (this.word.frequency ?? 99999) <= 10000 ? 'color-on' : 'color-off',
      (this.word.frequency ?? 99999) <= 8000 ? 'color-on' : 'color-off',
      (this.word.frequency ?? 99999) <= 6000 ? 'color-on' : 'color-off',
      (this.word.frequency ?? 99999) <= 4000 ? 'color-on' : 'color-off',
      (this.word.frequency ?? 99999) <= 2000 ? 'color-on' : 'color-off'
    ];
  }

  playSound = (url:string|undefined) => {
    if(!url) return;
    let mySound = new Audio(url);
    mySound.play().catch(e=>console.log('failed to play sound'));
  };

}
