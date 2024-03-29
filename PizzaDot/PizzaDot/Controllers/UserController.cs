﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaDot.Models;
using PizzaDot.Interfaces;
using PizzaDot.Services;

namespace PizzaDot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpPost("Register")]
        public IActionResult CreateUser(User user)
        {
            if (user == null)
                return BadRequest();

            try
            {
                var regUser = _user.RegisterUser(user);

                return Ok(new
                {
                    Message = "Registered Successfully!"
                });
            }
            catch (ArgumentException ex) // Catch the exception thrown from RegisterUser method
            {
                return StatusCode(500, new
                {
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPost("Verify")]
        public IActionResult Validate(User user)
        {
            if (user == null)
                return BadRequest(new {Message = "Wrong Username or Password!"});

            var token = _user.VerifyUser(user);

            if (token == null)
                return StatusCode(500);

            return Ok(new
            {
                Message = "Login Successfully!",
                Token = token
            }) ;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _user.GetAllUser();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _user.GetUserByID(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
