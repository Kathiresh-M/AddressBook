using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace AddressProfileBookProject.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class accountController : ControllerBase
    {
        private readonly IAddressBookServices _addressBookServices;
        private readonly ILogger<accountController> _logger;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        public accountController(IAddressBookServices addressBookServices, IMapper mapper, 
            ILogger<accountController> logger, ILog log)
        {
            _addressBookServices = addressBookServices ??
                throw new ArgumentNullException(nameof(addressBookServices));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _log = log??
                throw new ArgumentNullException(nameof(log));
        }

        [HttpPost("ref-set")]
        public ActionResult AddRefSet([FromBody] RefSetDto refsetdto)
        {
            var result = _addressBookServices.Add(refsetdto);
            return Ok();
        }

        [HttpPost("ref-term")]
        public ActionResult AddRefTerm([FromBody] RefTermDto reftermdto)
        {
           
            try
                {
                    var result = _addressBookServices.AddRefTerm(reftermdto);
                    return Ok();
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }
        }

        [HttpPost("ref-set-term")]
        public ActionResult AddRefSetTerm([FromBody] RefSetTermDto refsettermdto)
        {
            
            try
                {
                    var result = _addressBookServices.AddRefSetTerm(refsettermdto);
                    return Ok();
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }
            
        }

        [HttpGet("count")]
        public ActionResult GetCount()
        {
                var result = _addressBookServices.CountRecord();
                return Ok(result);
        }

        [HttpGet("account")]
        public ActionResult GetAddress()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _addressBookServices.GetAddressBook();
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public ActionResult DeleteAddressBook(Guid Id)
        {
            var bandFromRepo = _addressBookServices.GetAddress(Id);

            if (bandFromRepo == null)
                return NotFound();

            _addressBookServices.DeleteAddress(bandFromRepo);
            _addressBookServices.Save();

            return Ok();
        }

        [HttpGet("Id")]
        public ActionResult GetAddressId([FromRoute] Guid Id)
        {
            if (Id == null || Id == Guid.Empty)
            {
                _log.Error("Invalid address book id");
                return BadRequest("Not a valid address book ID.");
            }
            var result = _addressBookServices.GetById(Id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
