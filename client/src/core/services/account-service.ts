import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginCreds, RegisterCreds, User } from '../../types/user';
import { tap } from 'rxjs/internal/operators/tap';
import { environment } from '../../environments/environment';
import { LikesService } from './likes-service';
import { PresenceService } from './presence-service';
import { HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  private likesService = inject(LikesService);
  private baseUrl = environment.apiUrl;
  private presenceService = inject(PresenceService);
  currentUser = signal<User | null>(null);

  register(creds: RegisterCreds){
    return this.http.post<User>(this.baseUrl + 'account/register', creds, { withCredentials: true }).pipe(
      tap(user => {
        if (user) {
          this.setCurrentUser(user);
          this.startTokenRefreshInterval();         
        }
      })
    );
  }

  login(creds: LoginCreds) {
    return this.http.post<User>(this.baseUrl + 'account/login', creds, { withCredentials: true }).pipe(
      tap(user => {
        if (user) {
          this.setCurrentUser(user);         
          this.startTokenRefreshInterval();
        }
      })
    );
  }

  refreshToken(){
    return this.http.post<User>(this.baseUrl + 'account/refresh-token', {}, { withCredentials: true })
  }

  setCurrentUser(user: User){
      user.roles = this.getRolesFromToken(user);
      this.currentUser.set(user);
      this.likesService.getLikeIds();
      if(this.presenceService.hubConnection?.state !== HubConnectionState.Connected) {
        this.presenceService.createHubConnection(user);
      }

  }

  startTokenRefreshInterval() {
    setInterval(() => {
      this.refreshToken().subscribe({
        next: (user) => {
          if (user) {
            this.setCurrentUser(user);            
          }
        },
        error: () => {
          this.logout();
        }
      })
    }, 5 * 60 * 1000)
  }

  logout() {
    localStorage.removeItem('filters');
    this.likesService.clearLikeIds();
    this.currentUser.set(null);
    this.presenceService.stopHubConnection();
  }

  private getRolesFromToken(user: User) : string[] {
    const payload = user.token.split('.')[1];
    const decodedToken = atob(payload);
    const tokenData = JSON.parse(decodedToken);
    return Array.isArray(tokenData.role) ? tokenData.role : [tokenData.role];
  }


}
