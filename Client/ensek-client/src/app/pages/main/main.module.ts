import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main.component';
import { MainRoutingModule } from './main-routing.module';
import { MaterialDesignModule } from '@modules/material-design/material-design.module';
import { BatchUploadsComponent } from './batch-uploads/batch-uploads.component';
import { MeterReadingsComponent } from './meter-readings/meter-readings.component';


@NgModule({
  imports: [
    CommonModule,
    MainRoutingModule,
    MaterialDesignModule
  ],
  declarations: [MainComponent, BatchUploadsComponent, MeterReadingsComponent]
})
export class MainModule { }
