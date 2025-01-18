using eShopSolution.Application.Catalog.Products;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductSizes;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Carts
{
    public class CartService : ICartService
    {
        private readonly EShopDbContext _context;
        private readonly IProductService _productService;
        public CartService(IProductService producService,EShopDbContext context) {
            _productService = producService;
            _context = context;
        }

        public async Task<ApiResult<bool>> AddProductToCart(AddToCartRequest request)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == request.Id && x.ProductId==request.ProductId&&x.Size == request.ProductSize.Name);
            if (cart != null)
            {
                cart.Quantity += request.ProductSize.Quantity;
                cart.DateCreated = DateTime.Now;

                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }

            cart = new Cart
            {
                ProductId = request.ProductId,
                Size = request.ProductSize.Name,
                Price = request.Price,
                DateCreated = request.CreatedAt,
                UserId = request.Id,
                Quantity = request.ProductSize.Quantity
            };
            await _context.Carts.AddAsync(cart);
            if(await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>($"Can't Add Product Id {request.ProductId} To Cart");
        }

        
        

        public async Task<ApiResult<CartViewModel>> GetCartByUserID(Guid Id,string languageId)
        {
            var cart = await _context.Carts.Where(c => c.UserId == Id).ToListAsync();
            List<CartItem> items = new List<CartItem>();
            decimal total = 0;
            foreach (var c in cart) {

                var product = await _productService.GetById(c.ProductId, languageId);
                if (!product.IsSuccessed ) {
                    return new ApiErrorResult<CartViewModel>($"Can't Found The Product ID{c.ProductId}");
                }

                total = total + product.ResultObj.Price*c.Quantity;
                items.Add(new CartItem
                {
                    Product = product.ResultObj,
                    Size = new ProductSizeViewModel()
                    {
                        Id = _context.Sizes.FirstOrDefault(x=> x.Name == c.Size).Id,
                        Name = c.Size,
                        Quantity = c.Quantity
                    }
                });
            }
            return new ApiSuccessResult<CartViewModel>(new CartViewModel { UserId = Id,Items = items, Total = total });
        }

        public async Task<ApiResult<bool>> UpdateCart(UpdateCartRequest request)
        {
            var cart = await _context.Carts.Where(c => c.UserId == request.UserId).ToListAsync();
            var items = request.Items;
            
            foreach (var c in cart)
            {
                var item = items.FirstOrDefault(x => x.ProductId == c.ProductId && x.Size.Name == c.Size);
                if (item == null)
                {
                    cart.Remove(c);
                    continue;
                }

                c.ProductId = item.ProductId;
                c.Quantity = item.Size.Quantity;
                c.Size = item.Size.Name;
                c.DateCreated = DateTime.Now;
                c.Price = item.Price;
            }

            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage("Update Cart Success!");
            return new ApiSuccessResult<bool>().CreateMessage("Nothing Changes To Update!");
        }
    }
}
