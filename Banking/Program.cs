using Banking.Api.Application.Services;
using Banking.Api.Features.Accounts.Create;
using Banking.Api.Features.Accounts.GetBalance;
using Banking.Api.Features.Customers.Create;
using Banking.Api.Features.Transactions.Deposit;
using Banking.Api.Features.Transactions.History;
using Banking.Api.Features.Transactions.Withdraw;
using Banking.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;
var env = builder.Environment;

// Add services to the container.
var dbPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, cfg["Database:Path"]!));
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<IInterestService, InterestService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation (lo conectamos cuando creemos validadores)
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();
// Aplciar migraciones pendientes al iniciar la app
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Health
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.UseHttpsRedirection();

app.UseAuthorization();

// Enpoints mapping
app.MapCreateCustomer();
app.MapCreateBankAccount();
app.MapGetAccountBalance();
app.MapDeposit();
app.MapWithdraw();
app.MapGetTransactions();

app.Run();
