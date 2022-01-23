using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DStudioTest.Model.BusinessEntities
{
    /// <summary>
    /// Payment methods
    /// </summary>
    public enum PaymentMethod : int
    {
        [Description("Кредитная карта")]
        Credit = 1,
        [Description("Дебитовая карта")]
        Debit = 2,
        [Description("Электронный чек")]
        Receipt = 3
    }
}
