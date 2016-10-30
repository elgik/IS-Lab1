using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WpfApplication1.Domain
{
    public class Context : DbContext
    {
        public Context() : base("InformationSecurity") { }

        public DbSet<User> Users { get; set; }
    }
}
