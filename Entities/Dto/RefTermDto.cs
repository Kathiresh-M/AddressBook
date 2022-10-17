namespace Entities.Dto
{
        public class RefTermDTO
        {
            public string RefTerm_Key { get; set; }
            public string Description { get; set; }
        }

        public class RefTermCreationDTO : RefTermDTO { }

        public class RefTermUpdationDTO : RefTermDTO { }

        public class RefTermToReturnDTO : RefTermDTO
        {
            public Guid Id { get; set; }
        }
}
