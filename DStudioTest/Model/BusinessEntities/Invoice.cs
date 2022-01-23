using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace DStudioTest.Model.BusinessEntities
{
    /// <summary>
    /// Invoice business(repository) entity
    /// </summary>
    public class Invoice
    {
        public Invoice()
        {

        }

        /// <summary>
        /// Constructor from string representation
        /// </summary>
        /// <param name="str">string line from file</param>
        public Invoice(string str)
        {
            string[] parts = str.Split(";");
            created = System.Convert.ToDateTime(parts[0]);
            account = parts[1];
            status = (ProcessingStatus)System.Convert.ToInt32(parts[2]);
            amount = System.Convert.ToSingle(parts[3], new CultureInfo("en-US"));
            method = (PaymentMethod)System.Convert.ToInt32(parts[4]);
        }

        /// <summary>
        /// Created field
        /// </summary>
        public DateTime created { get; set; }

        /// <summary>
        /// Account field
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// Amount field
        /// </summary>
        public float amount { get; set; }

        /// <summary>
        /// Payment method field
        /// </summary>
        public PaymentMethod method { get; set; }

        /// <summary>
        /// Processing status field
        /// </summary>
        public ProcessingStatus status { get; set; }

        /// <summary>
        /// Convert invoice to string representation for storage file
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(";", new object[] {
                created.ToString(),
                account,
                Enum.Format(typeof(PaymentMethod), Enum.Parse(typeof(PaymentMethod), method.ToString()), "d"),
                amount.ToString(),
                Enum.Format(typeof(ProcessingStatus), Enum.Parse(typeof(ProcessingStatus), status.ToString()), "d"),
            });
        }
    }
}
