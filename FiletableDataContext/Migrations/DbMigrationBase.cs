using System.Configuration;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace FiletableDataContext.Migrations
{
    public abstract class DbMigrationBase: DbMigration
    {
        protected abstract string ConnectionStringName { get; }
        protected string DbName
        {
            get
            {
                var connBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
                return connBuilder.InitialCatalog;
            }
        }
    }
}
