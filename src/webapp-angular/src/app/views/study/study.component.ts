import { Component, computed, inject, signal } from '@angular/core';
import { WordsDataService } from '../../service/words-data.service';
import { MatProgressSpinnerModule, ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms'; // For ngModel
import { WordDetailsComponent } from '../shared/word-details/word-details.component';
import { UserAuthService } from '../../service/user-auth.service';
import { MatIconModule } from '@angular/material/icon';
import { firstValueFrom } from 'rxjs';
@Component({
  selector: 'app-study',
  imports: [WordDetailsComponent, MatProgressSpinnerModule, MatIconModule,MatTooltipModule, MatButtonModule, MatInputModule, MatFormFieldModule,FormsModule],
  // imports: [MatProgressSpinnerModule, WordDetailsComponent,MatIconModule,MatTooltipModule, MatInputModule, MatFormFieldModule,FormsModule],
  templateUrl: './study.component.html',
  styleUrl: './study.component.scss'
})
export class StudyComponent {
  // authorization
  authService = inject(UserAuthService);
  authData = this.authService.userAuthData;
  // study data
  #wordsDataService = inject(WordsDataService);
  studyData = this.#wordsDataService.userWordStudyData;
  daysAdjust = signal(0);
  idx = signal(0);
  daysToStudy = computed(() => this.studyData.value()![this.idx()]?.daysToStudy);
  studyMessage = computed(() => {
    console.log('in studyMessage', this.idx(), this.studyData.value());
      const days = (this.daysToStudy()??0)+this.daysAdjust();
      return days <= 0 ? 'Next study in 2 hours' :`Study in ${days} days`;
    });

  hideMeaningToggle = true;
  toggleWordExplanation() {
    this.hideMeaningToggle = !this.hideMeaningToggle
  }

  nextWord() {
    this.idx.update( (idx)=> (idx + 1) % this.studyData.value()!.length);
    this.daysAdjust.update(days => 0);
  }

  prevWord() {
    this.idx.update( (idx)=> (idx - 1 + this.studyData.value()!.length) % this.studyData.value()!.length);
    this.daysAdjust.update(days => 0);
  }

  increaseDaysAdjust() {
    this.daysAdjust.update(days => days + 1);
  }

  decreaseDaysAdjust() {
    if (this.daysAdjust()+this.daysToStudy()! <= 1) return;
    this.daysAdjust.update(days => days - 1);
  }

  async onSubmitStudy() {
    console.log('onSubmitStudy in study.component.ts', this.studyData.value());
    const result = await firstValueFrom(this.#wordsDataService.UpdateWordStudy(this.studyData.value()![this.idx()]?.wordName!,this.daysAdjust()+this.daysToStudy()!));
    this.studyData.update((list)=> 
      {
        console.log('in onSubmitStudy:', this.idx(), this.studyData.value());

        list!.splice(this.idx(),1);
        console.log(' onSubmitStudy after slice:', this.idx(), this.studyData.value());
       
        return list;
      });
    if(this.idx() >= this.studyData.value()!.length) {
      console.log('studied last word, move to previous word');
        this.prevWord();
    }
  }
  
}
