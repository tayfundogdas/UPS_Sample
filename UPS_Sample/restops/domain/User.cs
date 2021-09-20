using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS_Sample.restops.domain
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string status { get; set; }

        public string ToCSVRow()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.id);
            sb.Append(',');
            sb.Append(this.name);
            sb.Append(',');
            sb.Append(this.email);
            sb.Append(',');
            sb.Append(this.gender);
            sb.Append(',');
            sb.Append(this.status);
            sb.Append(',');
            //Id,Name,E-mail,Gender,Status,
            return sb.ToString();
        }
    }
}
