import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {UserProfile} from '../models/userProfile';
import {ActivatedRoute} from '@angular/router';

declare var $: any;
@Injectable({
  providedIn: 'root'
})
export class UserProfileSignalRService {
  private readonly signalRServerEndPoint: string;
  private connection: any;
  private proxy: any;

  private userProfileSource = new BehaviorSubject<UserProfile>({});
  public userProfile$ = this.userProfileSource.asObservable();

  constructor(private route: ActivatedRoute) {
    this.signalRServerEndPoint = this.route.snapshot.queryParamMap.get('signalRUrl');
  }

  public startConnection() {
    this.connection = $.hubConnection(this.signalRServerEndPoint);

    this.proxy = this.connection.createHubProxy('userProfile');
    this.proxy.on('doOnConnectAndOnDisconnect', () => {}); // Needed so OnConnected and OnDisconnected will fire
    this.proxy.on('OnReceivedFromWpfApp', (userProfileJson) => this.userProfileSource.next(JSON.parse(userProfileJson)));

    this.connection.start().done((data: any) => {
      console.log('Connected to Notification Hub');
    }).fail((error: any) => {
      console.log('Notification Hub error -> ' + error);
    });
  }

  public save(userProfileJson: string): void {
    this.proxy.invoke('SendToWpfApp', userProfileJson)
      .done(() => {
        this.connection.stop(false, true);
        window.close();
      })
      .fail((error: any) => {
        console.log('ERROR: Failed to send to WpfApp -> ' + error);
      });
  }
}
