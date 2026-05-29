import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../core/services/auth';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatButtonModule, MatCardModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class HomeComponent {
  userName = '';

  constructor(private authService: AuthService) {
    this.userName = this.authService.getUserName();
  }

  logout() {
    this.authService.logout();
  }
}