using AutoMapper;
using Contracts;
using Entities.Dto;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AddressProfileBookProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class metadataController : ControllerBase
    {
        private readonly ILogger<accountController> _logger;
        private readonly IMetadataServices _metadataServices;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        public metadataController(IAddressBookServices addressBookServices, IMapper mapper,
            ILogger<accountController> logger, ILog log, IMetadataServices metadataServices)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _metadataServices = metadataServices ??
                throw new ArgumentNullException(nameof(metadataServices));
            _log = LogManager.GetLogger(typeof(accountController));
        }

        [HttpGet("refset/{refSetId}")]
        public IActionResult GetAllRefTerm(Guid Id)
        {

            Guid tokenUserId;
            Guid.TryParse(Profile.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (refSetId == null || refSetId == Guid.Empty)
            {
                _log.Info("Invalid ref set id " + tokenUserId);
                return BadRequest("Invalid ref set id");
            }

            var response = _metadataServices.GetRefTermsByRefSetId(Id);

            var refSetToReturn = _mapper.Map<IEnumerable<RefTermToReturnDTO>>(response.RefTerms);

            return Ok(refSetToReturn);
        }
    }
}
