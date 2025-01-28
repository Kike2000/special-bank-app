using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpecialBankAPI.Data;
using SpecialBankAPI.Services.Implementations;
using SpecialBankAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddDbContext<SpecialBankDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SpecialBankDbConnection") + ";TrustServerCertificate=True"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Special Bank API",
        Version = "v1",
        Description = "Backend",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Enrique Carrillo",
            Email = "pedro.crodriguez18@gmail.com",
            Url = new Uri("https://github.com/Kike2000")
        }
    });
});

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
