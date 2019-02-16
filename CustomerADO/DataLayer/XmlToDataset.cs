using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataLayer
{
    public class XmlToDataSet
    {
        public DataSet ReadXmlToDataSet(string File)
        {
            try
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(File, XmlReadMode.InferSchema);
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string InsertIntoDatabase(DataSet ds)
        {
            try
            {
                string Result = null ;
                foreach (DataTable dt in ds.Tables)
                {

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDBConnectionString"].ToString());

                    // Creating Query for Table Creation
                    string Query = CreateTableQuery(dt);

                    con.Open();

                    // Deletion of Table if already Exist
                    SqlCommand cmd = new SqlCommand("IF OBJECT_ID('dbo." +
                    dt.TableName + "', 'U') IS NOT NULL DROP TABLE dbo." + dt.TableName + ";", con);
                    cmd.ExecuteNonQuery();
                    
                    // Table Creation
                    cmd = new SqlCommand(Query, con);
                    int check = cmd.ExecuteNonQuery();
                    if (check != 0)
                    {
                        // Copy Data from DataTable to Sql Table
                        using (var bulkCopy = new SqlBulkCopy
                              (con.ConnectionString, SqlBulkCopyOptions.KeepIdentity))
                        {
                            // my DataTable column names match my SQL Column names,
                            // so I simply made this loop.
                            //However if your column names don't match,
                            //just pass in which datatable name matches the SQL column name in Column Mappings
                            foreach (DataColumn col in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            }

                            bulkCopy.BulkCopyTimeout = 600;
                            bulkCopy.DestinationTableName = dt.TableName;
                            bulkCopy.WriteToServer(dt);
                        }
                        Result = "Created Successfully";

                    }
                    con.Close();
                }
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }
        // Getting Query for Table Creation
        public string CreateTableQuery(DataTable table)
        {
            string sqlsc = "CREATE TABLE " + table.TableName + "(";

            for (int i = 0; i < table.Columns.Count; i++)
            {
                sqlsc += "[" + table.Columns[i].ColumnName + "]";
                string columnType = table.Columns[i].DataType.ToString();
                switch (columnType)
                {
                    case "System.Int32":
                        sqlsc += " int ";
                        break;
                    case "System.Int64":
                        sqlsc += " bigint ";
                        break;
                    case "System.Int16":
                        sqlsc += " smallint";
                        break;
                    case "System.Byte":
                        sqlsc += " tinyint";
                        break;
                    case "System.Decimal":
                        sqlsc += " decimal ";
                        break;
                    case "System.DateTime":
                        sqlsc += " datetime ";
                        break;
                    case "System.String":
                    default:
                        sqlsc += string.Format(" nvarchar({0}) ",
                        table.Columns[i].MaxLength == -1 ? "max" :
                        table.Columns[i].MaxLength.ToString());
                        break;
                }
                if (table.Columns[i].AutoIncrement)
                    sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() +
                    "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";
                if (!table.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";

            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";
        }
       
    }
}
