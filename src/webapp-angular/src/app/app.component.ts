import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';
import { NavbarTopComponent } from './views/navbar-top/navbar-top.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarTopComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'webapp-angular';
  health_status = 'checking...';

}
