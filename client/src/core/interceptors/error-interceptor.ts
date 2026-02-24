import { HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs';
import { ToastService } from '../services/toast-service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);
  const router = inject(Router);  
  
  return next(req).pipe(
    catchError((error) => {
      if(error){
        switch (error.status) {
          case 400:
            toast.error(error.error);        
            break;
          case 401:
            toast.error("Unauthorized");
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          default:
            toast.error("Something unexpected went wrong");
            break;
        }
      }

      throw error;
    })
  )
}
