using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DB;

namespace Service.DBAccess
{
    public class DataService
    {
        public int UpdateGold(string user, string password, string connectionString)
        {
            return Access.GetUserGold(user, password, connectionString);
        }
    }
}
