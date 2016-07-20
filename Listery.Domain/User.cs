using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Domain
{
    public class User
    {
        public Guid Subject { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid? HouseholdID { get; set; }

        /* Navigation Properties */
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual Household Household { get; set; }
    }
}
