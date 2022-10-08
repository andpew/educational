import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/user.model';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserCardComponent implements OnInit {

  @Input() user: User = {} as User;

  constructor() { }

  ngOnInit(): void {

  }
}
