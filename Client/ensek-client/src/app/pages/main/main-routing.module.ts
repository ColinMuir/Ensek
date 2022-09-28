import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BatchUploadsComponent } from './batch-uploads/batch-uploads.component';
import { MainComponent } from './main.component';
import { MeterReadingsComponent } from './meter-readings/meter-readings.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      { path: '', redirectTo: 'batch', pathMatch: 'full' },
      { path: 'batch', component: BatchUploadsComponent },
      { path: 'meterreads', component: MeterReadingsComponent }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainRoutingModule { }