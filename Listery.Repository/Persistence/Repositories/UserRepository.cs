using Listery.Data;
using Listery.Repository.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listery.Domain;
using System.Data.Entity;

namespace Listery.Repository.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ListeryDbContext _context;

        public UserRepository(ListeryDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            _context.Users.Add(entity);
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
        }

        public IEnumerable<User> FindAll()
        {
            return _context.Users.ToList();
        }

        public User Get(Guid id)
        {
            return _context.Users.Find(id);
        }

        public void Update(User entity)
        {
            _context.Entry<User>(entity).State = EntityState.Modified;
        }
    }
}
