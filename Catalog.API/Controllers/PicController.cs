namespace Catalog.API.Controllers
{
    [ApiController]
    public class PicController : ControllerBase
    {

        private readonly CatalogContext _catalogContext;
        private readonly IWebHostEnvironment _env;

        public PicController(CatalogContext catalogContext, IWebHostEnvironment env)
        {
            _catalogContext = catalogContext;
            _env = env;
        }

        [HttpGet]
        [Route("api/v1/catalog/items/{catalogItemId}/pic")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetImageAsync(int catalogItemId)
        {
            if (catalogItemId <= 0)
                return BadRequest();

            var item = await _catalogContext.CatalogItems
                .SingleOrDefaultAsync(x => x.Id == catalogItemId);

            if (item == null)
                return NotFound();

            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot, item.PictureFileName);

            string imageFileExtension = Path.GetExtension(item.PictureFileName);
            string mimeType = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

            var buffer = await System.IO.File.ReadAllBytesAsync(path);

            return File(buffer, mimeType);

        }

        private string GetImageMimeTypeFromImageFileExtension(string extension)
        {
            string mimetype;

            switch (extension)
            {
                case ".png":
                    mimetype = "image/png";
                    break;
                case ".gif":
                    mimetype = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimetype = "image/jpeg";
                    break;
                case ".bmp":
                    mimetype = "image/bmp";
                    break;
                case ".tiff":
                    mimetype = "image/tiff";
                    break;
                case ".wmf":
                    mimetype = "image/wmf";
                    break;
                case ".jp2":
                    mimetype = "image/jp2";
                    break;
                case ".svg":
                    mimetype = "image/svg+xml";
                    break;
                default:
                    mimetype = "application/octet-stream";
                    break;
            }

            return mimetype;
        }
    }
}
