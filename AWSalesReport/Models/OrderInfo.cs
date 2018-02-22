using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWSalesReport
{
    public class OrderInfo
    {
        public String CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String CustomerAcctNo { get; set; }
        public String SalesOrderNumber { get; set; }
        public String ShipToAddress { get; set; }
        public String ShipMethod { get; set; }
        public String SubTotal { get; set; }
        public String TaxAmt { get; set; }
        public String Freight { get; set; }
        public String TotalDue { get; set; }
    }
}