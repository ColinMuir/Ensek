import { NgModule } from '@angular/core';

import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
  imports: [
  ],
  declarations: [],
  exports: [
    MatSidenavModule,
    MatButtonModule,
    MatListModule,
    MatTableModule,
    MatProgressBarModule
  ]
})
export class MaterialDesignModule { }