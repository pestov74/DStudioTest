import { AfterViewInit, ElementRef, Component, OnInit, ViewChild } from '@angular/core';
import { Invoice } from "../../../shared/models/invoice";
import { InvoiceService } from "../../../shared/services/invoice/invoice.service";
import { InvoicesDS } from "../../../shared/sources/InvoicesDS";
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { debounceTime, distinctUntilChanged, startWith, tap, delay } from 'rxjs/operators';
import { merge, fromEvent } from "rxjs";
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html',
  styleUrls: ['./invoices.component.css']
})
export class InvoicesComponent implements OnInit, AfterViewInit {

  datepipe: DatePipe = new DatePipe('en-US');

  data: InvoicesDS;
  displayedColumns = ["Created", "Account", "Method", "Amount", "Status"];
  methods: any[];
  statuses: any[];
  countInvoices: number;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;


  constructor(private invoiceSvc: InvoiceService,
    private router: Router) {
  }

  ngOnInit() {
    this.data = new InvoicesDS(this.invoiceSvc);
    this.data.loadInvoices('');

    this.methods = this.invoiceSvc.getPaymentMethods2();
    this.statuses = this.invoiceSvc.getProcessingStatuses2();

    this.invoiceSvc.getInvoicesCount().subscribe(res => this.countInvoices = res);
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;

          this.loadInvoices();
        })
      )
      .subscribe();

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.loadInvoices())
      )
      .subscribe();
  }

  loadInvoices() {
    this.data.loadInvoices(
      this.input.nativeElement.value,
      this.sort.active,
      this.sort.direction,
      this.paginator.pageIndex,
      this.paginator.pageSize);
  }

  onRowClicked(row: Invoice) {
    this.invoiceSvc.updateCurInvoice(row);
    this.router.navigate(['/invoice', row.account]);
  }

  getMethod(id: number): string {
    return this.methods.find(x => x.value == id).name;
  }

  getStatus(id: number): string {
    return this.statuses.find(x => x.value == id).name;
  }

  onCreate() {
    this.invoiceSvc.updateCurInvoice(new Invoice());
    this.router.navigate(['/invoice', '0']);
  }

}
