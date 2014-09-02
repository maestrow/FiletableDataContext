using System.Data.Entity;
using FiletableDataContext.Domain;

namespace Demo.Domain
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(): base("name=MyDb")
        {
        }
    }
}
