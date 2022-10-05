import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { BaseComponent } from './base/base.component';




@NgModule({
  declarations: [BaseComponent],
  imports: [SharedModule],
})
export class CoreModule { }
