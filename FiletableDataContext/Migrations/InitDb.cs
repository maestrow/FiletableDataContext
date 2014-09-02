using System.IO;

namespace FiletableDataContext.Migrations
{
    public abstract class InitDb : DbMigrationBase
    {
        public abstract string RootPath { get; }

        public override void Up()
        {
            CreateDbDir_Up();
            FilestreamFilegroup_Up();
            //FileStreamProperties_Up();
            GetNewID_Up();
            GetNewPathLocator_Up();
        }

        public override void Down()
        {
            FilestreamFilegroup_Down();
            GetNewID_Down();
            GetNewPathLocator_Down();
        }

        private void CreateDbDir_Up()
        {
            var dirName = string.Format(@"{0}\{1}", RootPath, DbName);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
        }

        public void FilestreamFilegroup_Up()
        {
            Sql(string.Format(@"ALTER DATABASE {0} ADD FILEGROUP {0}Group CONTAINS FILESTREAM", DbName), true);
            Sql(string.Format(@"ALTER DATABASE {0}
                ADD FILE (
	                NAME = '{0}Filestream',
	                FILENAME = '{1}\{0}\Filestream'
                ) TO FILEGROUP {0}Group
                ", DbName, RootPath), true);
        }

        public void FilestreamFilegroup_Down()
        {
            Sql(string.Format(@"ALTER DATABASE {0} REMOVE FILE {0}Filestream", DbName), true);
            Sql(string.Format(@"ALTER DATABASE {0} REMOVE FILEGROUP {0}Group", DbName), true);
        }

        private void FileStreamProperties_Up()
        {
            Sql(string.Format(@"ALTER DATABASE {0} SET FILESTREAM ( NON_TRANSACTED_ACCESS = FULL, DIRECTORY_NAME = '{0}' )", DbName), true);
            Sql(string.Format(@"ALTER DATABASE {0} SET READ_COMMITTED_SNAPSHOT OFF", DbName), true);
        }

        private void GetNewID_Up()
        {
            Sql(@"CREATE VIEW dbo.GetNewID AS SELECT newid() AS new_id");
        }

        private void GetNewID_Down()
        {
            Sql(@"DROP VIEW dbo.GetNewID");
        }

        private void GetNewPathLocator_Up()
        {
            Sql(@"
            CREATE FUNCTION dbo.GetNewPathLocator (@parent hierarchyid = null) RETURNS varchar(max) AS
            BEGIN       
                DECLARE @result varchar(max), @newid uniqueidentifier  -- declare new path locator, newid placeholder       
                SELECT @newid = new_id FROM dbo.getNewID; -- retrieve new GUID      
                SELECT @result = ISNULL(@parent.ToString(), '/') + -- append parent if present, otherwise assume root
                                 convert(varchar(20), convert(bigint, substring(convert(binary(16), @newid), 1, 6))) + '.' +
                                 convert(varchar(20), convert(bigint, substring(convert(binary(16), @newid), 7, 6))) + '.' +
                                 convert(varchar(20), convert(bigint, substring(convert(binary(16), @newid), 13, 4))) + '/'     
                RETURN @result -- return new path locator     
            END
            ");
        }

        private void GetNewPathLocator_Down()
        {
            Sql(@"DROP FUNCTION dbo.GetNewPathLocator");
        }
    }
}
