import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MeterReading } from '../meter-reading';

@Injectable({
  providedIn: 'root'
})
export class MeterReadingService {

constructor(private http: HttpClient) {
 }

 getAll() {
  return this.http
  .get('https://localhost:7000/api/MeterReads')
  .pipe(
    map((data: MeterReading[]) => data)
  );
  }
}
