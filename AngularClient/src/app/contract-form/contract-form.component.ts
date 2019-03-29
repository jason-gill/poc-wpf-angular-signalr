import { Component, OnInit } from '@angular/core';
import {Contract} from '../models/contract';

@Component({
  selector: 'app-contract-form',
  templateUrl: './contract-form.component.html',
  styleUrls: ['./contract-form.component.scss']
})
export class ContractFormComponent implements OnInit {

  model = new Contract();

  constructor() { }

  ngOnInit() {
  }

  onSubmit() {
    alert('You submitted: ' + JSON.stringify(this.model));
  }
}
