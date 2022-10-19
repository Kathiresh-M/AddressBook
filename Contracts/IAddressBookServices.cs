using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Response;

namespace Contracts
{
    public interface IAddressBookServices
    {
        RefSetDto Add(RefSetDto refsetdto);
        RefTermDto AddRefTerm(RefTermDto reftermdto);
        RefSetTermDto AddRefSetTerm(RefSetTermDto refsettermdto);
        AddressBookResponsetoUser CreateAddressBook(ProfileforCreatingDto addressBookData,
            Guid userId);
        int CountAddressBook();
        bool Save();
        Task File(ICollection<IFormFile> files);
        Profiles GetAddress(Guid Id);
        ProfileforCreatingDto GetAddressBookById(Guid Id);
        ActionResult<List<ProfileforCreatingDto>> GetAddressBooks();
        AddressBookResponsetoUser UpdateAddressBook(AddressBookUpdate addressBookData,
            Guid addressBookId, Guid userId);
        AddressBookResponsetoUser GetAddressBook(Guid addressBookId, Guid tokenUserId);
        MessageResponsetoUser DeleteAddressBook(Guid addressBookId, Guid userId);
    }
}

