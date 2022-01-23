using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DStudioTest.Utils;
using DStudioTest.Model.BusinessEntities;
using System.IO;

namespace DStudioTest.Repositories
{
    /// <summary>
    /// Storage of invoices in memory
    /// </summary>
    public class InvoiceData  : Singleton<InvoiceData>
    {
        private static readonly object _loc_session = new object();
        private static readonly string file = @"c:\invoices.csv";

        private InvoiceData()
        {
            LoadInvoices();
        }

        private Dictionary<string, Invoice> invoices = new Dictionary<string, Invoice>();

        /// <summary>
        /// Load invoices from file
        /// </summary>
        public void LoadInvoices()
        {
            lock(_loc_session)
            {
                string line;
                using (var sr = new StreamReader(file, true))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Invoice invoice = new Invoice(line);
                        invoices.Add(invoice.account, invoice);
                    }
                }
            }
        }

        /// <summary>
        /// Get all invoices
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Invoice> GetAllInvoices()
        {
            return this.invoices;
        }

        /// <summary>
        /// Get invoice by id (account number)
        /// </summary>
        /// <param name="id">account number</param>
        /// <returns></returns>
        public Invoice GetInvoiceById(string id)
        {
            return this.invoices.FirstOrDefault(x => x.Key.Equals(id)).Value;
        }

        /// <summary>
        /// Update invoice in memory storage
        /// </summary>
        /// <param name="invoice">invoice</param>
        public void UpdateInvoice(Invoice invoice)
        {
            if (this.invoices.TryGetValue(invoice.account, out var inv))
            
            {
                inv.amount = invoice.amount;
                inv.status = invoice.status;
                inv.method = invoice.method;
                inv.created = invoice.created;
            }
        }

        /// <summary>
        /// Add new invoice
        /// </summary>
        /// <param name="invoice">invoice</param>
        public void AddInvoice(Invoice invoice)
        {
            this.invoices.Add(invoice.account, invoice);
        }
    }
}
