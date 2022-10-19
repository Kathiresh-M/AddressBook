using Contracts;
using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class AuthenticationRepository : IAuthRepository
    {
        private readonly BookRepository _context;

        public AuthenticationRepository(BookRepository context)
        {
            _context = context;
        }

        //Get user by username
        public Profiles GetUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName) + " was null in GetUser");

            return _context.Profile.SingleOrDefault(user => user.User_Name == userName);
        }

        //Get user by user id
        public Profiles GetUser(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId) + " was null in GetUser");

            return _context.Profile.SingleOrDefault(user => user.Id == userId);
        }
    }
}
