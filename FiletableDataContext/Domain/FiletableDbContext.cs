using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstStoredProcs;
using FiletableDataContext.Domain.Params;

namespace FiletableDataContext.Domain
{
    public abstract class FiletableDbContext<T> : DbContext where T : FiletableEntityBase
    {
        private string TableName
        {
            get { return typeof (T).Name; }
        }

        [StoredProcAttributes.ReturnTypes(typeof(CreateResult))]
        public StoredProc<ICreateDirParams> CreateDir { get; set; }

        [StoredProcAttributes.ReturnTypes(typeof (CreateResult))]
        public StoredProc<ICreateFileParams> CreateFile { get; set; }

        public StoredProc<IRenameParams> Rename { get; set; }

        public StoredProc<IUpdateParams> Update { get; set; }

        public StoredProc<IStreamId> Delete { get; set; }

        public DbSet<T> Filetable { get; set; }

        public FiletableDbContext()
        {
            init();
        }

        public FiletableDbContext(string connectionStrName): base(connectionStrName)
        {
            init();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>().ToTable(string.Format("{0}_View", TableName));
        }

        private void init()
        {
            ((DbContext)this).InitializeStoredProcs();

            CreateDir.HasName(string.Format("{0}_CreateDir", TableName));
            CreateFile.HasName(string.Format("{0}_CreateFile", TableName));
            Rename.HasName(string.Format("{0}_Rename", TableName));
            Update.HasName(string.Format("{0}_Update", TableName));
            Delete.HasName(string.Format("{0}_Delete", TableName));
        }
    }
}
