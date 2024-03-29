﻿using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }

        [HttpPost("AddingProducts")] 
      //  [Authorize(Roles = "Supplier,Admin")]
        public async Task<IActionResult> AddProducts(ProductDTO obj)
        {
            if (obj == null)
            {
                return BadRequest("Details can't be null");
            }
         //   string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          //  int userId = Convert.ToInt32(id);
            var product = await _product.AddProductAsync(obj, 1);
            return Ok(product);
        }

        [HttpGet("view-all-products")]
        [ResponseCache(Duration = 60 ,Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> ViewProducts()
        {
            var res = await _product.View();
            return Ok(res);
        }

        [HttpDelete("delete-product-byid")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var res = await _product.DeleteProducts(id);
            return Ok(res);
        }

        [HttpPut("update-products"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProdcuts(ProductDTO obj)
        {
            string Uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(Uid);
            var res = await _product.UpdateProduct(obj, userId);
            return Ok(res);
        }
    }
}