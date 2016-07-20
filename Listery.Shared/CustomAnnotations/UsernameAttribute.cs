using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Listery.Shared.CustomAnnotations
{
    public class UsernameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // TODO Find way to validate username regular expression
            return true;
        }
    }
}
