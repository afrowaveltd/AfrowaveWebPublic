namespace Id.Api
{
   [Route("api/[controller]")]
   [ApiController]
   public class IsBrandNameUnique(IBrandService brandService) : ControllerBase
   {
      private readonly IBrandService _brandService = brandService;

      [HttpGet]
      [Route("{name}")]
      public async Task<IActionResult> OnGetAsync(string name)
      {
         return Ok(new IsUniqueResponse(await _brandService.IsBrandNameUnique(name)));
      }

      private class IsUniqueResponse(bool unique)
      {
         public bool isUnique { get; set; } = unique;
      }
   }
}