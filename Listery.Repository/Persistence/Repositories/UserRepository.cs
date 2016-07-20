using BCrypt.Net;
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

        public User FindByEmail(string email)
        {
            var claim = _context.UserClaims.FirstOrDefault(m => m.ClaimType.Equals("email")
                    && m.ClaimValue.Equals(email, StringComparison.CurrentCultureIgnoreCase));

            var user = (claim != null) ? _context.Users.Find(claim.Subject) : null;

            return user;
        }

        public User FindByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(m => m.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));

            return user;
        }

        public User Get(Guid id)
        {
            return _context.Users.Find(id);
        }

        public User GetWithClaims(Guid id)
        {
            return _context.Users.Include(m => m.Claims)
                .FirstOrDefault(m => m.Subject.Equals(id));
        }

        public User GetWithClaimsForIdentity(string username, string password)
        {
            var user = _context.Users.Include(m => m.Claims)
                .FirstOrDefault(m => m.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));

            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                    user = null;
            }

            return user;
        }

        public void Update(User entity)
        {
            _context.Entry<User>(entity).State = EntityState.Modified;
        }
    }
}
