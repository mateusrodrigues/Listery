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
            var username = value as string;
            // Allow only alpha-numeric characters, hyphens, periods and underscores
            string pattern = @"^[0-9A-Za-z][-._]*[0-9A-Za-z]$";

            return Regex.IsMatch(username, pattern);
        }
    }
}
