import { Component, signal } from '@angular/core';
import { ProfileEditComponent } from './profile/profile-edit.component/profile-edit.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ProfileEditComponent],
  templateUrl: './app.html',
  styleUrl: './app.sass'
})
export class App {
  protected readonly title = signal('profile-exercise-angular');
}
