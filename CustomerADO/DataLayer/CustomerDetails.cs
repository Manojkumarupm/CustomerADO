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
    public class CustomerDetails
    {
        private SqlConnection con = null;
        DataSet ds;
        SqlDataAdapter sda;
        public CustomerDetails()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());
            ds = new DataSet();
            sda = new SqlDataAdapter("Select * from Customers", con);
            sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            GetCustomer();
        }
        public void GetCustomer()
        {
            try
            {
                con.Open();
                sda.Fill(ds, "Customers");
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public List<Customer> GetCustomerList()
        {
            List<Customer> lst = new List<Customer>();
            try
            {
                lst = ds.Tables[0].DataTableToList<Customer>();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return lst;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public string AddCustomer(Customer c)
        {
            string result = "";
            try
            {
                // Get data row information

                DataRow dr = ds.Tables[0].NewRow();
                List<int> levels = ds.Tables[0].AsEnumerable().Select(al => al.Field<int>("CustomerId")).Distinct().ToList();

                int max = levels.Max();

                dr["CustomerId"] = max + 1;
                dr["CustomerName"] = c.CustomerName;
                dr["CustomerAddress"] = c.CustomerAddress;
                dr["DOB"] = c.DOB;
                dr["Salary"] = c.Salary;
                ds.Tables[0].Rows.Add(dr);
                con.Open();
                SqlCommandBuilder scb = new SqlCommandBuilder(sda);
                scb.DataAdapter.Update(ds, "Customers");
            }
            catch (Exception ex)
            {
                result = "Error" + ex.InnerException;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public Customer SearchCustomer(int Id)
        {
            Customer c = new Customer();
            DataRow dr = ds.Tables[0].Rows.Find(Id);
            if (dr != null)
            {
                c.CustomerName = dr["CustomerName"].ToString();
                c.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                c.CustomerAddress = dr["CustomerAddress"].ToString();
                c.DOB = DateTime.Parse(dr["DOB"].ToString());
                c.Salary = Decimal.Parse(dr["Salary"].ToString());
            }
            return c;
        }

        public string UpdateCustomer(Customer c)
        {
            string result = "";
            try
            {
                DataRow dr = ds.Tables[0].Rows.Find(c.CustomerId);
                dr["CustomerName"] = c.CustomerName;
                dr["Salary"] = c.Salary.ToString();
                dr["DOB"] = c.DOB.ToString();
                dr["CustomerAddress"] = c.CustomerAddress;
                con.Open();
                SqlCommandBuilder scb = new SqlCommandBuilder(sda);
                scb.DataAdapter.Update(ds, "Customers");
                //scb.Update(ds,"Customers");
            }
            catch (Exception ex)
            {
                result = "Error" + ex.InnerException;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public string DeleteCustomerDetails(int Id)
        {
            string Result;
            try
            {
                DataRow dr = ds.Tables[0].Rows.Find(Id);
                dr.Delete();
                con.Open();
                SqlCommandBuilder scb = new SqlCommandBuilder(sda);
                scb.DataAdapter.Update(ds, "Customers");
                Result = "Deleted Successfully";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return Result;
        }
    }
}
