using Azure.Core;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}


		[HttpGet("{languageId}")]
		public async Task<IActionResult> GetAll(string languageId)
		{
			var result = await _productService.GetAll(languageId);
			if(!result.IsSuccessed)
			{
				return BadRequest(result);
			}
			return Ok(result);
			
		}

		[HttpGet("paging/{languageId}")]
		public async Task<IActionResult> GetAllByCatagoryId([FromRoute] string languageId, [FromQuery] GetPublicProductPagingRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var result = await _productService.GetAllByCatagoryId(languageId, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

		[HttpGet("paging")]
        public async Task<IActionResult> GetAllByKeywordAndCatagoryId([FromQuery] GetManageProductPagingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _productService.GetAllPagingByKeywordAndatagoryId(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



        [HttpGet("{productId}/{languageId}")]

		public async Task<IActionResult> GetById(int productId, string languageId)
		{
			var result = await _productService.GetById(productId, languageId);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

		[HttpPost]
        [Authorize]
		[Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _productService.Create(request);
		
			/*var product = await _productService.GetById(result.ResultObj, request.LanguageId);
			return CreatedAtAction(nameof(GetById), new { id = result.ResultObj }, product);*/

			return Ok(result);
		}

		[HttpPut]
		[Authorize]
        public async Task<IActionResult> Update([FromBody] ProductUpdateRequest request)
		{
			var result = await _productService.Update(request);
        
            return Ok(result);

        }
		[HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int productId)
		{
			var result = await _productService.Delete(productId);
			if (!result.IsSuccessed)
				return BadRequest(result);

			return Ok(result);
		}
		[HttpPatch("{productId}/{newPrice}")]
        [Authorize]
        public async Task<IActionResult> UpdatePrice(int productId, int newPrice)
		{
			var result = await _productService.UpdatePrice(productId, newPrice);

			if (!result.IsSuccessed)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPut("{productId}/ViewCount")]
        [Authorize]
        public async Task<IActionResult> AddViewcount(int productId)
		{
			var result = await _productService.AddViewcount(productId);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

		



		[HttpPut("{productId}/UpdateStock/{addedQuantity}")]
        [Authorize]
        public async Task<IActionResult> UpdateStock(int productId, int addedQuantity)
		{
			var result = await _productService.UpdateStock(productId, addedQuantity);

            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("brand/{brandId:int}/language/{languageId}")]
        public async Task<IActionResult> GetAllProductByBrand(int brandId, string languageId)
        {
            var products = await _productService.GetAllProductByBrand(brandId, languageId);

            if (!products.IsSuccessed)
                return BadRequest(products);
            return Ok(products);
        }




        //image

        [HttpPost("{productId}/image")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var result = await _productService.AddImage(productId, request);

			if (!result.IsSuccessed)
				return BadRequest(result);
			return Ok(result);
		}

		
		[HttpGet("image/{imageId}")]
		public async Task<IActionResult> GetImageById(int imageId)
		{
			var result = await _productService.GetImageById(imageId);

			if (!result.IsSuccessed)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPut("{productId}/UpdateImages/{imageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage(int imageId, [FromBody] ProductImageUpdateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var result = await _productService.UpdateImage(imageId, request);

			if (!result.IsSuccessed)
				return BadRequest(result);
			return Ok(result);
		}
		[HttpDelete("{productId}/image/{imageId}")]
        [Authorize]
        public async Task<IActionResult> RemoveImage(int imageId)
		{
			var result = await _productService.RemoveImage(imageId);
			if (!result.IsSuccessed)
				return BadRequest(result);
			return Ok(result);
		}
		//Size
        [HttpGet("{productId}/quantity")]

        public async Task<IActionResult> GetQuantity(int productId)
        {
            var result = await _productService.GetQuantity(productId);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{productId}/quantity")]
        public async Task<IActionResult> UpdateQuantity(int productId, UpdateQuantityRequest request)
        {
            var result = await _productService.UpdateQuantity(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        //category 
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            var result = await _productService.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //brand


    }
}
