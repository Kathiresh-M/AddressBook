using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;

namespace AddressProfileBookProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IAuthenticateBookServices _authenticateBookServices;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public authController(IAuthenticateBookServices authenticateBookServices, 
            IMapper mapper, ILog log)
        {
            _authenticateBookServices = authenticateBookServices;
            _mapper = mapper;
            _log = LogManager.GetLogger(typeof(accountController));
        }

        [HttpPost("signin")]
        public IActionResult AuthUser([FromBody] LoginDto user)
        {
            if (!ModelState.IsValid)
            {
                _log.Error("Invalid login details used.");
                return BadRequest("Enter valid user data");
            }

            var response = _authenticateBookServices.AuthUser(user);
            var token = new Token(response.AccessToken, response.TokenType);
            return Ok(token);
        }

        /*[HttpPost("CreateAddress")]
        public ActionResult<ProfilesDto> CreateBand([FromBody] ProfileforCreatingDto profileforcreatingdto)
        {
            var result = _mapper.Map<Profiles>(profileforcreatingdto);
            _authenticateBookServices.AddAddressBook(result);
            _authenticateBookServices.Save();

            var AddressToReturn = _mapper.Map<ProfilesDto>(result);

            return CreatedAtRoute("GetBand", new { bandId = AddressToReturn.Id }, AddressToReturn);
        }*/
    }
}