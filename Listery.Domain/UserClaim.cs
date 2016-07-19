using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Domain
{
    public class UserClaim
    {
        public Guid UserClaimID { get; set; }
        public Guid Subject { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        /* Navigation Property */
        public virtual User User { get; set; }
    }
}
