/*using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressProfileBookProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateBookServices _authenticateBookServices;
        private readonly IMapper _mapper;
        private readonly IJWTManagerServices _jWTManagerServices;

        public AuthenticateController(IAuthenticateBookServices authenticateBookServices, IMapper mapper
            , IJWTManagerServices jWTManagerServices)
        {
            _authenticateBookServices = authenticateBookServices;
            _mapper = mapper;
            _jWTManagerServices = jWTManagerServices;
        }

        [HttpPost("signin")]
        public IActionResult Authenticate(LoginDto login)
        {
            var token = _jWTManagerServices.Authenticate(login);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost("CreateAddress")]
        public ActionResult<ProfilesDto> CreateBand([FromBody] ProfileforCreatingDto profileforcreatingdto)
        {
            var bandEntity = _mapper.Map<Profiles>(profileforcreatingdto);
            _authenticateBookServices.AddAddress(bandEntity);

            _authenticateBookServices.Save();

            var bandToReturn = _mapper.Map<ProfilesDto>(bandEntity);

            return CreatedAtRoute("GetBand", new { bandId = bandToReturn.Id }, bandToReturn);
        }


    }
}*/
using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressProfileBookProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IAuthenticateBookServices _authenticateBookServices;
        private readonly IMapper _mapper;
        private readonly IJWTManagerServices _jWTManagerServices;

        public authController(IAuthenticateBookServices authenticateBookServices, IMapper mapper
            , IJWTManagerServices jWTManagerServices)
        {
            _authenticateBookServices = authenticateBookServices;
            _mapper = mapper;
            _jWTManagerServices = jWTManagerServices;
        }

        //[AllowAnonymous]
        [HttpPost("signin")]
        public IActionResult Authenticate(LoginDto login)
        {
            var token = _jWTManagerServices.Authenticate(login);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost("CreateAddress")]
        public ActionResult<ProfilesDto> CreateBand([FromBody] ProfileforCreatingDto profileforcreatingdto)
        {
            var bandEntity = _mapper.Map<Profiles>(profileforcreatingdto);
            _authenticateBookServices.AddBand(bandEntity);
            _authenticateBookServices.Save();

            var bandToReturn = _mapper.Map<ProfilesDto>(bandEntity);

            return CreatedAtRoute("GetBand", new { bandId = bandToReturn.Id }, bandToReturn);
        }
    }
}