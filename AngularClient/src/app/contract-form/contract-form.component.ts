import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Contract} from '../models/contract';
import {ContractSignalRService} from '../services/contract-signal-r.service';

@Component({
  selector: 'app-contract-form',
  templateUrl: './contract-form.component.html',
  styleUrls: ['./contract-form.component.scss']
})
export class ContractFormComponent implements OnInit {

  private _contract = new Contract();

  constructor(private contractSignalRService: ContractSignalRService, private cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.contractSignalRService.contract$.subscribe(contract => {
      this._contract = contract;
      this.cd.detectChanges();
    });
    this.contractSignalRService.startConnection();
  }

  onSubmit() {
    const json = JSON.stringify(this._contract);

    this.contractSignalRService.save(json);
    alert('You submitted: ' + JSON.stringify(json));
  }
}
