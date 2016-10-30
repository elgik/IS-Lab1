using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Domain;

namespace WpfApplication1.Controller
{
    class AdminWorkSpaceContoller
    {
        private Context db = new Context();

        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.Select(u => u).ToList();
        }
    }
}
