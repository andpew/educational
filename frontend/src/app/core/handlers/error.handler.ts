import { HttpErrorResponse, HttpStatusCode } from "@angular/common/http";
import { ErrorHandler, Inject, Injectable, Injector } from "@angular/core";
import { ToastrService } from "ngx-toastr";

@Injectable()
export class CustomErrorHandler implements ErrorHandler {

  constructor(@Inject(Injector) private readonly injector: Injector) { }

  private get toastrService() {
    return this.injector.get(ToastrService);
  }

  handleError(error: Error): void {
    if (error instanceof HttpErrorResponse) {
      switch (error.status) {
        case HttpStatusCode.Unauthorized:
          break;
      }
    }

    console.error(error);
    this.toastrService.error(error.message, error.name);
  }
}
