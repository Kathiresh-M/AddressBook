using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Services.Response;

namespace Services
{
    public class AddressBookServices : IAddressBookServices
    {
        private readonly BookRepository _context;
        private readonly IMapper _mapper;
        private readonly IAddressBookRepository _addressBookRepository;
        private readonly IRefSetRepo _refSetRepo;
        private readonly IMetaDataMappingRepo _metadataMappingRepo;
        private readonly IAddressRepository _addressRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IEmailRepository _emailRepository;

        public AddressBookServices(BookRepository bookRepository, IMapper mapper,
            IAddressBookRepository addressBookRepository, IRefSetRepo refSetRepo,
            IMetaDataMappingRepo metadataMappingRepo, IAddressRepository addressRepository,
            IPhoneRepository phoneRepository, IEmailRepository emailRepository)
        {
            _context = bookRepository;
            _mapper = mapper;
            _addressBookRepository = addressBookRepository;
            _refSetRepo = refSetRepo;
            _metadataMappingRepo = metadataMappingRepo;
            _addressRepository = addressRepository;
            _phoneRepository = phoneRepository;
            _emailRepository = emailRepository;
        }

        public RefSetDto Add(RefSetDto refsetdto)
        {
            var result = _mapper.Map<RefSet>(refsetdto);
            var r = _mapper.Map<RefSetDto>(result);
            _context.RefSet.Add(result);
            _context.SaveChanges();
            return r;
        }

        public RefTermDto AddRefTerm(RefTermDto reftermdto)
        {
            var result = _mapper.Map<RefTerm>(reftermdto);
            var r = _mapper.Map<RefTermDto>(result);
            _context.RefTerm.Add(result);
            _context.SaveChanges();
            return r;
        }

        public RefSetTermDto AddRefSetTerm(RefSetTermDto refsettermdto)
        {
            var result = _mapper.Map<RefSetTerm>(refsettermdto);
            var r = _mapper.Map<RefSetTermDto>(result);
            _context.RefSetTerm.Add(result);
            _context.SaveChanges();
            return r;
        }

        public ActionResult<List<ProfileforCreatingDto>> GetAddressBooks()
        {
            var Address = _context.Profile.ToList();
            var result = _mapper.Map<List<ProfileforCreatingDto>>(Address);
            return result;
        }

        public ProfileforCreatingDto GetAddressBookById(Guid Id)
        {
            var address = _context.Profile.Find(Id);
            var result = _mapper.Map<ProfileforCreatingDto>(address);
            return result;
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

        public AddressBookResponsetoUser UpdateAddressBook(ProfileforCreatingDto addressBookData, Guid addressBookId, Guid userId)
        {
            var addressBookCheck = _addressBookRepository.GetAddressBookById(addressBookId);

            if (addressBookCheck == null && addressBookCheck.Id == userId)
                return new AddressBookResponsetoUser(false, "Address book not found", null);

            var emailsUpdated = UpdateEmails(addressBookData.Email, addressBookId);
            if (!emailsUpdated.IsSuccess)
                return new AddressBookResponsetoUser(false, emailsUpdated.Message, null);

            var phonesUpdated = UpdatePhones(addressBookData.Phone, addressBookId);
            if (!phonesUpdated.IsSuccess)
                return new AddressBookResponsetoUser(false, phonesUpdated.Message, null);

            var addressUpdated = UpdateAddresses(addressBookData.Address, addressBookId);
            if (!addressUpdated.IsSuccess)
                return new AddressBookResponsetoUser(false, addressUpdated.Message, null);

            addressBookCheck.First_Name = addressBookData.First_Name;
            addressBookCheck.Last_Name = addressBookData.Last_Name;

            _addressBookRepository.UpdateAddressBook(addressBookCheck, emailsUpdated.Email, addressUpdated.Address, phonesUpdated.Phone);

            _addressBookRepository.Save();

            return new AddressBookResponsetoUser(true, "", null);
        }

        public int CountAddressBook()
        {
            return _context.Profile.Count();
        }

        public Profiles GetAddress(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentNullException(nameof(Id));

            return _context.Profile.FirstOrDefault(b => b.Id == Id);
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        private EmailResponse UpdateEmails(IEnumerable<EmailUpdationDTO> emails, Guid addressbookId)
        {
            var emailSet = _refSetRepo.GetRefSet("Email_Type");
            var emailTerms = _metadataMappingRepo.GetRefTermsByRefSetId(emailSet.Id);
            var emailRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(emailSet.Id);

            IList<Email> emailsList = new List<Email>();
            var emailsInDB = IEmailRepository.GetEmailsByAddressBookId(addressbookId);


            for (int i = 0; i < emails.Count(); i++)
            {
                var refTerm = emailTerms.SingleOrDefault(refTerm => refTerm.Key.ToLower() == emails.ElementAt(i).Type.Key.ToLower());
                if (refTerm == null)
                    return new EmailResponse(false, $"Key {emails.ElementAt(i).Type.Key} was not found.", null);
                var mapping = emailRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTermId == refTerm.Id);
                if (mapping == null)
                    return new EmailResponse(false, $"Key {emails.ElementAt(i).Type.Key} was not found.", null);

                emailsList.Add(new Email
                {
                    Id = emails.ElementAt(i).Id,
                    User_Email = emails.ElementAt(i).User_Email,
                    Type = mapping.Id
                });
            }

            foreach (var email in emailsList)
            {
                int index = emailsInDB.ToList().FindIndex(emailFromDB => emailFromDB.Id == email.Id);
                var emailFromDB = emailsInDB.ToList().ElementAt(index);
                emailFromDB.EmailAddress = email.User_Email;
                emailFromDB.EmailTypeId = email.Type;
            }

            return new EmailResponse(true, "", emailsInDB);
        }

        private PhoneResponse UpdatePhones(IEnumerable<PhoneUpdationDTO> phones, Guid addressbookId)
        {
            var phoneSet = _refSetRepo.GetRefSet("PHONE_TYPE");
            var phoneTerms = _metadataMappingRepo.GetRefTermsByRefSetId(phoneSet.Id);
            var phoneRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(phoneSet.Id);

            IList<Phone> phonesList = new List<Phone>();
            var phonesInDB = _phoneRepository.GetPhonesByAddressBookId(addressbookId);

            if (phonesInDB.Count() != phones.Count())
                return new PhoneResponse(false, "Additional phone number data given", null);

            foreach (var phoneFromDB in phonesInDB)
            {
                var DoesExists = phones.Where(phone => phone.Id == phoneFromDB.Id);
                if (DoesExists == null)
                    return new PhoneResponse(false, "Given phone number data not found in the address book", null);
            }

            //phone validation
            bool unique = true;
            for (int i = 0; i < phones.Count() - 1; i++)
            {
                for (int j = i + 1; j < phones.Count(); j++)
                {
                    if (phones.ElementAt(i).Phone_Number.Length == 13)
                    {
                        unique = false;
                        break;
                    }
                }
            }

            for (int i = 0; i < phones.Count(); i++)
            {
                var refTerm = phoneTerms.SingleOrDefault(refTerm => refTerm.Key.ToLower() == phones.ElementAt(i).Type.Key.ToLower());
                if (refTerm == null)
                    return new PhoneResponse(false, $"Key {phones.ElementAt(i).Type.Key} was not found.", null);
                var mapping = phoneRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTermId == refTerm.Id);
                if (mapping == null)
                    return new PhoneResponse(false, $"Key {phones.ElementAt(i).Type.Key} was not found.", null);

                phonesList.Add(new Phone
                {
                    Id = phones.ElementAt(i).Id,
                    Phone_Number = phones.ElementAt(i).Phone_Number,
                    Phone_Type = mapping.Id,
                });
            }

            foreach (var phone in phonesList)
            {
                int index = phonesInDB.ToList().FindIndex(phoneFromDB => phoneFromDB.Id == phone.Id);
                var phoneFromDB = phonesInDB.ToList().ElementAt(index);
                phoneFromDB.PhoneNumber = phone.Phone_Number;
                phoneFromDB.PhoneTypeId = phone.Phone_Type;
            }

            return new PhoneResponse(true, null, phonesInDB);
        }

