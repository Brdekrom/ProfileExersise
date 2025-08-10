// src/app/shared/confirm-dialog.component.ts
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

export interface ConfirmData {
  title?: string;
  message?: string;
  confirmText?: string;
  cancelText?: string;
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data?.title || 'Bevestigen' }}</h2>
    <div mat-dialog-content>{{ data?.message || 'Weet je het zeker?' }}</div>
    <div mat-dialog-actions align="end">
      <button mat-button (click)="close(false)">{{ data?.cancelText || 'Annuleren' }}</button>
      <button mat-raised-button color="warn" (click)="close(true)">{{ data?.confirmText || 'OK' }}</button>
    </div>
  `
})
export class ConfirmDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ConfirmData,
    private ref: MatDialogRef<ConfirmDialogComponent>
  ) {}
  close(v: boolean) { this.ref.close(v); }
}
