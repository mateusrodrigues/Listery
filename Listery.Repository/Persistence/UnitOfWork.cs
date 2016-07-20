using Listery.Data;
using Listery.Repository.Core;
using Listery.Repository.Core.Repositories;
using Listery.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Repository.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ListeryDbContext _context;

        public UnitOfWork()
        {
            _context = new ListeryDbContext();

            Users = new UserRepository(_context);
        }

        public IUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
