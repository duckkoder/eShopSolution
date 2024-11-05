using eShopSolution.Application.System.Languages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        public LanguagesController(ILanguageService languageService) {
            _languageService = languageService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _languageService.GetAll();

            if(result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
