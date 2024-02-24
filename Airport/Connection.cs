using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    internal class Connection
    {
        public SqlConnection generateConnection()
        {
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"PC1\SQLEXPRESS",
                InitialCatalog = "Airport",
                IntegratedSecurity = true
            };
            SqlConnection connection = new SqlConnection(connBuilder.ToString());
            return connection;
        }
    }
}
