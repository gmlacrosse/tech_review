import { Pipe, PipeTransform } from "@angular/core";
import { Person } from "../models";


@Pipe({ name: 'filter' })
export class FilterPersonPipe implements PipeTransform {
  transform(items: Person[] | undefined, searchTerm: string): Person[] | undefined {
    if (!items || !searchTerm) {
      return items;
    }
    searchTerm = searchTerm.toLowerCase();
    return items.filter(item => item.name.toLowerCase().includes(searchTerm));
  }
}
