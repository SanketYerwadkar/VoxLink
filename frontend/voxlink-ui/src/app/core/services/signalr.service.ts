import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: HubConnection | null = null;

  constructor() {}

  startConnection(): Promise<void> {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .withAutomaticReconnect()
      .build();

    return this.hubConnection.start();
  }

  stopConnection(): Promise<void> {
    return this.hubConnection?.stop() || Promise.resolve();
  }

  on<T>(methodName: string, callback: (data: T) => void): void {
    this.hubConnection?.on(methodName, callback);
  }

  off(methodName: string): void {
    this.hubConnection?.off(methodName);
  }

  invoke<T>(methodName: string, ...args: any[]): Promise<T> {
    if (!this.hubConnection) {
      throw new Error('Hub connection not established');
    }
    return this.hubConnection.invoke<T>(methodName, ...args);
  }

  getConnectionState(): string {
    return this.hubConnection?.state || 'Disconnected';
  }
}
