using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiletableDataContext.Domain;

namespace Demo.Domain
{
    public class ArticleContext: FiletableDbContext<Article>
    {
        public ArticleContext(): base("name=MyDb")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }
    }
}
