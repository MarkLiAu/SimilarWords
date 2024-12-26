import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

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
    const response = await fetch('http://localhost:7071/api/health');
    const data = await response.text();
    this.health_status = data;
  }
}
