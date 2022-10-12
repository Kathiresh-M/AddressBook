using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressProfileBookProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class assetController : ControllerBase
    {
        private readonly IAssetServices _assetServices;

        public assetController(IAssetServices assetServices)
        {
            _assetServices = assetServices;
        }
        [HttpPost("{addressBookId}")]
        public IActionResult UploadAsset(Guid addressBookId, [FromForm] IFormFile file)
        {
            var asset = new Assert();
            asset.Id = Guid.NewGuid();
            asset.DownloadUrl = GenerateDownloadUrl(asset.Id);
            var response = _assetService.AddAsset(addressBookId, asset, file);

            
            var assetToReturn = _mapper.Map<AssetToReturnDTO>(response.Asset);
            return Ok(assetToReturn);
        }

        private string GenerateDownloadUrl(Guid assetId)
        {
            return Url.Link("DownloadImage", new { Id = assetId });
        }
    }
}
