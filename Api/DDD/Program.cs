using DDD.Application.AutoMapper;
using DDD.Domain.Model;
using DDD.Infrastructure.Common;
using DDD.Infrastructure.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//EF
builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")
    , x => x.MigrationsAssembly("DDD.Infrastructure.EF"))
);

builder.Services.AddDataProtection();

#region Identity
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;//设置密码是否必须是数字
    options.Password.RequireLowercase = false;//是否必须小写
    options.Password.RequireNonAlphanumeric = false;//是否必须非数字
    options.Password.RequireUppercase = false;//是否必须大写
    options.Password.RequiredLength = 6;//长度为6
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;//设置密码令牌
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;//设置邮箱令牌
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
idBuilder.AddEntityFrameworkStores<MyContext>()
    .AddDefaultTokenProviders().AddRoleManager<RoleManager<Role>>()
    .AddUserManager<UserManager<User>>(); 
#endregion

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoProfile));

#region JWT
//从配置文件中获取JWT数据
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    //将配置文件的JWT数据转换为JWToptions这个类
    var jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
    //创建对称安全密钥
    byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
    var secKey = new SymmetricSecurityKey(keyBytes);
    //令牌验证参数配置
    x.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = secKey
    };
}); 
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
