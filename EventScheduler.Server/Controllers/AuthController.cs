using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EventScheduler.Server.Services;
using EventScheduler.Server.DTOs;
using EventScheduler.Server.Utils;
using EventScheduler.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace EventScheduler.Server.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userSrv;
    private readonly IMapper _mapper;
    private readonly JwtTokenGen _jwtGen;
    public AuthController(IUserService userSrv, IMapper mapper, JwtTokenGen jwtGen)
    {
        _userSrv = userSrv;
        _mapper = mapper;
        _jwtGen = jwtGen;
    }

    //Login Endpoint//
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> auth_user([FromBody] UserLoginDto userLoginData)
    {
        try
        {
            User? user = await _userSrv.get_user(userLoginData.Email, userLoginData.Password);
            if (user==null)
            {
                return BadRequest(new ErrorDto { error_msgs= ["Incorrect Email Or Password"] });
            }
            string jwtToken = _jwtGen.get_token(user!);
            TokenDto token = new TokenDto { token = jwtToken };
            return Ok(token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Signup Endpoint//
    [HttpPost("signup")]
    public async Task<ActionResult<TokenDto>> add_user(UserSignupDto new_user)
    {
        try
        {

            bool is_user = await _userSrv.is_user(new_user.Email);
            if (is_user)
            {
                return BadRequest(new ErrorDto { error_msgs = ["Email already exists"] });
            }

            User new_user_data = _mapper.Map<User>(new_user);
            User created_user_data= await _userSrv.signup_user(new_user_data);
            string jwtToken= _jwtGen.get_token(created_user_data);
            TokenDto token= new TokenDto { token = jwtToken };
            return StatusCode(201, token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //get all users endpoint//
    [Authorize(Roles ="Admin")]
    [HttpGet("users")]
    public async Task<ActionResult<UserDto>> get_users()
    {
        try
        {
            List<User> users = await _userSrv.get_all_users();
            List<UserDto> usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
