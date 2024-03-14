using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web.Models.DTO.Login;
using Proyecto_Web.Models.DTO.Register;
using Proyecto_Web.Repositories.Interface;

namespace Proyecto_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO) 
        { 
            // User1@CodePulse.com
            // User1@123

            // Check Email
           var identityUser = await userManager.FindByEmailAsync(requestDTO.Email);

            if (identityUser is not null) { 
                // check password 
              var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, requestDTO.Password);

                if (checkPasswordResult) 
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    // Create a Token and Response
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                   
                    var response = new LoginResponseDTO
                    {
                        Email = requestDTO.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken

                    };


                    return Ok(response);

                }
            }
            ModelState.AddModelError("", "Email or Password is invalid");

            return ValidationProblem(ModelState);

        }
        

            
        

        // POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO requestDTO)
        {
            // Create IdentityUser object
            var user = new IdentityUser
            {
                UserName = requestDTO.Email?.Trim(),
                Email = requestDTO.Email,
            };

            // Create User
           var identityResult = await userManager.CreateAsync(user, requestDTO.Password);

            if (identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");
            
                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {

                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

            }
            else
            {
                if(identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors) { 
                    
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }

    }
}
