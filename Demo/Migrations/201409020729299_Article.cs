using FiletableDataContext.Migrations;

namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Article : CreateFiletable
    {
        protected override string ConnectionStringName
        {
            get { return "MyDb"; }
        }

        protected override string TableName
        {
            get { return "Article"; }
        }
    }
}
