import { Component, computed, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterLink } from '@angular/router';
import { Word } from '../../../domain/model/word';

@Component({
  selector: 'app-word-details',
  imports: [MatIconModule, RouterLink, MatDividerModule, MatCardModule,MatTooltipModule],
  templateUrl: './word-details.component.html',
  styleUrl: './word-details.component.scss'
})
export class WordDetailsComponent {
  word = input.required<Word>();
  similarWords = computed(() => this.word().similarWords?.trim().split(" "));
  dictionaryLinks = computed(() => [
    { name: "Collins", url: `https://www.collinsdictionary.com/dictionary/english/${this.word().name}` },
    { name: "Longman", url: `https://www.ldoceonline.com/dictionary/${this.word().name}` },
    { name: "Cambridge", url: `https://dictionary.cambridge.org/dictionary/english/${this.word().name}` },
    { name: "Oxford", url: `https://www.oxfordlearnersdictionaries.com/definition/english/${this.word().name}` },
    { name: "Merriam-Webster", url: `https://www.merriam-webster.com/dictionary/${this.word().name}` },
  ]);

  playSound = (url:string|undefined) => {
    if(!url) return;
    let mySound = new Audio(url);
    mySound.play().catch(e=>console.log('failed to play sound'));
}
}
