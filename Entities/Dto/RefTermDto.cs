using System;

namespace Entities.Dto
{
    public class RefTermDto
    {
        public string RefTerm_Key { get; set; }
        public string Description { get; set; }
    }
    public class RefTermToReturnDTO : RefTermDto
    {
        public Guid Id { get; set; }
    }
}
