using AutoMapper;
using Contracts;
using Entities.Models;
using Repository;

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
    }
}
