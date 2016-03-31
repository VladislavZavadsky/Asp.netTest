using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Data
{
    class FileSpliter
    {
        public List<UVote> GetUVote(int id, string login)
        {
            List<UVote> uVotes = new List<UVote>();

            var row = GetText().Split('\n');
            foreach (var currentRow in row)
            {
                if (currentRow == "")
                    break;
                UVote uVote = new UVote();
                var element = currentRow.Split('\t');
                uVote.Id = Convert.ToInt32(element[0]);
                uVote.Data = element[1];
                uVote.Ip = element[2];
                uVote.Login = element[3].Split(' ')[0];
                uVote.Vote = Convert.ToInt32(4);
                if (login.Equals(uVote.Login) && id < uVote.Id)
                    uVotes.Add(uVote);
            }
            return uVotes;
        }

        public string GetText()
        {
            WebClient client = new WebClient();
            string URL =
                "https://mmotop.ru/votes/3d1c5d44db6ab17f74f1b90c83d88ec7fa0ab273.txt?603819ac288008a327adb6e088b6c663";
            string text = client.DownloadString(URL);
            client.Dispose();
            return text;
        }
    }
}
