using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AutoMapper_MarkedAssemblies());
builder.Services.AddMediator(mediator => {
    mediator.AddRequestClient<CreateUser>();
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