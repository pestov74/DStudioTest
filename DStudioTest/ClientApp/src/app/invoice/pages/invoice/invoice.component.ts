import { Component, OnInit, OnDestroy } from '@angular/core';
import { Invoice } from "../../../shared/models/invoice";
import { ActivatedRoute } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { InvoiceService } from "../../../shared/services/invoice/invoice.service";
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';

import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
import { default as _rollupMoment, Moment } from 'moment';

const moment = _rollupMoment || _moment;

// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: 'DD.MM.YYYY HH:mm',
  },
  display: {
    dateInput: 'DD.MM.YYYY HH:mm',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    { provide: MAT_DATE_LOCALE, useValue: 'en-US' }
  ]
})
export class InvoiceComponent implements OnInit, OnDestroy {

  date = new FormControl(moment());

  account = new FormControl('', [
    Validators.required,
    Validators.maxLength(20)
  ]);

  amount = new FormControl();

  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.date.value;
    ctrlValue.year(normalizedYear.year());
    this.date.setValue(ctrlValue);
  }

  chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value;
    ctrlValue.month(normalizedMonth.month());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }

  methods: any[];
  method: string;
  statuses: any[];
  status: string;

  public curInvoice: Invoice;
  public subscription: Subscription;


  constructor(private invoiceSvc: InvoiceService,
    private route: ActivatedRoute,
    private location: Location) {
  }

  ngOnInit() {
    this.statuses = this.invoiceSvc.getProcessingStatuses2();
    this.methods = this.invoiceSvc.getPaymentMethods2();

    this.subscription = this.invoiceSvc.invoiceData$.subscribe(x => this.curInvoice = x);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onBack(): void {
    this.location.back();
  }

  onSave(): void {
    this.invoiceSvc.updateInvoice(this.curInvoice).subscribe(result => {
    });
    this.location.back();
  }

}
