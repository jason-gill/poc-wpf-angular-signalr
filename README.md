# WPF, Angular, and SignalR 
A simple POC showing how to connect a WPF app with an Angular Web Site using SignalR

## Notes
The WPF app contains a self hosted SignalR server.  The WFP view connects to the SignalR server as a client.  There is a button on the view that lauches a web browser to the Angular web site.  The Angular web site connects to the SignalR server and is sent data from the WPF view.  Changes maded on the Angular web site are then sent back to the WFP view via the SignalR server.

## Things to consider
- Should the WPF app host the SignalR server or should it be hosted in IIS?
- Authenication and Authorization
  - https://docs.microsoft.com/en-us/aspnet/signalr/overview/security/hub-authorization
  - https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-2.2
 - SSL and any other security concerns