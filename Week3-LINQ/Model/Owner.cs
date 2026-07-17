using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3_LINQ.Model
{
    public class Owner
    {
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }       
        public string Phone { get; set; }       
    }
}
