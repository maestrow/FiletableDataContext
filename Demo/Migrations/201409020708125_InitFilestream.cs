using FiletableDataContext.Migrations;

namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitFilestream : InitDb
    {
        protected override string ConnectionStringName
        {
            get { return "MyDb"; }
        }

        public override string RootPath
        {
            get { return @"z:\DBs"; }
        }
    }
}
