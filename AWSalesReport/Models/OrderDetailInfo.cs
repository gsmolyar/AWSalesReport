using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWSalesReport
{
    public class OrderDetailInfo
    {
        public String ProductName { get; set; }
        public String ProductID { get; set; }
        public String OrderQty { get; set; }
        public String UnitPrice { get; set; }
        public String Discount { get; set; }
        public String LineTotal { get; set; }

    }
}