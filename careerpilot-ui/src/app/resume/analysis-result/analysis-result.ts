import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } 
  from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-analysis-result',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule, MatButtonModule,
    MatChipsModule, MatIconModule,
    MatProgressBarModule, MatDividerModule
  ],
  templateUrl: './analysis-result.html',
  styleUrl: './analysis-result.css'
})
export class AnalysisResultComponent implements OnInit {
  result: any = null;

  constructor(private router: Router) {}

  ngOnInit() {
    const stored = localStorage.getItem('analysisResult');
    if (stored) {
      this.result = JSON.parse(stored);
    } else {
      this.router.navigate(['/resume']);
    }
  }

  getScoreColor(score: number): string {
    if (score >= 75) return 'primary';
    if (score >= 50) return 'accent';
    return 'warn';
  }

  analyseAnother() {
    localStorage.removeItem('analysisResult');
    this.router.navigate(['/resume']);
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }
}