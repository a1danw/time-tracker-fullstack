using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimeTracker.Shared.Models.Login;

namespace TimeTracker.API.Services
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<User> _signInManager; // identity verifies credentials
        private readonly IConfiguration _config; 
        private readonly UserManager<User> _userManager;

        public LoginService(SignInManager<User> signInManager, IConfiguration config, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _config = config;
            _userManager = userManager;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);            

            if(!result.Succeeded)
            {
                return new LoginResponse(false, "Email or password is incorrect.");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);
            if(user == null) {  
                return new LoginResponse(false, "User doesn't exist.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSecurityKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_config["Jwt:ExpiryInDays"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponse(true, Token: jwt); 
        }
    }
}
