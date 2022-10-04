using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contracts
{
    public interface IAddressBookServices
    {
        ActionResult<RefSetDto> Add(RefSetDto refsetdto);
        ActionResult<RefTermDto> AddRefTerm(RefTermDto reftermdto);
        ActionResult<RefSetTermDto> AddRefSetTerm(RefSetTermDto refsettermdto);
        int CountRecord();
        bool Save();
        Task File(ICollection<IFormFile> files);
        void DeleteAddress(Profiles profile);
        Profiles GetAddress(Guid Id);
        ActionResult<ProfileforCreatingDto> GetById(Guid Id);
        ActionResult<List<ProfileforCreatingDto>> GetAddressBook();

    }
}

