import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ResumeService {

  private apiUrl = 'http://localhost:5227/api/resume';

  constructor(private http: HttpClient) {}

  uploadResume(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(
      `${this.apiUrl}/upload`, formData);
  }

  analyseResume(
    resumeId: number, 
    jobDescription: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/analyse`, {
      resumeId,
      jobDescription
    });
  }

  getHistory(): Observable<any> {
    return this.http.get(`${this.apiUrl}/history`);
  }
}