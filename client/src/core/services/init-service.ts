import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accoutnService = inject(AccountService);

  init(): Observable<null> {
    const user = localStorage.getItem('user');
    if (!user) return of(null);
    this.accoutnService.setCurrentUser(JSON.parse(user));

    return of(null);

  }
  
}
