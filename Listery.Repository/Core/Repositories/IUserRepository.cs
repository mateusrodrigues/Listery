using Listery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Repository.Core.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        User GetWithClaims(Guid id);
        User GetWithClaimsForIdentity(string username, string password);
    }
}
