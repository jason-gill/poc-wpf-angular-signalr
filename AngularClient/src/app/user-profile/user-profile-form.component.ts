import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {UserProfile} from '../models/userProfile';
import {UserProfileSignalRService} from '../services/user-profile-signal-r.service';

@Component({
  selector: 'app-user-profile-form',
  templateUrl: './user-profile-form.component.html',
  styleUrls: ['./user-profile-form.component.scss']
})
export class UserProfileFormComponent implements OnInit {

  private userProfile = new UserProfile();

  constructor(private userProfileSignalRService: UserProfileSignalRService, private cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.userProfileSignalRService.userProfile$.subscribe(userProfile => {
      this.userProfile = userProfile;
      this.cd.detectChanges();
    });
    this.userProfileSignalRService.startConnection();
  }

  onSubmit() {
    const json = JSON.stringify(this.userProfile);

    this.userProfileSignalRService.save(json);
  }
}
