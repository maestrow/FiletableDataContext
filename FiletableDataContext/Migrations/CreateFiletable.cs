namespace FiletableDataContext.Migrations
{
    public abstract class CreateFiletable: DbMigrationBase
    {
        protected abstract string TableName { get; }

        public override void Up()
        {
            Filetable_Up();
            View_Up();
            CreateDir_Up();
            CreateFile_Up();
            Rename_Up();
            Delete_Up();
        }

        public override void Down()
        {
            Filetable_Down();
            View_Down();
            CreateDir_Down();
            CreateFile_Down();
            Rename_Down();
            Delete_Down();
        }


        private void Filetable_Up()
        {
            Sql(string.Format(
            @"CREATE TABLE [{0}] AS FileTable WITH (FILETABLE_STREAMID_UNIQUE_CONSTRAINT_NAME = {0}_streamid_constraint)", TableName));
        }

        private void Filetable_Down()
        {
            Sql(string.Format(
            @"DROP TABLE [{0}]", TableName));
        }

        private void View_Up()
        {
            Sql(string.Format(@"
            CREATE VIEW {0}_View AS            
            SELECT 
	             stream_id
	            ,file_stream
	            ,name
	            ,file_type
	            ,cached_file_size
	            ,creation_time
	            ,last_write_time
	            ,last_access_time
	            ,is_directory
	            ,is_offline
	            ,is_hidden
	            ,is_readonly
	            ,is_archive
	            ,is_system
	            ,is_temporary
                ,parent_path_locator.ToString() as [parentpath]
                ,path_locator.ToString() as [path]
            FROM 
	            [{0}]
            ", TableName));
        }

        private void View_Down()
        {
            Sql(string.Format(@"DROP VIEW [{0}_View]", TableName));
        }

        private void CreateDir_Up()
        {
            Sql(string.Format(@"
            CREATE PROCEDURE [{0}_CreateDir] (@name AS NVARCHAR(255), @parentpath nvarchar(4000))
            AS
            BEGIN
	            INSERT INTO [{0}] (name, path_locator, is_directory, is_archive) 
                OUTPUT INSERTED.stream_id, INSERTED.path_locator.ToString() as [path]
	            VALUES (@name, dbo.GetNewPathLocator(@parentpath), 1, 0)
            END
            ", TableName));
        }

        private void CreateDir_Down()
        {
            Sql(string.Format(@"DROP PROCEDURE [{0}_CreateDir]", TableName));
        }

        private void CreateFile_Up()
        {
            Sql(string.Format(@"
            CREATE PROCEDURE [{0}_CreateFile] (
	            @name nvarchar(255), 
	            @file_stream varbinary(max),
	            @parentpath nvarchar(4000),
	            @is_hidden bit = 0,
	            @is_readonly bit = 0,
	            @is_archive bit = 0,
	            @is_system bit = 0,
	            @is_temporary bit = 0
            )
            AS
            BEGIN
                INSERT INTO [{0}](
                   [name]
	              ,[file_stream]
                  ,[path_locator]
                  ,[is_hidden]
                  ,[is_readonly]
                  ,[is_archive]
                  ,[is_system]
                  ,[is_temporary]
	            ) 
	            OUTPUT INSERTED.stream_id, INSERTED.path_locator.ToString() as [path]
	            VALUES (@name, @file_stream, dbo.GetNewPathLocator(@parentpath), @is_hidden, @is_readonly, @is_archive, @is_system, @is_temporary)
            END
            ", TableName));
        }

        private void CreateFile_Down()
        {
            Sql(string.Format(@"DROP PROCEDURE [{0}_CreateFile]", TableName));
        }

        private void Rename_Up()
        {
            Sql(string.Format(@"
            CREATE PROCEDURE [{0}_Rename] (
	             @stream_id uniqueidentifier
	            ,@name nvarchar(255)
            ) AS
            BEGIN
	            UPDATE [{0}] SET name = @name WHERE stream_id = @stream_id
            END
            ", TableName));
        }

        private void Rename_Down()
        {
            Sql(string.Format(@"DROP PROCEDURE [{0}_Rename]", TableName));
        }

        private void Update_Up()
        {
            Sql(string.Format(@"
            CREATE PROCEDURE [{0}_Update] (
	             @stream_id uniqueidentifier
	            ,@file_stream varbinary(max)
            ) AS
            BEGIN
	            UPDATE [{0}] SET file_stream = @file_stream WHERE stream_id = @stream_id
            END
            ", TableName));
        }

        private void Update_Down()
        {
            Sql(string.Format(@"DROP PROCEDURE [{0}_Update]", TableName));
        }

        private void Delete_Up()
        {
            Sql(string.Format(@"
            CREATE PROCEDURE [{0}_Delete] (
	             @stream_id uniqueidentifier
            ) AS
            BEGIN
	            DELETE FROM [{0}] WHERE stream_id = @stream_id
            END
            ", TableName));
        }

        private void Delete_Down()
        {
            Sql(string.Format(@"DROP PROCEDURE [{0}_Delete]", TableName));
        }
    }
}
