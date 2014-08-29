using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstStoredProcs;
using FiletableDataContext.Domain.CreateDir;
using FiletableDataContext.Domain.CreateFile;
using FiletableDataContext.Domain.Rename;
using FiletableDataContext.Domain.Update;

namespace FiletableDataContext.Domain
{
    public abstract class FiletableDataContext<T>: DbContext where T: class, IFiletableEntity
    {
        private string TableName
        {
            get { return typeof (T).Name; }
        }

        public StoredProc<IFiletableEntity> CreateDir { get; set; }

        [StoredProcAttributes.ReturnTypes(typeof (CreateFileResult))]
        public StoredProc<IFiletableEntity> CreateFile { get; set; }

        public StoredProc<IFiletableEntity> Rename { get; set; }

        public StoredProc<IFiletableEntity> Update { get; set; }

        public StoredProc<IFiletableEntity> Delete { get; set; }

        public DbSet<T> Filetable { get; set; }

        public FiletableDataContext()
        {
            ((DbContext)this).InitializeStoredProcs();

            CreateDir.HasName(string.Format("{0}_CreateDir", TableName));
            CreateFile.HasName(string.Format("{0}_CreateFile", TableName));
            Rename.HasName(string.Format("{0}_Rename", TableName));
            Update.HasName(string.Format("{0}_Update", TableName));
            Delete.HasName(string.Format("{0}_Delete", TableName));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>().ToTable(string.Format("{0}_View", TableName));
        }
    }
}
