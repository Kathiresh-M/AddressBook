using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contracts
{
    public interface IAddressBookServices
    {
        RefSetDto Add(RefSetDto refsetdto);
        RefTermDto AddRefTerm(RefTermDto reftermdto);
        RefSetTermDto AddRefSetTerm(RefSetTermDto refsettermdto);
        int CountAddressBook();
        bool Save();
        Task File(ICollection<IFormFile> files);
        void DeleteAddress(Profiles profile);
        Profiles GetAddress(Guid Id);
        ProfileforCreatingDto GetAddressBookById(Guid Id);
        ActionResult<List<ProfileforCreatingDto>> GetAddressBooks();
        AddressBookResponsetoUser UpdateAddressBook(ProfileforCreatingDto addressBookData, Guid addressBookId, Guid userId);
    }
}

