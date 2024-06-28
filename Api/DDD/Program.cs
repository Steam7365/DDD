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
    options.Password.RequireDigit = false;//���������Ƿ����������
    options.Password.RequireLowercase = false;//�Ƿ����Сд
    options.Password.RequireNonAlphanumeric = false;//�Ƿ���������
    options.Password.RequireUppercase = false;//�Ƿ�����д
    options.Password.RequiredLength = 6;//����Ϊ6
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;//������������
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;//������������
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
idBuilder.AddEntityFrameworkStores<MyContext>()
    .AddDefaultTokenProviders().AddRoleManager<RoleManager<Role>>()
    .AddUserManager<UserManager<User>>(); 
#endregion

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoProfile));

#region JWT
//�������ļ��л�ȡJWT����
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    //�������ļ���JWT����ת��ΪJWToptions�����
    var jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
    //�����Գư�ȫ��Կ
    byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
    var secKey = new SymmetricSecurityKey(keyBytes);
    //������֤��������
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
