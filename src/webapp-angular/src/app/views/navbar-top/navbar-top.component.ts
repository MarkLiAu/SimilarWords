import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Component({
  selector: 'app-navbar-top',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterLink, FormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './navbar-top.component.html',
  styleUrl: './navbar-top.component.scss'
})
export class NavbarTopComponent {
  constructor(private router: Router, private responsive: BreakpointObserver) { }
  searchText = '';
  icon_url = 'similar-words-logo-wide.png';

  onSubmit(f: NgForm) {
    this.router.navigate(['/search',this.searchText]);
  }

  ngOnInit() {
    console.log(this.responsive===undefined);
    this.responsive.observe(Breakpoints.HandsetPortrait).subscribe(result => {
        this.icon_url = result.matches ?  'similar-words-logo.png' : 'similar-words-logo-wide.png';
        console.log("icon_url: ", this.icon_url);
    });
  }
}
