namespace Services.Helper
{
    public class AuthenticationRepository
    {
        public void CreateUser(Profile user)
        {
            if (user == null)
                throw new ArgumentNullException("User data was null in CreateUser from repository");

            _context.Users.Add(user);
        }

        //Get user by username
        public User GetUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName) + " was null in GetUser from repository");

            return _context.Users.SingleOrDefault(user => user.UserName == userName);
        }

        //Get user by user id
        public User GetUser(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId) + " was null in GetUser from repository");

            return _context.Users.SingleOrDefault(user => user.Id == userId);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User data was null in Deleteuser from repository");

            _context.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User data was null in Deleteuser from repository");

            _context.Users.Remove(user);
        }
    }
}
