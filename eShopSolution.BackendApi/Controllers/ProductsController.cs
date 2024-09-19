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
		private readonly IManageProductService _manageProductService;
		private readonly IPublicProductService _publicProductService;

		public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
		{
			_publicProductService = publicProductService;
			_manageProductService = manageProductService;
		}

		//PUBLIC Service

		[HttpGet("{languageId}")]
		public async Task<IActionResult> get(string languageId)
		{
			var products = await _publicProductService.GetAll(languageId);
			return Ok(products);
		}

		[HttpGet("public-paging/{languageId}")]
		public async Task<IActionResult> GetAllPaging([FromRoute] string languageId, [FromQuery] GetPublicProductPagingRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var products = await _publicProductService.GetAllByCatagoryId(languageId, request);
			return Ok(products);
		}


		//MANAGE Service

		[HttpGet("{productId}/{languageId}")]
		public async Task<IActionResult> GetById(int productId, string languageId)
		{
			var product = await _manageProductService.GetById(productId, languageId);

			if (product == null)
				return BadRequest("Cannot Find Product");

			return Ok(product);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromQuery] ProductCreateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var productId = await _manageProductService.Create(request);
			if (productId == 0)
				return BadRequest();

			var product = await _manageProductService.GetById(productId, request.LanguageId);
			return CreatedAtAction(nameof(GetById), new { id = productId }, product);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromQuery] ProductUpdateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _manageProductService.Update(request);
			if (result == 0)
				return BadRequest();

			return Ok();
		}
		[HttpDelete("{productId}")]
		public async Task<IActionResult> Delete(int productId)
		{
			var result = await _manageProductService.Delete(productId);
			if (result == 0)
				return BadRequest();

			return Ok();
		}
		[HttpPatch("{productId}/{newPrice}")]
		public async Task<IActionResult> UpdatePrice(int productId, int newPrice)
		{
			var result = await _manageProductService.UpdatePrice(productId, newPrice);

			if (!result)
				return BadRequest("Cannot Update Price.");
			return Ok();
		}

		[HttpPut("ViewCount/{productId}")]
		public async Task<IActionResult> AddViewcount(int productId)
		{
			var result = await _manageProductService.AddViewcount(productId);
			if (result == 0)
				return BadRequest("Cannot Add Viewcount.");
			return Ok();
		}



		[HttpPut("{productId}/UpdateStock/{addedQuantity}")]
		public async Task<IActionResult> UpdateStock(int productId, int addedQuantity)
		{
			var result = await _manageProductService.UpdateStock(productId, addedQuantity);

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
			var imageId = await _manageProductService.AddImage(productId, request);

			if (imageId == 0)
				return BadRequest();
			var image = await _manageProductService.GetImageById(imageId);

			return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
		}

		[HttpGet("{productId}/ListImage")]
		public async Task<IActionResult> GetListImage(int productId)
		{
			var result = await _manageProductService.GetListImages(productId);
			return Ok();
		}

		[HttpGet("image/{imageId}")]
		public async Task<IActionResult> GetImageById(int imageId)
		{
			var image = await _manageProductService.GetImageById(imageId);

			if (image == null)
				return BadRequest();
			return Ok(image);
		}

		[HttpPut("{productId}/UpdateImages/{imageId}")]
		public async Task<IActionResult> UpdateImage(int imageId, [FromBody] ProductImageUpdateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var result = await _manageProductService.UpdateImage(imageId, request);

			if (result == 0)
				return BadRequest();
			return Ok();
		}
		[HttpDelete("{productId}/image/{imageId}")]
		public async Task<IActionResult> RemoveImage(int imageId)
		{
			var result = await _manageProductService.RemoveImage(imageId);
			if (result == 0)
				return BadRequest();
			return Ok();
		}
	}
}
