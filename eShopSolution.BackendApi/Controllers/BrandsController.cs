using Azure.Core;
using eShopSolution.Application.Catalog.Brands;
using eShopSolution.ViewModels.Catalog.Brands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly IBrandService _brandService;
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }


        [HttpPost]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> Create([FromForm] BrandCreateRequest request)
        {
            var result = await _brandService.Create(request);

            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] BrandUpdateRequest request)
        {
            var result = await _brandService.Update(request);

            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var result = await _brandService.Delete(Id);

            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAll();

            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _brandService.GetById(Id);

            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
