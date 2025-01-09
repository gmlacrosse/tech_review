import { Component, inject, signal } from '@angular/core';
import { PeopleService } from '../../services/people.service';
import { Person } from '../../models';
import { FilterPersonPipe } from '../../directives/person-filter.pipe';

@Component({
  selector: 'app-people',
  standalone: false,

  templateUrl: './people.component.html',
  styleUrl: './people.component.css'
})
export class PeopleComponent {
  peopleService = inject(PeopleService);
  searchTerm = signal('');
  people: Person[] = [];

  constructor() { }

  ngOnInit(){
    this.peopleService.getAllPeople().subscribe(response => {
      this.people = response.people;
    });
  }
}


