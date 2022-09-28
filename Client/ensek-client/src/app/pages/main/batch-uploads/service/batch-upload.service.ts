import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { catchError } from 'rxjs/operators';
import { Batch } from '../batch';

@Injectable({
  providedIn: 'root'
})
export class BatchUploadService {
  constructor(private http: HttpClient) {
 }

 getAll() {
  return this.http
  .get('https://localhost:7000/api/Batch')
  .pipe(
    map((data: Batch[]) => data)
  );
  }
}
