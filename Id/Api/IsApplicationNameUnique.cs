namespace Id.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsApplicationNameUnique : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IStringLocalizer<IsApplicationNameUnique> t;

        public IsApplicationNameUnique(IApplicationService applicationService,
            IStringLocalizer<IsApplicationNameUnique> _t)
        {
            _applicationService = applicationService;
            t = _t;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> OnGetAsync(string name)
        {
            return Ok(new IsUniqueResponse(await _applicationService.IsApplicationNameUnique(name)));
        }

        private class IsUniqueResponse(bool unique)
        {
            public bool isUnique { get; set; } = unique;
        }
    }
}