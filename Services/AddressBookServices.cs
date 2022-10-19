using AutoMapper;
using Contracts;
using Contracts.Response;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Services.Helper;
using Services.Response;
using System.Text.RegularExpressions;

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
        private readonly IRefTermRepo _refTermRepo;

        public AddressBookServices(BookRepository bookRepository, IMapper mapper,
            IAddressBookRepository addressBookRepository, IRefSetRepo refSetRepo,
            IMetaDataMappingRepo metadataMappingRepo, IAddressRepository addressRepository,
            IPhoneRepository phoneRepository, IEmailRepository emailRepository,
            IRefTermRepo refTermRepo)
        {
            _context = bookRepository;
            _mapper = mapper;
            _addressBookRepository = addressBookRepository;
            _refSetRepo = refSetRepo;
            _metadataMappingRepo = metadataMappingRepo;
            _addressRepository = addressRepository;
            _phoneRepository = phoneRepository;
            _emailRepository = emailRepository;
            _refTermRepo = refTermRepo;
        }

        /// <summary>
        /// Method to Create RefSet Details and store into RefSet database 
        /// </summary>
        /// <param name="refsetdto">RefSet Data to be created</param>
        public RefSetDto Add(RefSetDto refsetdto)
        {
            var result = _mapper.Map<RefSet>(refsetdto);
            var r = _mapper.Map<RefSetDto>(result);
            _context.RefSet.Add(result);
            _context.SaveChanges();
            return r;
        }

        /// <summary>
        /// Method to Create RefTerm Details and store into RefTerm Database 
        /// </summary>
        /// <param name="reftermdto">RefTerm Data to be created</param>
        public RefTermDto AddRefTerm(RefTermDto reftermdto)
        {
            var result = _mapper.Map<RefTerm>(reftermdto);
            var r = _mapper.Map<RefTermDto>(result);
            _context.RefTerm.Add(result);
            _context.SaveChanges();
            return r;
        }

        /// <summary>
        /// Method to Create a RefSetTerm Details and store into RefSetTerm database
        /// </summary>
        /// <param name="refsettermdto">RefSetTerm Data to be created</param>
        public RefSetTermDto AddRefSetTerm(RefSetTermDto refsettermdto)
        {
            var result = _mapper.Map<RefSetTerm>(refsettermdto);
            var r = _mapper.Map<RefSetTermDto>(result);
            _context.RefSetTerm.Add(result);
            _context.SaveChanges();
            return r;
        }

        /// <summary>
        /// Method to Get List of All Address Book Details  
        /// </summary>
        /// <returns>List of Address Book Details</returns>
        public ActionResult<List<ProfileforCreatingDto>> GetAddressBooks()
        {
            var Address = _context.Profile.ToList();
            var result = _mapper.Map<List<ProfileforCreatingDto>>(Address);
            return result;
        }

        public AddressBookResponsetoUser CreateAddressBook(ProfileforCreatingDto addressBookData, 
            Guid userId)
        {
            var addressBookCheck = _addressBookRepository.GetAddressBookByName(addressBookData.First_Name, addressBookData.Last_Name);

            if (addressBookCheck != null && addressBookCheck.Id == userId)
                return new AddressBookResponsetoUser(false, "Address book already exists with same first and last name.", null);

            var emailsAdded = addEmail(addressBookData.Email);
            if (!emailsAdded.IsSuccess)
                return new AddressBookResponsetoUser(false, emailsAdded.Message, null);

            var phonesAdded = addPhone(addressBookData.Phone);
            if (!phonesAdded.IsSuccess)
                return new AddressBookResponsetoUser(false, phonesAdded.Message, null);

            var addressAdded = addAddress(addressBookData.Address);
            if (!addressAdded.IsSuccess)
                return new AddressBookResponsetoUser(false, addressAdded.Message, null);

            var addressBook = new AddressBookDto()
            {
                Id = Guid.NewGuid(),
                FirstName = addressBookData.First_Name,
                LastName = addressBookData.Last_Name,
                Emails = emailsAdded.Email,
                Phones = phonesAdded.Phone,
                Addresses = addressAdded.Address,
                UserId = userId,
            };

            foreach (var email in emailsAdded.Email)
            {
                email.Id = userId;
                email.Id = addressBook.Id;
            }

            foreach (var phone in phonesAdded.Phone)
            {
                phone.Id = addressBook.Id;
                phone.Id = userId;
            }

            foreach (var address in addressAdded.Address)
            {
                address.Id = addressBook.Id;
                address.Id = userId;
            }

            _addressBookRepository.CreateAddressBook(addressBook);

            _addressBookRepository.Save();

            return new AddressBookResponsetoUser(true, "", addressBook);
        }

        /// <summary>
        /// Method to Get an Perticular Address Book Details by using Id 
        /// </summary>
        /// <param name="Id">Address Book Id</param>
        /// <returns>an address book</returns>
        public ProfileforCreatingDto GetAddressBookById(Guid Id)
        {
            var address = _context.Profile.Find(Id);
            var result = _mapper.Map<ProfileforCreatingDto>(address);
            return result;
        }

        /// <summary>
        /// Method to Get Address Boook Details 
        /// </summary>
        /// <param name="addressBookId">Address Book Id</param>
        /// <param name="tokenUserId">tokenuser Id</param>
        public AddressBookResponsetoUser GetAddressBook(Guid addressBookId, Guid tokenUserId)
        {

            var addressBook = _addressBookRepository.GetAddressBookById(addressBookId);

            if (addressBook == null)
            {
                return new AddressBookResponsetoUser(false, "Address book not found", null);
            }

            if (addressBook.Id != tokenUserId)
            {
                return new AddressBookResponsetoUser(false, "User has no access to address book", null);
            }

            var emailsListToReturn = getEmails(addressBookId);

            var phonesListToReturn = getPhones(addressBookId);

            var addressListToReturn = getAddresses(addressBookId);

            var addressBookToReturn = new AddressBookToReturnDto()
            {
                Id = addressBook.Id,
                FirstName = addressBook.First_Name,
                LastName = addressBook.Last_Name,
                Emails = emailsListToReturn,
                Phones = phonesListToReturn,
                Addresses = addressListToReturn
            };

            return new AddressBookResponsetoUser(true, "", addressBookToReturn);
        }

        /// <summary>
        /// Method to Delete Address Book 
        /// </summary>
        /// <param name="addressBookId">Address Book Id</param>
        /// <param name="userId">user Id</param>
        public MessageResponsetoUser DeleteAddressBook(Guid addressBookId, Guid userId)
        {
            var addressBook = _addressBookRepository.GetAddressBookById(addressBookId);

            if (addressBook == null)
                return new MessageResponsetoUser(false, "Address book not found");

            if (addressBook.Id != userId)
                return new MessageResponsetoUser(false, "User not having access");

            _addressBookRepository.DeleteAddressBook(addressBook);
            _addressBookRepository.Save();

            return new MessageResponsetoUser(true, null);
        }

        /// <summary>
        /// Method to Convert file to Base64
        /// </summary>
        /// <param name="File">File contains data</param>
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

        private EmailResponse addEmail(ICollection<EmailDto> emails)
        {
            var emailSet = _refSetRepo.GetRefSet("EMAIL_TYPE");
            var emailTerms = _metadataMappingRepo.GetRefTermsByRefSetId(emailSet.Id);
            var emailRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(emailSet.Id);

            IList<Email> emailsList = new List<Email>();

            //email validation
            bool unique = true;
            for (int i = 0; i < emails.Count() - 1; i++)
            {
                if (!Regex.Match(emails.ElementAt(i).User_Email, @"/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/").Success)
                {
                    unique = false;
                    break;
                }

                for (int j = i + 1; j < emails.Count(); j++)
                {
                    if (emails.ElementAt(i).User_Email.Equals(emails.ElementAt(i).User_Email))
                    {
                        unique = false;
                        break;
                    }
                }
            }

            if (!unique)
                return new EmailResponse(false, "Email data is not valid", null);

            for (int i = 0; i < emails.Count(); i++)
            {
                var refTerm = emailTerms.SingleOrDefault(refTerm =>refTerm.RefTerm_Key.ToLower() == emails.ElementAt(i).type.Key.ToLower());
                if (refTerm == null)
                    return new EmailResponse(false, $"Key {emails.ElementAt(i).type.Key} was not found.", null);
                var mapping = emailRefTermsWithMapping.SingleOrDefault(mapping =>mapping.RefTerm_Id == refTerm.Id);
                if (mapping == null)
                    return new EmailResponse(false, $"Key {emails.ElementAt(i).type.Key} was not found.", null);

                emailsList.Add(new Email
                {
                    Id = Guid.NewGuid(),
                    User_Email = emails.ElementAt(i).User_Email,
                    EmailTypeId = mapping.Id
                });
            }

            return new EmailResponse(true, null, emailsList);
        }

        private PhoneResponse addPhone(ICollection<PhoneDto> phones)
        {
            var phoneSet = _refSetRepo.GetRefSet("PHONE_TYPE");
            var phoneTerms = _metadataMappingRepo.GetRefTermsByRefSetId(phoneSet.Id);
            var phoneRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(phoneSet.Id);

            IList<Phone> phonesList = new List<Phone>();

            //phone validation
            bool unique = true;
            for (int i = 0; i < phones.Count() - 1; i++)
            {
                if (phones.ElementAt(i).Phone_Number.Length == 14)
                {
                    unique = false;
                    break;
                }

                for (int j = i + 1; j < phones.Count(); j++)
                {
                    if (phones.ElementAt(i).Phone_Number.Equals(phones.ElementAt(i).Phone_Number))
                    {
                        unique = false;
                        break;
                    }
                }
            }

            if (!unique)
                return new PhoneResponse(false, "Phone data is not valid", null);

            for (int i = 0; i < phones.Count(); i++)
            {
                var refTerm = phoneTerms.SingleOrDefault(refTerm => 
                refTerm.RefTerm_Key.ToLower() == phones.ElementAt(i).Phone_type.Key.ToLower());
                if (refTerm == null)
                    return new PhoneResponse(false, $"Key {phones.ElementAt(i).Phone_type.Key} was not found.", null);
                var mapping = phoneRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTerm_Id == refTerm.Id);
                if (mapping == null)
                    return new PhoneResponse(false, $"Key {phones.ElementAt(i).Phone_type.Key} was not found.", null);

                phonesList.Add(new Phone
                {
                    Id = Guid.NewGuid(),
                    Phone_Number = phones.ElementAt(i).Phone_Number,
                    PhoneTypeId = mapping.Id,
                });
            }

            return new PhoneResponse(true, null, phonesList);
        }

        private AddressResponse addAddress(ICollection<AddressDto> addresses)
        {
            var addressSet = _refSetRepo.GetRefSet("ADDRESS_TYPE");
            var addressTerms = _metadataMappingRepo.GetRefTermsByRefSetId(addressSet.Id);
            var addressRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(addressSet.Id);

            var countrySet = _refSetRepo.GetRefSet("COUNTRY_TYPE");
            var countryTerms = _metadataMappingRepo.GetRefTermsByRefSetId(countrySet.Id);
            var countryRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(countrySet.Id);

            IList<Address> addressesList = new List<Address>();

            for (int i = 0; i < addresses.Count(); i++)
            {
                var address = addresses.ElementAt(i);

                var addressRefTerm = addressTerms.SingleOrDefault(refTerm => refTerm.RefTerm_Key.ToLower() == address.type.Key.ToLower());
                if (addressRefTerm == null)
                    return new AddressResponse(false, $"Key {address.type.Key} was not found.", null);
                var addressMapping = addressRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTerm_Id == addressRefTerm.Id);
                if (addressMapping == null)
                    return new AddressResponse(false, $"Key {address.type.Key} was not found.", null);

                var countryRefTerm = countryTerms.SingleOrDefault(refTerm => refTerm.RefTerm_Key.ToLower() == address.Country.Key.ToLower());
                if (countryRefTerm == null)
                    return new AddressResponse(false, $"Key {address.Country.Key} was not found.", null);
                var countryMapping = countryRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTerm_Id == countryRefTerm.Id);
                if (countryMapping == null)
                    return new AddressResponse(false, $"Key {address.Country.Key} was not found.", null);

                addressesList.Add(new Address
                {
                    Id = Guid.NewGuid(),
                    Line1 = address.Line1,
                    Line2 = address.Line2,
                    City = address.City,
                    State_Name = address.State_Name,
                    ZipCode = address.ZipCode,
                    AddressTypeId = addressMapping.Id,
                    CountryTypeId = countryMapping.Id,
                });
            }

            return new AddressResponse(true, null, addressesList);
        }

        /// <summary>
        /// Method to Update Address Book
        /// </summary>
        /// <param name="addressBookData">address book data to be updated</param>
        /// <param name="addressBookId">Id of the address book in Database</param>
        public AddressBookResponsetoUser UpdateAddressBook(AddressBookUpdate addressBookData, 
            Guid addressBookId, Guid userId)
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

        /// <summary>
        /// Method to Count od Address Book 
        /// </summary>
        public int CountAddressBook()
        {
            return _context.Profile.Count();
        }

        /// <summary>
        /// Method to Get perticular Address Book Id 
        /// </summary>
        /// <param name="Id">Id of Address Book</param>
        public Profiles GetAddress(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentNullException(nameof(Id));

            return _context.Profile.FirstOrDefault(b => b.Id == Id);
        }

        /// <summary>
        /// Method to Save the changes 
        /// </summary>
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        private IList<EmailToReturnDto> getEmails(Guid addressBookId)
        {
            var emailsList = _emailRepository.GetEmailsByAddressBookId(addressBookId);
            IList<EmailToReturnDto> emailsListToReturn = new List<EmailToReturnDto>();
            foreach (var email in emailsList)
            {
                var refSetMapping = _metadataMappingRepo.GetRefSetMapping(email.EmailTypeId);
                var refTerm = _refTermRepo.GetRefTerm(refSetMapping.RefTerm_Id);
                emailsListToReturn.Add(new EmailToReturnDto()
                {
                    Id = email.Id,
                    User_Email = email.User_Email,
                    type = new TypeReference() { Key = refTerm.RefTerm_Key }
                });
            }

            return emailsListToReturn;
        }

        private IList<PhoneToReturnDTO> getPhones(Guid addressBookId)
        {
            var phonesList = _phoneRepository.GetPhonesByAddressBookId(addressBookId);
            IList<PhoneToReturnDTO> phonesListToReturn = new List<PhoneToReturnDTO>();
            foreach (var phone in phonesList)
            {
                var refSetMapping = _metadataMappingRepo.GetRefSetMapping(phone.PhoneTypeId);
                var refTerm = _refTermRepo.GetRefTerm(refSetMapping.RefTerm_Id);
                phonesListToReturn.Add(new PhoneToReturnDTO()
                {
                    Id = phone.Id,
                    Phone_Number = phone.Phone_Number,
                    Phone_type = new TypeReference() { Key = refTerm.RefTerm_Key }
                });
            }

            return phonesListToReturn;
        }

        private IList<AddressToReturnDTO> getAddresses(Guid addressBookId)
        {
            var addressList = _addressRepository.GetAddresssByAddressBookId(addressBookId);
            IList<AddressToReturnDTO> addressListToReturn = new List<AddressToReturnDTO>();
            foreach (var address in addressList)
            {
                var refSetMapping = _metadataMappingRepo.GetRefSetMapping(address.AddressTypeId);
                var typeRefTerm = _refTermRepo.GetRefTerm(refSetMapping.RefTerm_Id);
                refSetMapping = _metadataMappingRepo.GetRefSetMapping(address.CountryTypeId);
                var countryRefTerm = _refTermRepo.GetRefTerm(refSetMapping.RefTerm_Id);
                addressListToReturn.Add(new AddressToReturnDTO()
                {
                    Id = address.Id,
                    Line1 = address.Line1,
                    Line2 = address.Line2,
                    City = address.City,
                    State_Name = address.State_Name,
                    ZipCode = address.ZipCode,
                    type = new TypeReference() { Key = typeRefTerm.RefTerm_Key },
                    Country = new TypeReference() { Key = countryRefTerm.RefTerm_Key },
                });
            }

            return addressListToReturn;
        }

        /// <summary>
        /// Method to Update the email and return to update Address book method
        /// </summary>
        /// <param name="emails">Email Updated data from user</param>
        /// <param name="addressbookId">Id of Address Book</param>
        private EmailResponse UpdateEmails(ICollection<EmailUpdationDto> emails, Guid addressbookId)
        {
            var emailSet = _refSetRepo.GetRefSet("Email_Type");
            var emailTerms = _metadataMappingRepo.GetRefTermsByRefSetId(emailSet.Id);
            var emailRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(emailSet.Id);

            IList<Email> emailsList = new List<Email>();
            var emailsInDB = _emailRepository.GetEmailsByAddressBookId(addressbookId);


            for (int i = 0; i < emails.Count(); i++)
            {
                var refTerm = emailTerms.SingleOrDefault(refTerm => refTerm.RefTerm_Key.ToLower() == emails.ElementAt(i).type.Key.ToLower());
                if (refTerm == null)
                    return new EmailResponse(false, $"Key {emails.ElementAt(i).type.Key} was not found.", null);
                var mapping = emailRefTermsWithMapping.SingleOrDefault(mapping => mapping.RefTerm_Id == refTerm.Id);
                if (mapping == null)
                    return new EmailResponse(false, $"Key {emails.ElementAt(i).type.Key} was not found.", null);

                emailsList.Add(new Email
                {
                    Id = emails.ElementAt(i).Id,
                    User_Email = emails.ElementAt(i).User_Email,
                    EmailTypeId = mapping.Id
                });
            }

            foreach (var email in emailsList)
            {
                int index = emailsInDB.ToList().FindIndex(emailFromDB => emailFromDB.Id == email.Id);
                var emailFromDB = emailsInDB.ToList().ElementAt(index);
                emailFromDB.User_Email = email.User_Email;
                emailFromDB.Id = email.Id;
            }

            return new EmailResponse(true, "", emailsInDB);
        }

        /// <summary>
        /// Method to Update Phone and return to Update Address Book Method 
        /// </summary>
        /// <param name="phones">Phone updated data from user</param>
        /// <param name="addressbookId">Id of Address Book</param>
        private PhoneResponse UpdatePhones(ICollection<PhoneUpdationDTO> phones, Guid addressbookId)
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
                var refTerm = phoneTerms.SingleOrDefault(refTerm => refTerm.RefTerm_Key.ToLower()
                == phones.ElementAt(i).Phone_type.Key.ToLower());
                if (refTerm == null)
                    return new PhoneResponse(false, $"Key {phones.ElementAt(i).Phone_type.Key} was not found.", null);
                var mapping = phoneRefTermsWithMapping.SingleOrDefault(mapping => mapping.Id == refTerm.Id);
                if (mapping == null)
                    return new PhoneResponse(false, $"Key {phones.ElementAt(i).Phone_type.Key} was not found.", null);

                phonesList.Add(new Phone
                {
                    Id = phones.ElementAt(i).Id,
                    Phone_Number = phones.ElementAt(i).Phone_Number,
                    PhoneTypeId = mapping.Id,
                });
            }

            foreach (var phone in phonesList)
            {
                int index = phonesInDB.ToList().FindIndex(phoneFromDB => phoneFromDB.Id == phone.Id);
                var phoneFromDB = phonesInDB.ToList().ElementAt(index);
                phoneFromDB.Phone_Number = phone.Phone_Number;
                phoneFromDB.PhoneTypeId = phone.PhoneTypeId;
            }

            return new PhoneResponse(true, null, phonesInDB);
        }

        /// <summary>
        /// Method to Update Address and return to Update Address Book Method 
        /// </summary>
        /// <param name="addresses">Updated addres from user</param>
        /// <param name="addressBookId">Id of Address Book</param>
        private AddressResponse UpdateAddresses(ICollection<AddressUpdationDTO> addresses, Guid addressBookId)
        {
            var addressSet = _refSetRepo.GetRefSet("ADDRESS_TYPE");
            var addressTerms = _metadataMappingRepo.GetRefTermsByRefSetId(addressSet.Id);
            var addressRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(addressSet.Id);

            var countrySet = _refSetRepo.GetRefSet("COUNTRY_TYPE");
            var countryTerms = _metadataMappingRepo.GetRefTermsByRefSetId(countrySet.Id);
            var countryRefTermsWithMapping = _metadataMappingRepo.GetRefTermMappingId(countrySet.Id);

            IList<Address> addressesList = new List<Address>();
            var addressesInDB = _addressRepository.GetAddresssByAddressBookId(addressBookId);

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

                var addressRefTerm = addressTerms.SingleOrDefault(refTerm => refTerm.RefTerm_Key.ToLower() == address.type.Key.ToLower());
                if (addressRefTerm == null)
                    return new AddressResponse(false, $"Key {address.type.Key} was not found.", null);
                var addressMapping = addressRefTermsWithMapping.SingleOrDefault(mapping => mapping.Id == addressRefTerm.Id);
                if (addressMapping == null)
                    return new AddressResponse(false, $"Key {address.type.Key} was not found.", null);

                var countryRefTerm = countryTerms.SingleOrDefault(refTerm => refTerm.RefTerm_Key.ToLower() == address.Country.Key.ToLower());
                if (countryRefTerm == null)
                    return new AddressResponse(false, $"Key {address.Country.Key} was not found.", null);
                var countryMapping = countryRefTermsWithMapping.SingleOrDefault(mapping => mapping.Id == countryRefTerm.Id);
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
                    AddressTypeId = addressMapping.Id,
                    CountryTypeId = countryMapping.Id,
                });
            }

            foreach (var address in addressesList)
            {
                int index = addressesInDB.ToList().FindIndex(addressFromDB => addressFromDB.Id == address.Id);
                var addressFromDB = addressesInDB.ToList().ElementAt(index);
                addressFromDB.Line1 = address.Line1;
                addressFromDB.Line2 = address.Line2;
                addressFromDB.City = address.City;
                addressFromDB.State_Name = address.State_Name;
                addressFromDB.ZipCode = address.ZipCode;
                addressFromDB.AddressTypeId = address.AddressTypeId;
                addressFromDB.CountryTypeId = address.CountryTypeId;
            }

            return new AddressResponse(true, null, addressesInDB);
        }
    }
}
