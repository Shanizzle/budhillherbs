using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BudhillHerbs.WebApi.Controllers
{
    public class ListController : ApiController
    {
        // GET api/List
        [HttpGet]
        [Route("api/list/getTinctures")]
        public DataTable GetTinctures()
        {
            DataSet ds = new DataSet();

            //string connStr = @"Server=KORTEGO-VM2\SQLSERVER2017;Database=BuddhillHerbs;uid=budhill_admin;pwd=stayout";
            string connStr = @"Server=68.66.228.7;Database=kennetho_test_bhh;uid=kennetho_bhh_testadmin;pwd=gnCk#862";

            //string connStr = ConfigurationManager.ConnectionStrings["WebApiApp"].ConnectionString;

            SqlConnection connection = new SqlConnection(connStr);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "select * from Tinctures";

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connection.Open();
            adapter.Fill(ds);

            connection.Close();

            return ds.Tables[0];

        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}