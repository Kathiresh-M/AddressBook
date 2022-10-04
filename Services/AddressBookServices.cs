using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Services
{
    public class AddressBookServices : IAddressBookServices
    {
        private readonly BookRepository _context;
        private readonly IMapper _mapper;

        public AddressBookServices(BookRepository bookRepository, IMapper mapper)
        {
            _context = bookRepository;
            _mapper = mapper;
        }

        public ActionResult<RefSetDto> Add(RefSetDto refsetdto)
        {
            var result = _mapper.Map<RefSet>(refsetdto);
            var r = _mapper.Map<RefSetDto>(result);
            return r;
        }

        public ActionResult<RefTermDto> AddRefTerm(RefTermDto reftermdto)
        {
            var result = _mapper.Map<RefTerm>(reftermdto);
            var r = _mapper.Map<RefTermDto>(result);
            return r;
        }

        public ActionResult<RefSetTermDto> AddRefSetTerm(RefSetTermDto refsettermdto)
        {
            var result = _mapper.Map<RefSetTerm>(refsettermdto);
            var r = _mapper.Map<RefSetTermDto>(result);
            return r;
        }

        public ActionResult<List<ProfileforCreatingDto>> GetAddressBook()
        {
            var Address = _context.Profile.ToList();
            var result = _mapper.Map<List<ProfileforCreatingDto>>(Address);
            return result;
        }

        public ActionResult<ProfileforCreatingDto> GetById(Guid Id)
        {
            var address = _context.Profile.Find(Id);
            var result = _mapper.Map<ProfileforCreatingDto>(address);
            return result;
        }

        public Profiles GetAddress(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentNullException(nameof(Id));

            return _context.Profile.FirstOrDefault(b => b.Id == Id);
        }

        public void DeleteAddress(Profiles profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _context.Profile.Remove(profile);
        }

        public async Task File(ICollection<IFormFile> files)
        {
            foreach(var file in files)
            {
                if(file.Length > 0)
                {
                    using (var str = new MemoryStream())
                    {
                        file.CopyTo(str);
                        var fileBytes = str.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                    }
                }
            }
        }

        public int CountRecord()
        {
            return _context.Profile.Count();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