        private AddressResponse UpdateAddresses(IEnumerable<AddressUpdationDTO> addresses, Guid addressBookId)
        {
            var addressSet = _refSetRepo.GetRefSet("ADDRESS_TYPE");
            var addressTerms = _metadataMappingRepo.GetRefTermsByRefSetId(addressSet.Id);
            var addressRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(addressSet.Id);

            var countrySet = _refSetRepo.GetRefSet("COUNTRY_TYPE");
            var countryTerms = _metadataMappingRepo.GetRefTermsByRefSetId(countrySet.Id);
            var countryRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(countrySet.Id);

            IList<Address> addressesList = new List<Address>();
            var addressesInDB = IAddressRepository.GetAddresssByAddressBookId(addressBookId);

            if (addressesInDB.Count() != addresses.Count())
                return new AddressResponse(false, "Additional address data given", null);

            foreach (var addressFromDB in addressesInDB)
            {
                var DoesExists = addresses.Where(address => address.Id == addressFromDB.Id);
                if (DoesExists == null)
                    return new AddressResponse(false, "Given address data not found in the address book", null);
            }

            for (int i = 0; i < addresses.Count(); i++)
            {
                var address = addresses.ElementAt(i);

                var addressRefTerm = addressTerms.SingleOrDefault(refTerm => refTerm.Key.ToLower() == address.Type.Key.ToLower());
                if (addressRefTerm == null)
                    return new AddressResponse(false, $"Key {address.Type.Key} was not found.", null);
                var addressMapping = addressRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTermId == addressRefTerm.Id);
                if (addressMapping == null)
                    return new AddressResponse(false, $"Key {address.Type.Key} was not found.", null);

                var countryRefTerm = countryTerms.SingleOrDefault(refTerm => refTerm.Key.ToLower() == address.Country.Key.ToLower());
                if (countryRefTerm == null)
                    return new AddressResponse(false, $"Key {address.Country.Key} was not found.", null);
                var countryMapping = countryRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTermId == countryRefTerm.Id);
                if (countryMapping == null)
                    return new AddressResponse(false, $"Key {address.Country.Key} was not found.", null);

                addressesList.Add(new Address
                {
                    Id = address.Id,
                    Line1 = address.Line1,
                    Line2 = address.Line2,
                    City = address.City,
                    State_Name = address.State_Name,
                    ZipCode = address.ZipCode,
                    type = addressMapping.Id,
                    Country = countryMapping.Id,
                });
            }

            foreach (var address in addressesList)
            {
                int index = addressesInDB.ToList().FindIndex(addressFromDB => addressFromDB.Id == address.Id);
                var addressFromDB = addressesInDB.ToList().ElementAt(index);
                addressFromDB.Line1 = address.Line1;
                addressFromDB.Line2 = address.Line2;
                addressFromDB.City = address.City;
                addressFromDB.StateName = address.State_Name;
                addressFromDB.ZipCode = address.ZipCode;
                addressFromDB.AddressTypeId = address.type;
                addressFromDB.CountryTypeId = address.Country;
            }

            return new AddressResponse(true, null, addressesInDB);
        }
    }
}
