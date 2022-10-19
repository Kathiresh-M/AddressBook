using AutoMapper;
using Contracts;
using Entities.Dto;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AddressProfileBookProject.Controller
{
    [Route("api/metadata")]
    [ApiController]
    public class MetaDataController : ControllerBase
    {
        private readonly ILogger<accountController> _logger;
        private readonly IMetadataServices _metadataServices;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        public MetaDataController(IAddressBookServices addressBookServices, IMapper mapper,
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

        /// <summary>
        /// Method to get list of reference term under a refernce set
        /// </summary>
        /// <param name="Id">reference set Id</param>
        /// <returns>list of reference terms</returns>
        [HttpGet("refset/{refSetId}")]
        public IActionResult GetAllRefTerm(Guid Id)
        {

            Guid tokenUserId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Info("Invalid ref set id " + tokenUserId);
                return BadRequest("Invalid ref set id");
            }

            var response = _metadataServices.GetRefTermsByRefSetId(Id);

            var refSetToReturn = _mapper.Map<ICollection<RefTermToReturnDTO>>(response.RefTerms);

            return Ok(refSetToReturn);
        }
    }
}
