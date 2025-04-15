import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})

export class LoginService {
  apiUrl = 'http://localhost:5205';
  constructor(private readonly http: HttpClient) {}
  userLogin(user: {name: string, pwd: string}) {
      return this.http.post<any>(`${this.apiUrl}/login`, user)
  }
}

