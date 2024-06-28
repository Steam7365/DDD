using DDD.Domain.Model;
using DDD.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DDD.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityController : Controller
    {
        //通过RoleManager、UserManager等来进行数据操作。比如创建角色、创建用户。
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        public IdentityController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        private static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
        {
            DateTime expires = DateTime.Now.AddSeconds(options.ExpireSeconds);
            byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);//获取对称安全密钥
            var credentials = new SigningCredentials(secKey,
                SecurityAlgorithms.HmacSha256Signature);//令牌的加密方式
            var tokenDescriptor = new JwtSecurityToken(expires: expires,
                signingCredentials: credentials, claims: claims);//完成令牌的制作
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);//返回完成令牌数据
        }

        /// <summary>
        /// 创建角色和用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateUserRole()
        {
            bool roleExists = await roleManager.RoleExistsAsync("admin");//判断是否存在这个角色
            if (!roleExists)
            {
                Role role = new Role { Name = "Admin" };
                var r = await roleManager.CreateAsync(role);
                //创建失败返回错误信息
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
            }

            User user = await this.userManager.FindByNameAsync("rodio");//获取某用户信息
            if (user == null)
            {
                user = new User
                {
                    UserName = "rodio",
                    Email = "abc123888@gmail.com",
                    EmailConfirmed = true
                };
                var r = await userManager.CreateAsync(user, "123456");
                //创建用户信息失败返回错误信息
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
                //将角色和用户进行关联
                r = await userManager.AddToRoleAsync(user, "admin");
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
            }
            return Ok();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req"></param>
        /// <param name="jwtOptions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest req,
            [FromServices] IOptions<JWTOptions> jwtOptions)
        {
            string userName = req.UserName;
            string password = req.Password;
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"用户名不存在{userName}");
            }
            var success = await userManager.CheckPasswordAsync(user, password);
            if (!success)
            {
                return BadRequest("Failed");
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            var roles = await userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //登录成功后生成令牌，将令牌发送给客户端
            string jwtToken = BuildToken(claims, jwtOptions.Value);
            return Ok(jwtToken);
        }

        /// <summary>
        /// 判断是否登录成功
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult Hello()
        {
            string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
            string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
            return Ok($"id={id},userName={userName},roleNames ={roleNames}");
        }
    }
}
