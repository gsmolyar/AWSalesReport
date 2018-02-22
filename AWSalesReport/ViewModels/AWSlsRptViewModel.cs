using AWSalesReport.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AWSalesReport
{
    public class AWSlsRptViewModel
    {
        #region Public Properties
        public OrderInfo Entity { get; set; }
        public List<OrderInfo> Orders { get; set; }
        public List<OrderDetailInfo> OrderDetails { get; set; }
        public OrderSearch SearchEntity { get; set; }
        public OrderDetailSearch SearchDetailEntity { get; set; }
        public string EventCommand { get; set; }
        public string EventArgument { get; set; }
        public bool IsValid { get; set; }
        public string PageMode { get; set; }
        public bool IsListAreaVisible { get; set; }
        public bool IsSearchAreaVisible { get; set; }
        public ModelStateDictionary Messages { get; set; }
        #endregion

        #region Constructor
        public AWSlsRptViewModel()
        {
            Init();
        }
        #endregion

        #region Init Method
        public void Init()
        {
            Orders = new List<OrderInfo>();
            OrderDetails = new List<OrderDetailInfo>();
            Entity = new OrderInfo();
            SearchEntity = new OrderSearch();

            EventCommand = string.Empty;
            EventArgument = string.Empty;
            IsValid = true;
            IsListAreaVisible = true;
            IsSearchAreaVisible = true;
            PageMode = PageConstants.LIST;
            Messages = new ModelStateDictionary();
        }
        #endregion

        #region LoadOrders Method
        public void LoadOrders()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSlsRptConnectionString"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("uspAWSlsReport_Orders", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CustomerName", SearchEntity.CustomerName));
                    command.Parameters.Add(new SqlParameter("@OrderDateFrom", SearchEntity.OrderDateFrom));
                    command.Parameters.Add(new SqlParameter("@OrderDateTo", SearchEntity.OrderDateTo));
                    command.Parameters.Add(new SqlParameter("@DueDateFrom", SearchEntity.DueDateFrom));
                    command.Parameters.Add(new SqlParameter("@DueDateTo", SearchEntity.DueDateTo));
                    command.Parameters.Add(new SqlParameter("@ShipDateFrom", SearchEntity.ShipDateFrom));
                    command.Parameters.Add(new SqlParameter("@ShipDateTo", SearchEntity.ShipDateTo));
                    //command.Parameters.AddWithValue("@CustomerName", SearchEntity.CustomerName);

                    // execute the command
                    SqlDataReader rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        OrderInfo order = new OrderInfo();
                        order.CustomerID = rdr.GetInt32(rdr.GetOrdinal("CustomerID")).ToString();
                        order.CustomerName = rdr.GetString(rdr.GetOrdinal("CustomerName"));
                        order.CustomerAcctNo = rdr.GetString(rdr.GetOrdinal("CustomerAcctNo"));
                        order.SalesOrderNumber = rdr.GetString(rdr.GetOrdinal("SalesOrderNumber"));
                        order.ShipToAddress = rdr.GetString(rdr.GetOrdinal("ShipToAddress"));
                        order.ShipMethod = rdr.GetString(rdr.GetOrdinal("ShipMethod"));
                        order.SubTotal = rdr.GetDecimal(rdr.GetOrdinal("SubTotal")).ToString();
                        order.TaxAmt = rdr.GetDecimal(rdr.GetOrdinal("TaxAmt")).ToString();
                        order.Freight = rdr.GetDecimal(rdr.GetOrdinal("Freight")).ToString();
                        order.TotalDue = rdr.GetDecimal(rdr.GetOrdinal("TotalDue")).ToString();
                        Orders.Add(order);
                    }
                    rdr.Close();

                }
                catch (Exception exp)
                {
                    throw exp;
                }
                finally
                {
                    conn.Close();
                }

            }
        }
        #endregion

        #region LoadOrderDetails Method
        public void LoadOrderDetails()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSlsRptConnectionString"].ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand("uspAWSlsReport_OrderDetails", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SalesOrderNumber", SearchDetailEntity.SalesOrderNumber));
                    //command.Parameters.AddWithValue("@SalesOrderNumber", SearchDetailEntity.SalesOrderNumber);

                    // execute the command
                    SqlDataReader rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        OrderDetailInfo orderDetail = new OrderDetailInfo();
                        orderDetail.ProductName = rdr.GetString(rdr.GetOrdinal("ProductName"));
                        orderDetail.ProductID = rdr.GetInt32(rdr.GetOrdinal("ProductID")).ToString();
                        orderDetail.OrderQty = rdr.GetInt16(rdr.GetOrdinal("OrderQty")).ToString();
                        orderDetail.UnitPrice = rdr.GetDecimal(rdr.GetOrdinal("UnitPrice")).ToString();
                        orderDetail.Discount = rdr.GetDecimal(rdr.GetOrdinal("Discount")).ToString();
                        orderDetail.LineTotal = rdr.GetDecimal(rdr.GetOrdinal("LineTotal")).ToString();
                        OrderDetails.Add(orderDetail);
                    }
                    rdr.Close();

                }
                catch (Exception exp)
                {
                    throw exp;
                }
                finally
                {
                    conn.Close();
                }

            }
        }
        #endregion

        #region HandleRequest Method
        public void HandleRequest()
        {
            LoadOrders();

            switch (EventCommand.ToLower())
            {
                case "search":
                    LoadOrders();
                    break;

                case "resetsearch":
                    ResetSearch();
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region ResetSearch Method
        public void ResetSearch()
        {
            //SearchEntity = new ProductSearch();
            //Get();
        }
        #endregion

        #region SetUIState Method
        protected void SetUIState(string state)
        {
            PageMode = state;

            IsListAreaVisible = (PageMode == "List");
            IsSearchAreaVisible = (PageMode == "List");
        }
        #endregion
    }
}