"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var invoice_service_1 = require("./invoice.service");
describe('InvoiceService', function () {
    beforeEach(function () { return testing_1.TestBed.configureTestingModule({}); });
    it('should be created', function () {
        var service = testing_1.TestBed.get(invoice_service_1.InvoiceService);
        expect(service).toBeTruthy();
    });
});
//# sourceMappingURL=invoice.service.spec.js.map