import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-error',
  imports: [],
  templateUrl: './test-error.html',
  styleUrl: './test-error.css',
})
export class TestError {
  private http = inject(HttpClient);

  baseUrl= 'https://localhost:5001/api/buggy';

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  get400ValidationError() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }


  
}
