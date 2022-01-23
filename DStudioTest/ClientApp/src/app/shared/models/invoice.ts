import { Injectable } from '@angular/core';

@Injectable()
export class Invoice {

  public account: string;
  public created: Date = new Date();
  public amount: number;
  public method: string;
  public status: string;

  constructor() { }
}
