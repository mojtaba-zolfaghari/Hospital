using ElmahCore.Mvc;
using ElmahCore.Sql;
using Hospital.Api.QueueManagement.Utilities;
using Hospital.Application.Implementation;
using Hospital.Application.Interfaces;
using Hospital.Infrastructure.Contexts;
using Hospital.Shared.Repository;
using Inventory.Api.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<QueueDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddElmah<SqlErrorLog>(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SqlServerDatabaseSchemaName = "Errors"; //Defaults to dbo if not set
    options.SqlServerDatabaseTableName = "ElmahError"; //Defaults to ELMAH_Error if not set

});

builder.Services.AddTransient<IHospitalUnitOfWork, HospitalUnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());
builder.Services.AddEndpointsApiExplorer();


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen();
Infrastructure.Utitlities.Configuration.Configure(builder.Configuration);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
var CoresSpecification = "CoresSpecification";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CoresSpecification,
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

var app = builder.Build();
if (bool.Parse(builder.Configuration["EnableSwagger"]))
{

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocExpansion(DocExpansion.None);
    });
}



app.UseElmahExceptionPage();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseResponseCompression();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseElmah();
app.MapControllers();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Documents")),
//    RequestPath = "/Documents"
//});

app.UseCors(CoresSpecification);


if (bool.Parse(builder.Configuration["EnableAllLogs"]) == true) app.UseMiddleware<HttpRequestResponseLogger>();


app.Run();

