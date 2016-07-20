using Listery.Repository.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Repository.Core
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        int Complete();
    }
}
