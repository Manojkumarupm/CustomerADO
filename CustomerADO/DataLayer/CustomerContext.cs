using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CustomerContext
    {
        public List<Customer> GetCustomer()
        {
            List<Customer> lst = new List<Customer>();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());

                SqlCommand sqlcom = new SqlCommand();
                sqlcom.CommandText = "GetAllCustomers";
                sqlcom.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                sqlcom.Connection = con;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcom);

                sda.Fill(ds);
                lst = ds.Tables[0].DataTableToList<Customer>();

            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public string AddCustomer(Customer c)
        {
            string result = "";
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"InsertCustomer";

                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@CustName", c.CustomerName);
                cmd.Parameters.AddWithValue("@CustAddress", c.CustomerAddress);

                cmd.Parameters.AddWithValue("@DOB", c.DOB);
                cmd.Parameters.AddWithValue("@Salary", c.Salary);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                result = "Record Added Successfully !";
            }
            catch (Exception ex)
            {
                result = "Error" + ex.InnerException;
            }

            return result;
        }
        public List<Customer> SearchCustomer(int Id)
        {
            List<Customer> lst = new List<Customer>();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());

                SqlCommand sqlcom = new SqlCommand();
                sqlcom.CommandText = "GetCustomers";
                sqlcom.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                sqlcom.Connection = con;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcom);
                sda.SelectCommand.Parameters.AddWithValue("@CustId", Id);
                sda.Fill(ds);
                lst = ds.Tables[0].DataTableToList<Customer>();
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public List<Customer> GetCustomerYoungerByDOB(DateTime dob)
        {
            List<Customer> lst = new List<Customer>();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());

                SqlCommand sqlcom = new SqlCommand();
                sqlcom.CommandText = "GetCustomerAfterGivenDate";
                sqlcom.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                sqlcom.Connection = con;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcom);
                if (dob != null)
                    sda.SelectCommand.Parameters.AddWithValue("@DOB", dob.Date);
                sda.Fill(ds);
                lst = ds.Tables[0].DataTableToList<Customer>();
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public string UpdateCustomer(Customer c)
        {
            string result = "";
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UpdateCustomer";

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustId", c.CustomerId);
                cmd.Parameters.AddWithValue("@CustName", c.CustomerName);
                cmd.Parameters.AddWithValue("@CustAddress", c.CustomerAddress);

                cmd.Parameters.AddWithValue("@DOB", c.DOB);
                cmd.Parameters.AddWithValue("@Salary", c.Salary);
                cmd.Connection = con;
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                result = "Record Updated Successfully !";
            }
            catch (Exception ex)
            {
                result = "Error" + ex.InnerException;
            }

            return result;
        }
        public string DeleteCustomerDetails(int Id)
        {
            string Result;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());
                SqlCommand cmd = new SqlCommand();


                cmd.CommandText = "DeleteCustomer";
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustID", Id);
                con.Open();
                cmd.ExecuteNonQuery();
                Result = "Record Deleted Successfully";
            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }
    }
}
