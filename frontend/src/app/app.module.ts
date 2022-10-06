import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    ToastrModule.forRoot({
      progressBar: true,
      countDuplicates: true,
      preventDuplicates: true,
      resetTimeoutOnDuplicate: true,
      timeOut: 2500
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
