import { Component, computed, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatMenuModule} from '@angular/material/menu';
import {MatBadgeModule} from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterLink, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { UserAuthService } from '../../service/user-auth.service';
import { WordsDataService } from '../../service/words-data.service';

@Component({
  selector: 'app-navbar-top',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterLink, FormsModule,MatTooltipModule,MatBadgeModule, MatFormFieldModule,MatMenuModule, MatInputModule],
  templateUrl: './navbar-top.component.html',
  styleUrl: './navbar-top.component.scss'
})
export class NavbarTopComponent {
  constructor(private router: Router, private userService: UserAuthService, private responsive: BreakpointObserver) { }
  headerTitle = '';
  wordsDataServise = inject(WordsDataService);
  searchText = '';
  icon_url = 'similar-words-logo-wide.png';

  onSubmit(f: NgForm) {
    if(!this.searchText) return;
    const text = this.searchText;
    this.searchText = '';
    this.router.navigate(['/search',text]);
  }

  ngOnInit() {
    this.responsive.observe(Breakpoints.HandsetPortrait).subscribe(result => {
        this.headerTitle = result.matches ?  '' : 'Similar Words';
    });
  }

  login() {
    window.location.href = '/login';
  }

  logout() {
    window.location.href = '/logout';
  }

  get userAuthData() {
    return this.userService.userAuthData;
  }

  get userName() {
    return this.userAuthData.value()?.clientPrincipal?.userDetails;
  }

  // get number of words to study
  get numWordsToStudy() {
    return this.wordsDataServise.userWordStudyData.isLoading() ? 0 : this.wordsDataServise.userWordStudyData.value()?.length;
  }
  get numNewWordsToStudy() {
    return this.wordsDataServise.userWordStudyData.isLoading() ? 0 : this.wordsDataServise.userWordStudyData.value()?.filter(w => w.studyCount == 0).length;
  }
}
