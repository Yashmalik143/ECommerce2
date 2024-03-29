﻿using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _user;

        public UsersController(IUser user)
        {
            _user = user;
        }

        [HttpGet("get-all-users")]
        [customAuthorize]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IList<UserResponseDTO>> GetAll()
        {
            var res = await _user.AllUsers();

            return res;
        }

        [HttpDelete("delete-user-by-id")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var res = await _user.DeleteUser(id);

            return Ok(res);
        }

        [HttpPost("add-customer")]
        public async Task<UserDTO> AddCustomer(UserDTO obj)
        {
            //if (obj == null)
            //{
            //    return BadRequest("Name cant be null");
            //}
            int role = 2;
            var res = await _user.AddUserasync(obj, role);

            return res; 
        }

        [HttpPost("add-suppiler"), Authorize(Roles = "Admin")]
        public IActionResult AddSuppiler(UserDTO obj)
        {
            if (obj == null)
            {
                return BadRequest("Name cant be null");
            }
            int role = 2;
            var res = _user.AddUserasync(obj, role);

            return Ok(res.Result);
        }

        [HttpPost("login")]
        public IActionResult Login(UserDTO userDTO)
        {
            if (userDTO.ID == null)
            {
                return BadRequest("UserId can't be blank");
            }
            var res = _user.Login(userDTO.ID);
            return Ok(res);
        }
    }
}