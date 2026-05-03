import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading',
  template: `
    <div class="loading-container">
      <mat-spinner></mat-spinner>
      <p *ngIf="message">{{ message }}</p>
    </div>
  `,
  styles: [`
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 20px;
    }
    
    p {
      margin-top: 16px;
      color: #666;
    }
  `]
})
export class LoadingComponent {
  @Input() message: string = '';
}
