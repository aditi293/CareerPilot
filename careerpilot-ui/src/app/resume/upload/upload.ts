import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { ResumeService } from '../../core/services/resume';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [
    CommonModule, FormsModule,
    MatCardModule, MatButtonModule,
    MatInputModule, MatFormFieldModule,
    MatProgressBarModule, MatIconModule
  ],
  templateUrl: './upload.html',
  styleUrl: './upload.css'
})
export class UploadComponent {
  selectedFile: File | null = null;
  jobDescription = '';
  isUploading = false;
  isAnalysing = false;
  errorMessage = '';
  resumeId: number | null = null;

  constructor(
    private resumeService: ResumeService,
    private router: Router
  ) {}

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file && file.type === 'application/pdf') {
      this.selectedFile = file;
      this.errorMessage = '';
    } else {
      this.errorMessage = 'Please select a PDF file only';
    }
  }

  uploadAndAnalyse() {
    if (!this.selectedFile) {
      this.errorMessage = 'Please select a PDF file';
      return;
    }

    if (!this.jobDescription.trim()) {
      this.errorMessage = 
        'Please enter a job description';
      return;
    }

    // Step 1: Upload
    this.isUploading = true;
    this.resumeService.uploadResume(this.selectedFile)
      .subscribe({
        next: (res) => {
          this.resumeId = res.resumeId;
          this.isUploading = false;

          // Step 2: Analyse
          this.isAnalysing = true;
          this.resumeService.analyseResume(
            res.resumeId, 
            this.jobDescription)
            .subscribe({
              next: (analysis) => {
                this.isAnalysing = false;
                // Store result and navigate
                localStorage.setItem(
                  'analysisResult', 
                  JSON.stringify(analysis));
                this.router.navigate(
                  ['/resume/result']);
              },
              error: () => {
                this.isAnalysing = false;
                this.errorMessage = 
                  'Analysis failed. Try again.';
              }
            });
        },
        error: () => {
          this.isUploading = false;
          this.errorMessage = 
            'Upload failed. Try again.';
        }
      });
  }
}