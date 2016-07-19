using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Domain
{
    public class GroceryList
    {
        public Guid GroceryListID { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid HouseholdID { get; set; }

        /* Navigation Properties */
        public virtual ICollection<GroceryItem> Items { get; set; }
        public virtual Household Household { get; set; }
    }
}
