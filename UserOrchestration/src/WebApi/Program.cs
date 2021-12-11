using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UserOrchestration.Core;
using UserOrchestration.Data.EntityFrameworkCore;
using UserOrchestration.EntityFrameworkCore;
using UserOrchestration.Utilities.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AutoMapper_MarkedAssemblies());
builder.Services.AddMediator(mediator => {
    mediator.AddRequestClient<CreateUser>();
});

builder.Services.AddUserOrchestration(userOrchestration => {
    userOrchestration
        .OnUtilities(utilities => utilities.Defaults())
        .AddEntityFrameworkCore(entityFrameworkCore => {
            entityFrameworkCore.AddUserDbContextFactory(options => options.UseNpgsql(builder.Configuration.GetConnectionString("UserOrchestration")));
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapUserEndpoints();

app.Run();

static Assembly[] AutoMapper_MarkedAssemblies() => new Assembly[]
{
    UserOrchestration.EntityFrameworkCore.AssemblyMarker.Assembly,
    UserOrchestration.WebApi.AssemblyMarker.Assembly
};