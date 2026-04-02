import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { Observable, of, tap } from 'rxjs';
import { LikesService } from './likes-service';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accoutnService = inject(AccountService);

  init() {
    return this.accoutnService.refreshToken().pipe(
      tap(user => {
        if (user) {
          this.accoutnService.setCurrentUser(user);
          this.accoutnService.startTokenRefreshInterval();
        }     
    })
  )
  } 
}
