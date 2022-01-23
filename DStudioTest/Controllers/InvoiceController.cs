using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DStudioTest.Model.BusinessEntities;
using DStudioTest.Repositories;
using DStudioTest.Utils;

namespace DStudioTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        /// <summary>
        /// Get all invoices from storage
        /// </summary>
        /// <returns>list of all invoices</returns>
        [HttpGet]
        public List<Invoice> GetAllInvoices()
        {
            InvoiceRepository repo = new InvoiceRepository();;
            return repo.GetAll();
        }

        /// <summary>
        /// Get invoice by id (account number)
        /// </summary>
        /// <param name="id">account number</param>
        /// <returns>invoice</returns>
        [HttpGet("{id}")]
        public Invoice GetInvoice([FromRoute] string id)
        {
            InvoiceRepository repo = new InvoiceRepository();
            Invoice invoice = repo.GetByKey(id);
            return invoice;
        }

        /// <summary>
        /// Update or create invoice
        /// </summary>
        /// <param name="id">account number (0 if new invoice)</param>
        /// <param name="created">created date of new invoice (or new date for updating)</param>
        /// <param name="amount">amount</param>
        /// <param name="status">processing status</param>
        /// <param name="method">payment method</param>
        /// <returns>message foy UI</returns>
        [HttpGet("{id}/{created}/{amount}/{status}/{method}")]
        public ActionResult Update([FromRoute] string id, [FromRoute] DateTime created, [FromRoute] float amount, [FromRoute] int status, [FromRoute] int method)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                InvoiceRepository repo = new InvoiceRepository();
                Invoice inv = new Invoice()
                {
                    created = DateTime.Now,
                    account = id,
                    status = (ProcessingStatus)status,
                    amount = amount,
                    method = (PaymentMethod)method
                };
                return Ok(repo.Update(inv));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPost]
        public ActionResult Update([FromBody] Invoice item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                InvoiceRepository repo = new InvoiceRepository();
                return Ok(repo.Update(item));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /*[HttpGet("{id}/{created}/{amount}/{status}/{method}")]
        public void Create([FromRoute] string id, [FromRoute] DateTime created, [FromRoute] float amount, [FromRoute] int status, [FromRoute] int method)
        {
            InvoiceRepository repo = new InvoiceRepository();
            Invoice inv = new Invoice()
            {
                created = created,
                account = id,
                status = (ProcessingStatus)status,
                amount = amount,
                method = (PaymentMethod)method
            };
            repo.Create(inv);
        }*/

        /// <summary>
        /// Get invoices by filter of account field including sort by account and pagination
        /// </summary>
        /// <param name="order">direction of sorting</param>
        /// <param name="page">current page</param>
        /// <param name="size">page size</param>
        /// <param name="filter">part of account number</param>
        /// <returns>list of invoices</returns>
        /*[HttpGet]
        [Route("find/{order}/{page}/{size}")]
        [Route("find/{order}/{page}/{size}/{filter}")]
        public IEnumerable<Invoice> GetInvoicesByFilter([FromRoute] string order, [FromRoute] int page, [FromRoute] int size, [FromRoute] string filter)
        {
            InvoiceRepository repo = new InvoiceRepository();
            return repo.GetByFilter(new string[]
            {
                filter,
                order,
                page.ToString(),
                size.ToString()
            });
        }*/

        /// <summary>
        /// Get invoices by filter of account field including sort by account and pagination
        /// </summary>
        /// <param name="field">field of sorting</param>
        /// <param name="order">direction of sorting</param>
        /// <param name="page">current page</param>
        /// <param name="size">page size</param>
        /// <param name="filter">part of account number</param>
        /// <returns>list of invoices</returns>
        [HttpGet]
        [Route("find/{field}/{order}/{page}/{size}")]
        [Route("find/{field}/{order}/{page}/{size}/{filter}")]
        public IEnumerable<Invoice> GetInvoicesByFilter([FromRoute] string field, [FromRoute] string order, [FromRoute] int page, [FromRoute] int size, [FromRoute] string filter)
        {
            InvoiceRepository repo = new InvoiceRepository();
            return repo.GetByFilter(new string[]
            {
                filter,
                field,
                order,
                page.ToString(),
                size.ToString()
            });
        }

        /// <summary>
        /// Get count of all invoices
        /// </summary>
        /// <returns>count</returns>
        [HttpGet("count")]
        public int GetInvoices()
        {
            InvoiceRepository repo = new InvoiceRepository();
            return repo.GetCount();
        }

        /// <summary>
        /// Get dictionary of payment methods
        /// </summary>
        /// <returns>list of payment methods</returns>
        [HttpGet("paymentmethods")]
        public List<KeyValuePair<int, string>> GetPaymentMethods()
        {
            return Extensions.GetEnum2<PaymentMethod>();
        }

        /// <summary>
        /// Get dictionary of processing statuses
        /// </summary>
        /// <returns>list of processing statuses</returns>
        [HttpGet("processingstatuses")]
        public List<KeyValuePair<int, string>> GetProcessingStatuses()
        {
            return Extensions.GetEnum2<ProcessingStatus>();
        }
    }
}
