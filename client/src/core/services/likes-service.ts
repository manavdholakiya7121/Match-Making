import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { signalGetFn } from '@angular/core/primitives/signals';
import { Member } from '../../types/member';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  private baseurl = environment.apiUrl;
  private http = inject(HttpClient);
  likeIds = signal<string[]>([]);

  toggleLike(targetMemberId: string){
    return this.http.post(`${this.baseurl}likes/${targetMemberId}`, {})
  }

  getLikes(predicate: string){
    return this.http.get<Member[]>(this.baseurl + 'likes?predicate=' + predicate);
  }

  getLikeIds(){
    return this.http.get<string[]>(this.baseurl + 'likes/list').subscribe({
      next: ids => this.likeIds.set(ids)
    })
  }

  clearLikeIds(){
    this.likeIds.set([])
  }
}
