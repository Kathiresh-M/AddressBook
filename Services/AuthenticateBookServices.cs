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

        public AuthenticateBookServices(BookRepository bookRepository, IMapper mapper)
        {
            _context = bookRepository;
            _mapper = mapper;
        }
        public void AddBand(Profiles profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _context.Profile.Add(profile);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
        public TokenResponse AuthUser(LoginDto userData)
        {

            if (userData == null)
                return new TokenResponse(false, "User not authenticated", null, null);

            if (_passwordHasher.PasswordMatches(userData.Password, userData.Hash))
                return new TokenResponse(true, "", IJWTManagerServices.GenerateSecurityToken(user), "bearer");

            return new TokenResponse(false, "User not authenticated", null, null);
        }
    }
}