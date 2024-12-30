import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-navbar-top',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterLink, FormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './navbar-top.component.html',
  styleUrl: './navbar-top.component.scss'
})
export class NavbarTopComponent {
  constructor(private router: Router) { }
  searchText = '';

  onSubmit(f: NgForm) {
    this.router.navigate(['/search',this.searchText]);
  }
}
