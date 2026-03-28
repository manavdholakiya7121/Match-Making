import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../../types/pagination';
import { Message } from '../../types/message';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getMessages(container: string, pageNumber: number, pageSize: number) {
    let params = new HttpParams()

    params = params.append('Container', container);
    params = params.append('PageNumber', pageNumber);
    params = params.append('PageSize', pageSize);

    return this.http.get<PaginatedResult<Message>>(this.baseUrl + 'messages', { params })
  }
}
