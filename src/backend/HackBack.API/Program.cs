using HackBack.API.Extensions;
using HackBack.Application.ServiceRegistration;
using HackBack.Infrastructure.ServiceRegistration;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddCustomSwaggerGen();

services.AddAuthentificationRules(configuration);
services.AddAuthorizationPermissionRequirements();

services
    .AddApplication()
    .AddInfrastructure(configuration);

var app = builder.Build();

app.UseResultSharpLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();