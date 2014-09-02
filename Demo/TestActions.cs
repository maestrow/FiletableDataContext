using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstStoredProcs;
using Demo.Domain;
using FiletableDataContext.Domain;

namespace Demo
{
    public class TestActions
    {
        private ArticleContext db;
        
        public TestActions()
        {
            db = new ArticleContext();
        }
        
        public string CreateDir(string dirName, string parentPath = null)
        {
            var dir = new Article() { Name = dirName, ParentPath = parentPath };
            CreateResult result = db.CreateDir.CallStoredProc(dir).ToList<CreateResult>().FirstOrDefault();
            return result.Path;
        }

        public CreateResult CreateFile(string fileName, string parentPath)
        {
            var now = DateTimeOffset.Now;
            var file = new Article()
            {
                Name = fileName,
                File_stream = Encoding.UTF8.GetBytes(string.Format("file content for file: {0}", fileName)),
                ParentPath = parentPath,
                Creation_time = now,
                Last_access_time = now,
                Last_write_time = now
            };
            CreateResult result = db.CreateFile.CallStoredProc(file).ToList<CreateResult>().FirstOrDefault();
            return result;
        }
    }
}
