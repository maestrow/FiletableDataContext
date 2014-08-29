namespace FiletableDataContext.Migrations
{
    public abstract class InitDb : DbMigrationBase
    {
        public override void Up()
        {
            FileStreamProperties_Up();
            GetNewID_Up();
            GetNewPathLocator_Up();
        }

        public override void Down()
        {
            FileStreamProperties_Down();
            GetNewID_Down();
            GetNewPathLocator_Down();
        }

        private void FileStreamProperties_Up()
        {
            Sql(string.Format(@"
            ALTER DATABASE {0} SET 
                FILESTREAM ( NON_TRANSACTED_ACCESS = FULL, DIRECTORY_NAME = '{0}' ),
                READ_COMMITTED_SNAPSHOT OFF
            ", DbName), true);
        }

        private void FileStreamProperties_Down()
        {
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
