using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Models;

namespace DataAccess.DB
{
    public class Access
    {
        public static int GetUserGold(string login, string password, string connectionString)
        {
            var gold = 0;
            User user = GetUserFromDb(connectionString, login);

            if (!VerifyUser(password, user.Password.Split(' ')[0]))
                return -1;
            List<UVote> uVotes = new FileSpliter().GetUVote(user.Id, user.Login);
            foreach (var uVote in uVotes)
            {
                if (uVote.Vote != 0)
                    gold += uVote.Vote;
            }
            user.Gold += gold;

            return user.Gold;
        }

        public static User GetUserFromDb(string connectionString,string login)
        {
            User user = new User();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("GetUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@login", login);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = reader.GetInt32(0);
                    user.Login = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.Gold = reader.GetInt32(3);
                }
                
            }
            catch (Exception e)
            {
                // ignored
            }
            connection.Close();
            return user;
        }

        public static bool VerifyUser(string innerPass, string hashedPass)
        {
            
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(innerPass));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            //24c9e15e52afc47c225b757e7bee1f9d
            if (0 == comparer.Compare(sBuilder.ToString(), hashedPass))
            {
                md5Hash.Dispose();
                return true;
            }
            else
            {
                md5Hash.Dispose();
                return false;
            }
        }
    }
}
