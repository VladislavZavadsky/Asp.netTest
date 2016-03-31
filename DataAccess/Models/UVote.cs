using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class UVote
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string IP { get; set; }
        public string Login { get; set; }
        public int Vote { get; set; }
    }
}
