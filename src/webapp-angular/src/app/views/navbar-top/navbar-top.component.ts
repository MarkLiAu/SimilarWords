import { Component, computed } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatMenuModule} from '@angular/material/menu';
import { RouterLink, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { UserAuthService } from '../../service/user-auth.service';

@Component({
  selector: 'app-navbar-top',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterLink, FormsModule, MatFormFieldModule,MatMenuModule, MatInputModule],
  templateUrl: './navbar-top.component.html',
  styleUrl: './navbar-top.component.scss'
})
export class NavbarTopComponent {
  constructor(private router: Router, private userService: UserAuthService, private responsive: BreakpointObserver) { }
  searchText = '';
  icon_url = 'similar-words-logo-wide.png';

  onSubmit(f: NgForm) {
    this.router.navigate(['/search',this.searchText]);
  }

  ngOnInit() {
    this.responsive.observe(Breakpoints.HandsetPortrait).subscribe(result => {
        this.icon_url = result.matches ?  'similar-words-logo.png' : 'similar-words-logo-wide.png';
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
}
