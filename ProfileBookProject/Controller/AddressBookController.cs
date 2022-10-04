using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace AddressProfileBookProject.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookServices _addressBookServices;
        private readonly IMapper _mapper;
        public AddressBookController(IAddressBookServices addressBookServices, IMapper mapper)
        {
            _addressBookServices = addressBookServices ??
                throw new ArgumentNullException(nameof(addressBookServices));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("RefSet")]
        public ActionResult AddRefSet([FromBody] RefSetDto refsetdto)
        {
            try
            {
                try
                {
                    var result = _addressBookServices.Add(refsetdto);
                    return Ok();
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }
            }
            catch(Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpPost("RefTerm")]
        public ActionResult AddRefTerm([FromBody] RefTermDto reftermdto)
        {
            try
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
            catch(Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpPost("RefSetTerm")]
        public ActionResult AddRefSetTerm([FromBody] RefSetTermDto refsettermdto)
        {
            try
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
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
            
        }

        [HttpGet("Count")]
        public ActionResult CountRecords()
        {

            try
            {
                var result = _addressBookServices.CountRecord();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
            
        }

        [HttpGet("account")]
        public ActionResult GetAddress()
        {
            var result = _addressBookServices.GetAddressBook();
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public ActionResult DeleteBand(Guid Id)
        {
            var bandFromRepo = _addressBookServices.GetAddress(Id);
            if (bandFromRepo == null)
                return NotFound();

            _addressBookServices.DeleteAddress(bandFromRepo);
            _addressBookServices.Save();

            return NoContent();
        }

        [HttpGet("Id")]
        public ActionResult GetAddressId([FromRoute] Guid Id)
        {
            var result = _addressBookServices.GetById(Id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
