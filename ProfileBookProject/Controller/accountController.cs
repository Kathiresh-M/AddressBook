using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Helper;
using System.Security.Claims;

namespace AddressProfileBookProject.Controller
{
    [Authorize]
    [Route("api/account")]
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

        /// <summary>
        /// Method to Create a RefSet 
        /// </summary>
        /// <param name="refsetdto">RefSet Data to be created</param>
        /// <returns></returns>
        [HttpPost("refset")]
        public IActionResult AddRefSet([FromBody] RefSetDto refsetdto)
        {
            var result = _addressBookServices.Add(refsetdto);
            return Ok();
        }

        /// <summary>
        /// Method to Create a RefTerm 
        /// </summary>
        /// <param name="reftermdto">RefTerm Data to be created</param>
        /// <returns></returns>
        [HttpPost("refterm")]
        public IActionResult AddRefTerm([FromBody] RefTermDto reftermdto)
        {
            var result = _addressBookServices.AddRefTerm(reftermdto);
            return Ok();    
        }

        /// <summary>
        /// Method to Create a RefSetTerm
        /// </summary>
        /// <param name="refsettermdto">RefSetTerm Data to be created</param>
        /// <returns></returns>
        [HttpPost("refsetterm")]
        public IActionResult AddRefSetTerm([FromBody] RefSetTermDto refsettermdto)
        {
            var result = _addressBookServices.AddRefSetTerm(refsettermdto);
            return Ok();
        }

        /// <summary>
        /// Method to create an address book
        /// </summary>
        /// <param name="addressBookData">address book data to be created</param>
        /// <returns>Id of the address book created</returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateAddressBook([FromBody]
        ProfileforCreatingDto addressBookData)
        {
            if (!ModelState.IsValid)
            {
                _log.Error("Invalid addressbook details used.");
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
                _log.Error("Trying to access address book count with not a valid user id by user: " + tokenUserId);
                return BadRequest("Not a valid user ID.");
            }

            var response = _addressBookServices.CreateAddressBook(addressBookData, tokenUserId);
            return Ok($"Address book created with ID: {response.addressBook.Id}");
        }

        /// <summary>
        /// Method to get a count of address book
        /// </summary>
        /// <returns>Count of Address Book</returns>
        [HttpGet("count")]
        public IActionResult GetAddressBookCount()
        {
            var result = _addressBookServices.CountAddressBook();
            return Ok(result);
        }

        /// <summary>
        /// Method to get a all address book
        /// </summary>
        /// <returns>an address book</returns>
        [HttpGet]
        public IActionResult GetAddressBooks()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _addressBookServices.GetAddressBooks();
            return Ok(result);
        }

        /// <summary>
        /// Method to delete an address book
        /// </summary>
        /// <param name="addressBookId">Id of the address book</param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public IActionResult DeleteAddressBook(Guid addressBookId)
        {
            Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (!isValidToken)
            {
                _log.Warn($"User with invalid token, trying to access address book data");
                return Unauthorized();
            }

            var addressBookResponseData = _addressBookServices.GetAddressBook(addressBookId, tokenUserId);

            var deleteResponse = _addressBookServices.DeleteAddressBook(addressBookId, tokenUserId);


            return Ok(addressBookResponseData.addressBook);

        }

        /// <summary>
        /// Method to get a particular address book
        /// </summary>
        /// <param name="Id">Address Book Id</param>
        /// <returns>an address book</returns>
        [HttpGet("{Id}")]
        public IActionResult GetAnAddressBook([FromRoute] Guid Id)
        {

	        Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

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

            var result = _addressBookServices.GetAddressBookById(Id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Method to update an address book
        /// </summary>
        /// <param name="addressBookId">Id of the address book in Database</param>
        /// <param name="addressBookData">address book data to be updated</param>
        /// <returns>Id of the address book created</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateAddressBook(Guid addressBookId, 
            [FromBody] AddressBookUpdate addressBookData)
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

            var response = _addressBookServices.UpdateAddressBook(addressBookData, 
                addressBookId, tokenUserId);

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
