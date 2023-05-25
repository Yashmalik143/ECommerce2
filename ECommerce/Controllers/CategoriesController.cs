using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategories _categories;

        public CategoriesController(ICategories categories)
        {
            _categories = categories;
        }

        [HttpPost("add-categories"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategories(UserDTO category)
        {
            string Uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(Uid);
            var res = await _categories.CategoryAdd(category.Name, category.ID , userId);
            return Ok(res);
        }

        [HttpGet("view-categories")]
        public async Task<IActionResult> ViewCategories()
        {
            var res = await _categories.GetAllCategories();
            return Ok(res);
        }

        [HttpDelete("Delete-by-id"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategories(DeleteDTO d)
        {
            var delcat = await _categories.DeleteCategories(d.id);
            return Ok(delcat);
        }

        [HttpPut("update-categories"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategories(UserDTO userDTO)
        {
            string Uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(Uid);

            var update = await _categories.UpdateCategories(userDTO.Name, userDTO.ID, userId);
            return Ok(update);
        }
    }
}