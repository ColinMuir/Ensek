import { Component, OnInit } from '@angular/core';
import { MeterReadingService } from './service/meter-reading.service';

@Component({
  selector: 'app-meter-readings',
  templateUrl: './meter-readings.component.html',
  styleUrls: ['./meter-readings.component.css']
})
export class MeterReadingsComponent implements OnInit {

  reads = [];

  loading = false;

  columns: string[] = ['id', 'accountId', 'meterReadingDateTime', 'meterReadValue'];
  
  constructor(private meterReadingService: MeterReadingService) { }

  ngOnInit() {
    this.refreshTable();
  }

  refreshTable() {
    this.loading = true;
    this.meterReadingService.getAll().subscribe(data=> {
      this.reads = data;
      this.loading = false;
    });
  }
}
