import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { Observable, BehaviorSubject, of } from 'rxjs';
import { Invoice } from "../models/invoice";
import { catchError, finalize } from 'rxjs/operators';
import { InvoiceService } from "../services/invoice/invoice.service";

export class InvoicesDS implements DataSource<Invoice> {

  public invoicesSubject = new BehaviorSubject<Invoice[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();

  constructor(private invoiceSvc: InvoiceService) { }

  connect(collectionViewer: CollectionViewer): Observable<Invoice[]> {
    return this.invoicesSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.invoicesSubject.complete();
    this.loadingSubject.complete();
  }

  loadInvoices(filter = '', field = 'account',
    sortDirection = 'asc', pageIndex = 0, pageSize = 5) {

    this.loadingSubject.next(true);

    this.invoiceSvc.findInvoices(filter, field, sortDirection,
      pageIndex, pageSize).pipe(
        //catchError(() => of([])),
        finalize(() => this.loadingSubject.next(false))
      )
      .subscribe(invoices => this.invoicesSubject.next(invoices));
  }
}
