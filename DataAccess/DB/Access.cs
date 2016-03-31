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

            if (!VerifyUser(password, user.Password))
                return -1;
            List<UVote> uVotes = new FileSpliter().GetUVote(user.Id, user.Login);
            if (uVotes.Count == 0)
                return 0;
            foreach (var uVote in uVotes)
            {
                if (uVote.Vote != 0)
                    gold += uVote.Vote;
            }
            user.Gold += gold;
            
            UpdateDb(uVotes,user,connectionString);

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
                    user.Login = reader.GetString(1).Split(' ')[0];
                    user.Password = reader.GetString(2).Split(' ')[0];
                    user.Gold = reader.GetInt32(3);
                }
                
            }
            catch (Exception e)
            {
                // ignored
            }
            command.Parameters.Clear();
            connection.Close();
            return user;
        }

        public static string Hesh(string inner)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(inner));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static bool VerifyUser(string innerPass, string hashedPass)
        {
            MD5 md5Hash = MD5.Create();
            string innerHesh = Hesh(innerPass);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(innerHesh, hashedPass))
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

        public static void UpdateDb(List<UVote> uVote, User user, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SetUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (var currentUVote in uVote)
            {
                command.Parameters.AddWithValue("@switcher", 1);
                command.Parameters.AddWithValue("@Id", currentUVote.Id);
                command.Parameters.AddWithValue("@Data", currentUVote.Data);
                command.Parameters.AddWithValue("@IP", currentUVote.IP);
                command.Parameters.AddWithValue("@Login", currentUVote.Login);
                command.Parameters.AddWithValue("@Vote", currentUVote.Vote);
                
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException exception)
                {

                }
                finally
                {
                    connection.Close();
                }
                command.Parameters.Clear();
            }

            command.Parameters.AddWithValue("@switcher", 2);
            command.Parameters.AddWithValue("@Id", uVote[uVote.Count - 1].Id);
            command.Parameters.AddWithValue("@Password", Hesh(user.Password));
            command.Parameters.AddWithValue("@Gold", user.Gold);
            
            
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException exception)
            {

            }
            finally
            {
                connection.Close();
            }
            command.Parameters.Clear();
        }
    }
}
