using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Repository;
using Services.Response;

namespace Services
{
    public class AuthenticateBookServices : IAuthenticateBookServices
    {
        private readonly BookRepository _context;
        private readonly IMapper _mapper;
        private readonly IPassword _password;
        private readonly IAuthRepository _authReposiotry;
        private readonly IJWTManagerServices _jWTManagerServices;

        public AuthenticateBookServices(BookRepository bookRepository, IMapper mapper,
            IPassword password, IAuthRepository authReposiotry, IJWTManagerServices jWTManagerServices)
        {
            _context = bookRepository;
            _mapper = mapper;
            _password = password;
            _authReposiotry = authReposiotry;
            _jWTManagerServices = jWTManagerServices;
        }

        
        /*public void AddAddressBook(Profiles profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _context.Profile.Add(profile);
        }*/

        /// <summary>
        /// Method to Save the changes 
        /// </summary>
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        /// <summary>
        /// Method to Generate token
        /// </summary>
        /// <param name="userData">Contains Username and password for generate token</param>
        public TokenResponse AuthUser(LoginDto userData)
        {
            var user = _authReposiotry.GetUser(userData.UserName);

            if (user == null)
                return new TokenResponse(false, "User not authenticated", null, null);

            if (_password.PasswordMatches(userData.Password, user.Password))
                return new TokenResponse(true, "", _jWTManagerServices.GenerateSecurityToken(user), "bearer");

            return new TokenResponse(false, "User not authenticated", null, null);
        }
    }
}