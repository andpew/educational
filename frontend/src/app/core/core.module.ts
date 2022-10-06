import { ErrorHandler, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { BaseComponent } from './base/base.component';
import { CustomErrorHandler } from './handlers/error.handler';

@NgModule({
  declarations: [BaseComponent],
  imports: [SharedModule],
  providers: [{ provide: ErrorHandler, useClass: CustomErrorHandler }]
})
export class CoreModule { }
