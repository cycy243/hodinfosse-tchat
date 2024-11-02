using Microsoft.EntityFrameworkCore;
using Tchat.Api.Mappers;
using Tchat.Api.Data.Repository.DataBase;
using Tchat.Api.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Tchat.Api.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tchat.Api.Services.Utils;
using Tchat.Api.Services.User;
using Tchat.Api.Services.Messages.Utils;
using Tchat.Api.Services.Messages;
using Tchat.Api.Exceptions;
using Tchat.Api.Services;
using Tchat.Api.Services.Contact;
using Tchat.API.Policies.Requirement;
using Tchat.API.Policies;
using Tchat.API.Persistence;
using Tchat.API.Data.Persistence.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddEnvironmentVariables();

var confiugration = builder.Configuration;

var jwtConfiguration = new JwtConfiguration();
confiugration.GetSection("JwtConfiguration").Bind(jwtConfiguration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if(builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("TchatContext")));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("TchatContext")));
}

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// For Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtConfiguration.Issuer,
            ValidAudience = jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret))
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = confiugration["GoogleOAuthConfiguration:ClientId"] ?? throw new ArgumentNullException("Client secret for google shouldn't be null");
        options.ClientSecret = confiugration["GoogleOAuthConfiguration:ClientSecret"] ?? throw new ArgumentNullException("Client secret for google shouldn't be null");
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(PoliciesName.USER_MANAGEMENT), policyBuilder => policyBuilder.AddRequirements(new PolicyRoleRequirement(nameof(UserRoleEnum.Admin))));
    options.AddPolicy(nameof(PoliciesName.CONTACT_MESSAGE_MANAGEMENT), policyBuilder => policyBuilder.AddRequirements(new PolicyRoleRequirement(nameof(UserRoleEnum.Admin))));
}); 

var pusherConfiguration = new PusherConfig("", "", "", "", false);
confiugration.GetSection("Pusher").Bind(pusherConfiguration);

builder.Services
    .AddMappers()
    .AddServices()
    .AddRepositories()
    .AddAuthServices(jwtConfiguration)
    .AddUtilsServices(confiugration)
    .AddPusherMessageServices(pusherConfiguration)
    .AddExceptions()
    .AddUserServices()
    .AddHttpContextAccessor()
    .AddContactServices()
    .AddPolicies();
;

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptions();

app.UseAuthentication();
app.UseAuthorization();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    DataInitializer.SeedRole(roleManager);
    DataInitializer.Seed(userManager);
}


app.UseCors();

app.MapControllers();

app.Run();
