import { BootstrapOptions, Component, OnInit } from '@angular/core';
import { Batch } from './batch';
import { BatchUploadService } from './service/batch-upload.service';
import { FileUploadService } from './service/file-upload.service';

@Component({
  selector: 'app-batch-uploads',
  templateUrl: './batch-uploads.component.html',
  styleUrls: ['./batch-uploads.component.css']
})
export class BatchUploadsComponent implements OnInit {

  batches = [];

  loading = false;

  columns: string[] = ['id', 'totalRecords', 'successful', 'failures'];

  constructor(private batchService: BatchUploadService, private fileUploadService: FileUploadService) { }

  ngOnInit() {
    this.refreshTable();
  }

  refreshTable() {
    this.loading = true;
    this.batchService.getAll().subscribe(data=> {
      this.batches = data;
      this.loading = false;
    });
  }

  onFileSelected(event: any) {
    let file: File= event.target.files[0]

    this.fileUploadService
      .postFile('https://localhost:7000/meter-reading-uploads/', file)
      .subscribe(() => { 
          this.refreshTable();
          console.log('File uploaded')
        }, error => console.log('error', error));
  }
}
