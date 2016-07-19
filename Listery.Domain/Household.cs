using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Domain
{
    public class Household
    {
        public Guid HouseholdID { get; set; }
        public string Name { get; set; }

        /* Navigation Properties */
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<GroceryList> Lists { get; set; }
    }
}
