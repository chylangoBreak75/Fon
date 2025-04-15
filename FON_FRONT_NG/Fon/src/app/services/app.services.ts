import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})

export class AppService {
  apiUrlApps = 'http://localhost:5205';
  constructor(private readonly http: HttpClient) {}
  appList() {
      return this.http.get<any>(`${this.apiUrlApps}/ListarApps`)
  }

  typeList() {
    return this.http.get<any>(`${this.apiUrlApps}/GetTypesApps`)
  }

  statusList() {
    return this.http.get<any>(`${this.apiUrlApps}/GetStatusApps`)
  }

  addApp(app: any) {
    return this.http.post<any>(`${this.apiUrlApps}/addApp`, app)
  }

}
