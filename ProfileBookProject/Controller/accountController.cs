using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

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
            _logger =  logger ??
                throw new ArgumentNullException(nameof(logger));
            _log = LogManager.GetLogger(typeof(accountController));
        }

        [HttpPost("refset")]
        public IActionResult AddRefSet([FromBody] RefSetDto refsetdto)
        {
            var result = _addressBookServices.Add(refsetdto);
            return Ok();
        }

        [HttpPost("refterm")]
        public ActionResult AddRefTerm([FromBody] RefTermDto reftermdto)
        {
                    var result = _addressBookServices.AddRefTerm(reftermdto);
                    return Ok();    
        }

        [HttpPost("refsetterm")]
        public ActionResult AddRefSetTerm([FromBody] RefSetTermDto refsettermdto)
        {
                    var result = _addressBookServices.AddRefSetTerm(refsettermdto);
                    return Ok();
        }

        [HttpGet("count")]
        public IActionResult GetAddressBookCount()
        {
            var result = _addressBookServices.CountAddressBook();
                return Ok(result);
        }

        [HttpGet("")]
        public ActionResult GetAddressBooks()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _addressBookServices.GetAddressBooks();
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public ActionResult DeleteAddressBook(Guid Id)
        {
            Guid tokenUserId;
            var isValidToken = Guid.TryParse(Profile.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (!isValidToken)
            {
                _log.Warn($"User with invalid token, trying to access user data");
                return Unauthorized();
            }

            var addressfromrepo = _addressBookServices.GetAddress(Id);

            if (addressfromrepo == null)
                return NotFound();

            _addressBookServices.DeleteAddress(addressfromrepo);
            _addressBookServices.Save();

            return Ok();
        }

        [HttpGet("Id")]
        public ActionResult GetAnAddressBookId([FromRoute] Guid Id)
        {

	        Guid tokenUserId;
            var isValidToken = Guid.TryParse(Profile.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (!isValidToken)
            {
                _log.Warn($"User with invalid token, trying to access user data");
                return Unauthorized();
            }

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

        [HttpPut("{userId}")]
        public IActionResult UpdateAddressBook(Guid addressBookId, [FromBody] ProfileforCreatingDto addressBookData)
        {
            if (!ModelState.IsValid)
            {
                _log.Error("Invalid addressbook");
                return BadRequest("Enter valid addressbook data");
            }

            Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (!isValidToken)
            {
                _log.Warn($"User with invalid token, trying to access address book data");
                return Unauthorized();
            }

            if (tokenUserId == null || tokenUserId == Guid.Empty)
            {
                _log.Error("Trying to update address book with not a valid user id by user: " + tokenUserId);
                return BadRequest("Not a valid user ID.");
            }

            var response = _addressBookService.UpdateAddressBook(addressBookData, addressBookId);

            if (!response.IsSuccess && response.Message.Contains("Additional") || response.Message.Contains("duplication") || response.Message.Contains("not valid"))
            {
                return Conflict(response.Message);
            }

            if (!response.IsSuccess && response.Message.Contains("not found"))
            {
                return NotFound(response.Message);
            }

            return Ok("Address book updated successfully.");
        }
    }
}
