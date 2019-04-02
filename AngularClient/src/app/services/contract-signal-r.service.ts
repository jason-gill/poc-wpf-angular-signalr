import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {Contract} from '../models/contract';

declare var $: any;
@Injectable({
  providedIn: 'root'
})
export class ContractSignalRService {
  private connection: any;
  private proxy: any;

  private contractSource = new BehaviorSubject<Contract>({});
  public contract$ = this.contractSource.asObservable();

  constructor() { }

  public startConnection() {
    const signalRServerEndPoint = 'http://localhost:9013';
    this.connection = $.hubConnection(signalRServerEndPoint);

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
      .catch((error: any) => {
        console.log('broadcastMessage error -> ' + error);
      });
  }
}
