import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'webapp-angular';
  health_status = 'checking...';

  async ngOnInit() {
    console.log('apiUrl=' + environment.apiUrl);
    const response = await fetch(environment.apiUrl+ 'health');
    const data = await response.text();
    this.health_status = data;
  }
}
