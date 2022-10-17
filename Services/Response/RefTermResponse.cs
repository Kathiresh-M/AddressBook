using Entities.Models;

namespace Services.Response
{
    public class RefTermResponse : MessageResponsetoUser
    {
        public IEnumerable<RefTerm> RefTerms { get; protected set; }

        public RefTermResponse(bool isSuccess, string message, IEnumerable<RefTerm> refTerms) : base(isSuccess, message)
        {
            RefTerms = refTerms;
        }
    }
}
