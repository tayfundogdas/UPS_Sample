using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS_Sample.restops.domain
{
    public class UserModel
    {
        public Meta meta { get; set; }
        public List<User> data { get; set; }
    }
}
