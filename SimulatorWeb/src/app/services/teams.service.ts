import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Team } from '../models/Team';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TeamsService {
  route = 'groups'
  url = 'https://localhost:7082/api/teams/groups'

  constructor(
    private httpClient: HttpClient
  ) { }

  public getGroups() {
     return this.httpClient.get<Team[][]>(this.url)
  }
}
