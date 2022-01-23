import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { map} from 'rxjs/operators';
import { Invoice } from "../../models/invoice";
import { DatePipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  datepipe: DatePipe = new DatePipe('en-US');

  public methods$ = new Subject<any[]>();
  public statuses$ = new Subject<any[]>();

  public methods: any[] = [];
  public statuses: any[] = [];

  public curInvoice = new BehaviorSubject<Invoice>(new Invoice());
  invoiceData$ = this.curInvoice.asObservable()

  private url: string;

  default_post_headers = {
    'Access-Control-Allow-Headers': 'Content-Type',
    'Content-Type': 'application/json',
    'Access-Control-Allow-Methods': 'POST, OPTIONS',
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
  }

  findInvoices(
    filter = '', field = 'account', sortOrder = 'asc',
    pageNumber = 0, pageSize = 5): Observable<Invoice[]> {

    let url: string = 'api/invoice/find/';
    url = url.concat(field, '/', sortOrder, '/', pageNumber.toString(), '/', pageSize.toString());
    if (filter != '')
      url = url.concat('/', filter);

    return this.http.get<Invoice[]>(this.url + url)
      .pipe(
        map((data) => <Invoice[]>Object.values(data))
      );
  }

  updateInvoice(invoice: Invoice): Observable<string> {
    let url: string = 'api/invoice/';
    url = url.concat(invoice.account,
      '/', this.datepipe.transform(invoice.created, 'yyyy-MM-dd'),
      '/', invoice.amount.toString(),
      '/', invoice.status.toString(),
      '/', invoice.method.toString());

    return this.http.get<any>(this.url + url);
  }

  updateInvoicePost(invoice: Invoice): Observable<any> {
    const body = JSON.stringify(invoice);
    return this.http.post(this.url + "api/invoice", body, { headers: this.default_post_headers, withCredentials: false });
  }

  getInvoicesCount(): Observable<number> {
    let url: string = 'api/invoice/count';
    return this.http.get<number>(this.url + url);
  }

  getPaymentMethods2(): any[] {
    return [
      { name: 'Credit', value: 1 },
      { name: 'Debit', value: 2 },
      { name: 'Receipt', value: 3 },
    ];
  }

  getProcessingStatuses2(): any[] {
    return [
      { name: 'New', value: 1 },
      { name: 'Paid', value: 2 },
      { name: 'Cancelled', value: 3 },
    ];
  }

  public getCurInvoice(): Observable<Invoice> {
    return this.invoiceData$;
  }

  public updateCurInvoice(invoice: Invoice): void {
    this.curInvoice.next(invoice);
  }
}
