import { HttpClient } from '@angular/common/http';
import { inject, Injectable, ResourceRef, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { UserInfo } from '../domain/model/user-auth';
import { rxResource } from '@angular/core/rxjs-interop';


@Injectable({
  providedIn: 'root'
})
export class UserAuthService {
  #http = inject(HttpClient);
  userAuthData = rxResource({
    loader:() => this.#http.get<UserInfo>('/.auth/me')
  })

}
