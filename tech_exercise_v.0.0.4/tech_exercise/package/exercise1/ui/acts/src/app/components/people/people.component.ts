import { Component, inject } from '@angular/core';
import { PeopleService } from '../../services/people.service';
import { Person } from '../../models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-people',
  standalone: false,

  templateUrl: './people.component.html',
  styleUrl: './people.component.css'
})
export class PeopleComponent {
  peopleService = inject(PeopleService);

  people: Person[] = [];

  ngOnInit(){
    this.peopleService.getAllPeople().subscribe((people) => {
      this.people = people;
    });
    console.log(this.people);
  }
}
