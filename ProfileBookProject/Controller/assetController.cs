using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AddressProfileBookProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class assetController : ControllerBase
    {
        private readonly IAssetServices _assetServices;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        private readonly ILogger<accountController> _logger;

        public assetController(IAssetServices assetServices, IMapper mapper,
            ILogger<accountController> logger, ILog log)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _assetServices = assetServices;
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _log = LogManager.GetLogger(typeof(accountController));
        }

        //// <summary>
        /// Method to upload an Asset
        /// </summary>
        /// <param name="Id">Address Book Id</param>
        /// <param name="file">asset file</param>
        /// <returns>asset meta data</returns>
        [HttpPost("{Id}")]
        public IActionResult UploadAsset(Guid Id, [FromForm] IFormFile file)
        {
            Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (!isValidToken)
            {
                _log.Warn("User with invalid token, trying to upload user data");
                return Unauthorized();
            }

            var asset = new Assert();
            asset.Id = Guid.NewGuid();
            asset.DownloadUrl = GenerateDownloadUrl(asset.Id);
            var response = _assetServices.AddAsset(Id, tokenUserId, asset, file);

            
            var assetToReturn = _mapper.Map<AssertDto>(response.Asset);
            return Ok(assetToReturn);
        }

        /// <summary>
        /// Method to Download an asset
        /// </summary>
        /// <param name="Id">Id of the Asset</param>
        /// <returns>asset file</returns>
        [HttpGet("{Id}")]
        public IActionResult DownloadAsset(Guid Id)
        {
            Guid tokenUserId;
            var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (!isValidToken)
            {
                _log.Warn("User with invalid token");
                return Unauthorized();
            }

            if (Id == null || Id == Guid.Empty)
            {
                _log.Error("Trying to access asset data with not a valid user id by user: " + tokenUserId);
                return BadRequest("Not a valid user ID.");
            }

            var response = _assetServices.GetAsset(Id, tokenUserId);

            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }

            byte[] bytes = Convert.FromBase64String(response.Asset.Content);

            return File(bytes, response.Asset.FileType, response.Asset.FileName);
        }

        private string GenerateDownloadUrl(Guid assetId)
        {
            return Url.Link("DownloadImage", new { Id = assetId });
        }
    }
}
