using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DStudioTest.Model.BusinessEntities
{
    /// <summary>
    /// Processing statuses
    /// </summary>
    public enum ProcessingStatus
    {
        [Description("Новый")]
        New = 1,
        [Description("Оплачен")]
        Paid = 2,
        [Description("Отменен")]
        Сancelled = 3
    }
}
