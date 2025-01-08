import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Person } from '../models';

@Injectable({
  providedIn: 'root'
})
export class PeopleService {
  private apiUrl = 'https://localhost:7204'; // Replace with actual API URL
  private httpOptions = {
    headers: new HttpHeaders({ 'accept': '*/*', 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getPersonByName(name: string): Observable<Person> {
    return this.http.get<Person>(`${this.apiUrl}/person/${name}`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  getAllPeople(): Observable<Person[]> {
    return this.http.get<Person[]>(`${this.apiUrl}/person`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  addOrUpdatePerson(person: Person): Observable<Person> {
    return this.http.post<Person>(`${this.apiUrl}/person`, person, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(err: any): Observable<never> {
    let errorMessage = `${err.error.message}`;
    console.log(err);
    return throwError(() => errorMessage)
  }
}
