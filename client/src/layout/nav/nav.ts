import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountService = inject(AccountService);
  protected creds: any = {}

  login() {
    console.log(this.creds)
    this.accountService.login(this.creds).subscribe({
      next: response => {
        this.creds = {};
        console.log('Login successful', response);
      },
      error: error => alert('Login failed: ' + error)      
    });
  }

  logout() {
    this.accountService.logout();
  } 
}
