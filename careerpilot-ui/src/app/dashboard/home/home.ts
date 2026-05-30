import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../core/services/auth';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class HomeComponent {
  userName = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.userName = this.authService.getUserName();
  }

  goToResume() {
    this.router.navigate(['/resume']);
  }

  logout() {
    this.authService.logout();
  }
}