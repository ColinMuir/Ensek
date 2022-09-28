import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError, map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor(private http: HttpClient) { }

  postFile(url: string, file: File) {
    const payload = new FormData();

    payload.append("file", file);

    return this.http.post(url, payload).pipe(
      map(response => response),
      catchError((error:HttpErrorResponse) => {
        console.log('File upload error')
        return throwError(() =>  new Error(error.message));
      } )
    );
  }
}
