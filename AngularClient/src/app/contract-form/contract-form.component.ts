import { Component, OnInit } from '@angular/core';
import {Contract} from '../models/contract';
import {ContractSignalRService} from '../services/contract-signal-r.service';

@Component({
  selector: 'app-contract-form',
  templateUrl: './contract-form.component.html',
  styleUrls: ['./contract-form.component.scss']
})
export class ContractFormComponent implements OnInit {

  model = new Contract();

  constructor(private contractSignalRService: ContractSignalRService) { }

  ngOnInit() {
    this.contractSignalRService.startConnection();
  }

  onSubmit() {
    const json = JSON.stringify(this.model);

    this.contractSignalRService.save(json);
    alert('You submitted: ' + JSON.stringify(json));
  }
}
