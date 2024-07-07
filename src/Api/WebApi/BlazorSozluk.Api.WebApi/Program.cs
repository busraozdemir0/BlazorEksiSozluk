using BlazorSozluk.Api.Application.Extensions;
using BlazorSozluk.Api.Infrastructure.Persistence.Extensions;
using BlazorSozluk.Api.WebApi.Infrastructure.ActionFilters;
using BlazorSozluk.Api.WebApi.Infrastructure.Extensions;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(opt => opt.Filters.Add<ValidateModelStateFilter>())
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddFluentValidation()
    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationRegistration();
builder.Services.AddInfrastructureRegistration(builder.Configuration);

builder.Services.ConfigureAuth(builder.Configuration);

// Add Cors => bir wep api'ye disaridan gelen isteklere izin vermek icin eklenmesi gerekir.
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    // Her turlu istek ulasabilmesi icin hepsine izin verdik
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Hatalarin handle edilmesi
app.ConfigureExceptionHandling(app.Environment.IsDevelopment());

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("MyPolicy");

app.MapControllers();

app.Run();
