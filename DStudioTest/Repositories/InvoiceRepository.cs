using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DStudioTest.Model.BusinessEntities;
using System.IO;
using System.Text;
using DStudioTest.Utils;

namespace DStudioTest.Repositories
{
    /// <summary>
    /// Repository of invoices
    /// </summary>
    public class InvoiceRepository : IRepository<Invoice>
    {
        /// <summary>
        /// Path to storage file
        /// </summary>
        private static readonly string file = @"c:\invoices.csv";

        public InvoiceRepository()
        {
        }

        /// <summary>
        /// Creating a new invoice
        /// </summary>
        /// <param name="item">new invoice</param>
        public void Create(Invoice item)
        {
            InvoiceData.Instance.AddInvoice(item);
            string[] lines = File.ReadAllLines($"{file}");
            lines = lines.Concat(new string[] { item.ToString() }).ToArray();
            File.WriteAllLines($"{file}", lines);
        }

        /// <summary>
        /// Get invoices by filter, sorting, pagination
        /// </summary>
        /// <param name="parameters">array parameters (part of account number, order of soring, current page and page size)</param>
        /// <returns>list of invoices</returns>
        public IEnumerable<Invoice> GetByFilter(params string[] parameters)
        {
            if (parameters.Length != 5)
                throw new ArgumentException("Five parameters are required");
            int pageSize;
            int page;
            string filter;
            string field;
            string order;
            try
            {
                filter = parameters[0];
                field = parameters[1];
                order = parameters[2];
                page = System.Convert.ToInt32(parameters[3]);
                pageSize = System.Convert.ToInt32(parameters[4]);
            }
            catch
            {
                throw new ArgumentException("Converting error");
            }
            List<Invoice> ret = InvoiceData.Instance.GetAllInvoices().Values.ToList();
            if (!string.IsNullOrEmpty(filter))
                ret = ret.Where(x => x.account.Contains(filter)).ToList();
            ret = HelperSort.OrderByDynamic(ret, field.ToLower(), order.ToLower()).ToList();
            return ret
                .Skip((page) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        /// <summary>
        /// Get all invoices
        /// </summary>
        /// <returns>list of invoices</returns>
        public List<Invoice> GetAll()
        {
            return InvoiceData.Instance.GetAllInvoices().Values.ToList();
        }

        /// <summary>
        /// Get invoice by id (account number)
        /// </summary>
        /// <param name="id">account number</param>
        /// <returns>invoice</returns>
        public Invoice GetByKey(string id)
        {
            return InvoiceData.Instance.GetInvoiceById(id);
        }

        /// <summary>
        /// Update invoice
        /// </summary>
        /// <param name="item">invoice</param>
        /// <returns>message for UI</returns>
        public string Update(Invoice item)
        {
            if (InvoiceData.Instance.GetInvoiceById(item.account) != null)
            {
                string[] lines = File.ReadAllLines($"{file}");
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(';');
                    if (parts[1] == item.account)
                    {
                        lines[i] = item.ToString();
                    }
                }
                InvoiceData.Instance.UpdateInvoice(item);
                File.WriteAllLines($"{file}", lines);
                return $"Invoice {item.account} was updated";
            }
            else
            {
                Create(item);
                return $"Invoice {item.account} was created";
            }
        }

        /// <summary>
        /// Get count of all invoices in storage
        /// </summary>
        /// <returns>count</returns>
        public int GetCount()
        {
            return InvoiceData.Instance.GetAllInvoices().Count;
        }
    }
}
