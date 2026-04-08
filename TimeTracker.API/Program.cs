using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TimeTracker.Shared.Models.Project;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => // applies authorization button to swagger
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
// builder.Services.AddOpenApi();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<User>(options => 
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<DataContext>();
// builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<DataContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],  
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]!))
        };
    }); // bearer string/token for every auth header http request
    

builder.Services.AddHttpContextAccessor(); // for accessing user context in services instead of passing through layers explicitly
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    /* Can be used in .NET 9 - app.MapScalarApiReference();
    app.MapOpenApi(); localhost:7189/scalar/v1.json */
    // https://localhost:7078/swagger/index.html

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
    // app.MapScalarApiReference(); - Add key: Authorization and Value: Bearer dgdgdgkey to the header of the request if using scalar
}

ConfigureMapster();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

void ConfigureMapster()
{
    // ProjectDetails is part of the Project, by default mapster cant interpret Descripion etc - map project details for returning to the client
    TypeAdapterConfig<Project, ProjectResponse>.NewConfig()
        .Map(dest => dest.Description, src => src.ProjectDetails != null ? src.ProjectDetails.Description : null)
        .Map(dest => dest.StartDate, src => src.ProjectDetails != null ? src.ProjectDetails.StartDate : null)
        .Map(dest => dest.EndDate, src => src.ProjectDetails != null ? src.ProjectDetails.EndDate : null);
}