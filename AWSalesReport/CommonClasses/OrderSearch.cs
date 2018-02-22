using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWSalesReport
{
    public class OrderSearch
    {
        public string CustomerName { get; set; }
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
        public string ShipDateFrom { get; set; }
        public string ShipDateTo { get; set; }
        public string DueDateFrom { get; set; }
        public string DueDateTo { get; set; }

    }
}