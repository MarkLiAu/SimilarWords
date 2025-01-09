import { Component, Input } from '@angular/core';
import { MatIconModule} from '@angular/material/icon'

@Component({
  selector: 'app-word-bookmark',
  imports: [MatIconModule],
  templateUrl: './word-bookmark.component.html',
  styleUrl: './word-bookmark.component.scss'
})
export class WordBookmarkComponent {
@Input({required:true}) wordName: string | undefined;

}
