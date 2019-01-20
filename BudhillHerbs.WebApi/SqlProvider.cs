using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BudhillHerbs.WebApi
{
    public class SqlProvider
    {

        public static SqlConnection globalConnection;
        private string connectionString;
        private string storedProc;
        private List<SqlParameter> parameters;


        /* Class creation */

        public SqlProvider()
        {

        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="connectionString">Connection String defined in web.config</param>
        /// <param name="storedProc">Stored Procedure Name</param>
        public SqlProvider(string connectionString, string storedProc)
        {
            this.connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            this.storedProc = storedProc;
        }


        /// <summary>
        /// Create SqlConnection based on Connection String
        /// </summary>
        /// <param name="connectionString">Connection String defined in web.config</param>
        /// <returns>.NET `SqlConnection` class</returns>
        public SqlConnection Getconnection(string connectionString)
        {
            SqlConnection cnn;
            string constring;
            try
            {
                constring = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
                cnn = new SqlConnection(constring);
            }
            catch (Exception)
            {
                throw;
            }
            return cnn;
        }


        /// <summary>
        /// Executes stored procedure.
        /// </summary>
        /// <returns>.NET SqlDataReader Class</returns>
        public IDataReader ExecuteReader()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(this.storedProc, connection))
                {
                    this.buildParameters(command);
                    command.CommandType = CommandType.StoredProcedure;
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }


        /// <summary>
        /// Executes stored procedure.
        /// </summary> 
        /// <returns>.NET DataSet Class</returns>
        public DataSet ExecuteDataSet()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(this.storedProc, connection))
                {
                    this.buildParameters(command);
                    command.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(ds);
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
                    return ds;
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }


        /// <summary>
        /// Executes stored procedure.
        /// </summary>
        /// <returns>Number of row affected</returns>
        public int ExecuteNonQuery()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(this.storedProc, connection))
                {
                    this.buildParameters(command);
                    command.CommandType = CommandType.StoredProcedure;
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected;
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Executes stored procedure.
        /// </summary>
        /// <returns>Out parameter value</returns>
        public string ExecuteNonQueryWithOutValue(string outParameter)
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(this.storedProc, connection))
                {
                    this.buildParameters(command);
                    command.CommandType = CommandType.StoredProcedure;
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return command.Parameters[outParameter].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }


        /// <summary>
        /// Executes stored procedure and returns the first column of the first row. 
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <returns>.NET object</returns>
        public object ExecuteScalar()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            globalConnection = connection;
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(this.storedProc, connection))
                {
                    this.buildParameters(command);
                    command.CommandType = CommandType.StoredProcedure;
                    object result = command.ExecuteScalar();
                    connection.Close();
                    return result;
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }


        /// <summary>
        /// Executes SQL Statement, respecting Parameters
        /// 
        /// Only used with Management Approval.
        /// </summary>
        /// <returns>Number of row affected</returns>
        public int ExecuteNonQuery_SQL()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(this.storedProc, connection))
                {
                    this.buildParameters(command);
                    command.CommandType = CommandType.Text;
                    // ---
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected;
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }


        /// <summary>
        /// Deprecated, saved for legacy
        /// </summary>
        /// <returns>.NET DataSet Class</returns>
        public DataSet GetData(string connectionString, SqlCommand cmd)
        {
            this.connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            SqlConnection connection = new SqlConnection(this.connectionString);
            cmd.Connection = connection;
            DataSet ds = new DataSet();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
            return ds;
        }


        /* Generic methods for Procedure execution */

        #region Parameter Methods

        public void AddInParameter(string parameterName, SqlDbType dbType, object value)
        {
            this.AddInParameter(parameterName, dbType, 0, value);
        }

        public void AddInParameter(string parameterName, SqlDbType dbType, int size, object value)
        {
            if (this.parameters == null)
            {
                this.parameters = new List<SqlParameter>();
            }

            SqlParameter p = new SqlParameter(parameterName, dbType, size);
            p.Value = value;

            parameters.Add(p);
        }

        public void AddOutParameter(string parameterName, SqlDbType dbType)
        {
            this.AddOutParameter(parameterName, dbType, 0);
        }

        public void AddOutParameter(string parameterName, SqlDbType dbType, int size)
        {
            if (this.parameters == null)
            {
                this.parameters = new List<SqlParameter>();
            }

            SqlParameter p = new SqlParameter(parameterName, dbType, size);
            p.Direction = ParameterDirection.Output;
            parameters.Add(p);
        }

        public void buildParameters(SqlCommand command)
        {
            if (parameters != null)
            {
                foreach (var p in this.parameters)
                {
                    command.Parameters.Add(p);
                }
            }
        }

        #endregion

        /* Generic methods for Procedure execution */


        /* Generic methods for Procedure execution */


        /* Generic methods for Procedure execution */
    }
}
