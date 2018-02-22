using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AWSalesReport.Controllers_Api
{
    public class OrderController : ApiController
    {

        [HttpPost()]
        [Route("api/Order/Search")]
        public IHttpActionResult Search(
          OrderSearch searchEntity)
        {

            IHttpActionResult ret = null;
            AWSlsRptViewModel vm = new AWSlsRptViewModel();

            // Search for Products
            vm.SearchEntity = searchEntity;
            vm.LoadOrders();
            if (vm.Orders.Count > 0)
            {
                ret = Ok(vm.Orders);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }

        [HttpPost()]
        [Route("api/Order/OrderDetails")]
        public IHttpActionResult OrderDetails(OrderDetailSearch searchEntity)
        {

            IHttpActionResult ret = null;
            AWSlsRptViewModel vm = new AWSlsRptViewModel();

            // Search for Products
            vm.SearchDetailEntity = searchEntity;
            vm.LoadOrderDetails();
            if (vm.OrderDetails.Count > 0)
            {
                ret = Ok(vm.OrderDetails);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }

        // GET api/<controller>/5
        //[HttpGet()]
        //public IHttpActionResult Get(int id)
        //{
        //    IHttpActionResult ret;
        //    Order ord = new Order();
        //    AWSalesRptViewModel vm = new AWSalesRptViewModel();

        //    ord = vm.Get(id);
        //    if (ord != null)
        //    {
        //        ret = Ok(ord);
        //    }
        //    else
        //    {
        //        ret = NotFound();
        //    }

        //    return ret;
        //}


        //// GET: api/Order
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Order/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Order
        //public void Post([FromBody]string value)
        //{
        //}


    }
}
