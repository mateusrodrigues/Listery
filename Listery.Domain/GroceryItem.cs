using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Domain
{
    public class GroceryItem
    {
        public Guid GroceryItemID { get; set; }
        public string Content { get; set; }
        public bool IsDone { get; set; }
        public Guid GroceryListID { get; set; }

        /* Navigation Properties */
        public virtual GroceryList List { get; set; }
    }
}
