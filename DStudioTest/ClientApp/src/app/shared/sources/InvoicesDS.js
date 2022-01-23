"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.InvoicesDS = void 0;
var rxjs_1 = require("rxjs");
var operators_1 = require("rxjs/operators");
var InvoicesDS = /** @class */ (function () {
    function InvoicesDS(invoiceSvc) {
        this.invoiceSvc = invoiceSvc;
        this.invoicesSubject = new rxjs_1.BehaviorSubject([]);
        this.loadingSubject = new rxjs_1.BehaviorSubject(false);
        this.loading$ = this.loadingSubject.asObservable();
    }
    InvoicesDS.prototype.connect = function (collectionViewer) {
        return this.invoicesSubject.asObservable();
    };
    InvoicesDS.prototype.disconnect = function (collectionViewer) {
        this.invoicesSubject.complete();
        this.loadingSubject.complete();
    };
    InvoicesDS.prototype.loadInvoices = function (filter, field, sortDirection, pageIndex, pageSize) {
        var _this = this;
        if (filter === void 0) { filter = ''; }
        if (field === void 0) { field = 'account'; }
        if (sortDirection === void 0) { sortDirection = 'asc'; }
        if (pageIndex === void 0) { pageIndex = 0; }
        if (pageSize === void 0) { pageSize = 5; }
        this.loadingSubject.next(true);
        this.invoiceSvc.findInvoices(filter, field, sortDirection, pageIndex, pageSize).pipe(
        //catchError(() => of([])),
        (0, operators_1.finalize)(function () { return _this.loadingSubject.next(false); }))
            .subscribe(function (invoices) { return _this.invoicesSubject.next(invoices); });
    };
    return InvoicesDS;
}());
exports.InvoicesDS = InvoicesDS;
//# sourceMappingURL=InvoicesDS.js.map