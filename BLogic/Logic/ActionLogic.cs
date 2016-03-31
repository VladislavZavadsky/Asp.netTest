using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DBAccess;

namespace BLogic.Logic
{
    public class ActionLogic
    {
        public int UserCheckOut(string user, string password)
        {
            var connectionString = "Data Source=5.39.94.64;Initial Catalog=vlad.z_Task1;Persist Security Info=True;User ID=vlad.z;Password=09Griffon; Min Pool Size=5";
            DataService connectToDb = new DataService();
            return connectToDb.UpdateGold(user, password, connectionString);
        }
    }
}
