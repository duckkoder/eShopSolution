using Azure.Core;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}


		[HttpGet("{languageId}")]
		public async Task<IActionResult> get(string languageId)
		{
			var products = await _productService.GetAll(languageId);
			return Ok(products);
		}

		[HttpGet("paging/{languageId}")]
		public async Task<IActionResult> GetAllPaging([FromRoute] string languageId, [FromQuery] GetPublicProductPagingRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var products = await _productService.GetAllByCatagoryId(languageId, request);
			return Ok(products);
		}


		[HttpGet("{productId}/{languageId}")]
		public async Task<IActionResult> GetById(int productId, string languageId)
		{
			var product = await _productService.GetById(productId, languageId);

			if (product == null)
				return BadRequest("Cannot Find Product");

			return Ok(product);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromQuery] ProductCreateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var productId = await _productService.Create(request);
			if (productId == 0)
				return BadRequest();

			var product = await _productService.GetById(productId, request.LanguageId);
			return CreatedAtAction(nameof(GetById), new { id = productId }, product);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromQuery] ProductUpdateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _productService.Update(request);
			if (result == 0)
				return BadRequest();

			return Ok();
		}
		[HttpDelete("{productId}")]
		public async Task<IActionResult> Delete(int productId)
		{
			var result = await _productService.Delete(productId);
			if (result == 0)
				return BadRequest();

			return Ok();
		}
		[HttpPatch("{productId}/{newPrice}")]
		public async Task<IActionResult> UpdatePrice(int productId, int newPrice)
		{
			var result = await _productService.UpdatePrice(productId, newPrice);

			if (!result)
				return BadRequest("Cannot Update Price.");
			return Ok();
		}

		[HttpPut("ViewCount/{productId}")]
		public async Task<IActionResult> AddViewcount(int productId)
		{
			var result = await _productService.AddViewcount(productId);
			if (result == 0)
				return BadRequest("Cannot Add Viewcount.");
			return Ok();
		}



		[HttpPut("{productId}/UpdateStock/{addedQuantity}")]
		public async Task<IActionResult> UpdateStock(int productId, int addedQuantity)
		{
			var result = await _productService.UpdateStock(productId, addedQuantity);

			if (!result)
				return BadRequest("Cannot Update Stock.");
			return Ok();
		}



		//image

		[HttpPost("{productId}/image")]
		public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var imageId = await _productService.AddImage(productId, request);

			if (imageId == 0)
				return BadRequest();
			var image = await _productService.GetImageById(imageId);

			return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
		}

		[HttpGet("{productId}/ListImage")]
		public async Task<IActionResult> GetListImage(int productId)
		{
			var result = await _productService.GetListImages(productId);
			return Ok();
		}

		[HttpGet("image/{imageId}")]
		public async Task<IActionResult> GetImageById(int imageId)
		{
			var image = await _productService.GetImageById(imageId);

			if (image == null)
				return BadRequest();
			return Ok(image);
		}

		[HttpPut("{productId}/UpdateImages/{imageId}")]
		public async Task<IActionResult> UpdateImage(int imageId, [FromBody] ProductImageUpdateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var result = await _productService.UpdateImage(imageId, request);

			if (result == 0)
				return BadRequest();
			return Ok();
		}
		[HttpDelete("{productId}/image/{imageId}")]
		public async Task<IActionResult> RemoveImage(int imageId)
		{
			var result = await _productService.RemoveImage(imageId);
			if (result == 0)
				return BadRequest();
			return Ok();
		}
	}
}
