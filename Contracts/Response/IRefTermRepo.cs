using Entities.Models;

namespace Contracts.Response
{
    public interface IRefTermRepo
    {
        RefTerm GetRefTerm(Guid refTermId);
        RefTerm GetRefTerm(string key);
    }
}
