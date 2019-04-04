import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {Contract} from '../models/contract';
import {ActivatedRoute} from '@angular/router';

declare var $: any;
@Injectable({
  providedIn: 'root'
})
export class ContractSignalRService {
  private signalRServerEndPoint: string;
  private connection: any;
  private proxy: any;

  private contractSource = new BehaviorSubject<Contract>({});
  public contract$ = this.contractSource.asObservable();

  constructor(private route: ActivatedRoute) {
    this.signalRServerEndPoint = this.route.snapshot.queryParamMap.get('signalRUrl');
  }

  public startConnection() {
    this.connection = $.hubConnection(this.signalRServerEndPoint);

    this.proxy = this.connection.createHubProxy('contract');
    this.proxy.on('doOnConnectAndOnDisconnect', () => {}); // Needed so OnConnected and OnDisconnected will fire
    this.proxy.on('dataFromServer', (contractName) => this.contractSource.next(new Contract(contractName)));

    this.connection.start().done((data: any) => {
      console.log('Connected to Notification Hub');
    }).catch((error: any) => {
      console.log('Notification Hub error -> ' + error);
    });
  }

  public save(contractJson: string): void {
    this.proxy.invoke('Save', contractJson)
      .done(() => {
        this.connection.stop();
        window.close();
      })
      .fail((error: any) => {
        console.log('broadcastMessage error -> ' + error);
      });
  }
}
