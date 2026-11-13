using BankingSystem;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<Bank>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlite("Data Source=bank.db"));

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

app.MapGet("/api/bank/total-balance", (Bank bank) =>
{
    return bank.GetTotalBalance();
});

app.MapPost("/api/accounts", (CreateAccountRequest request, Bank bank) =>
{
    BankAccount account = request.Type == AccountType.Checking
        ? new CheckingAccount(request.InitialBalance)
        : new SavingsAccount(request.InitialBalance, 0.05m);

    bank.Add(account);
    return Results.Created($"/api/accounts/{account.AccountNumber}", account.AccountNumber);
});

app.MapPost("/api/transfers", (TransferRequest request, Bank bank) =>
{
    try
    {
        bool success = bank.Transfer(request.From, request.To, request.Amount);
        if (success) return Results.Ok("Перевод выполнен успешно!");
        return Results.BadRequest("Недостаточно средств.");
    }
    catch (AccountNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
});
app.MapGet("/api/accounts", (Bank bank) =>
{
    return bank.GetAllAccounts();
});

app.Run();

public record CreateAccountRequest(AccountType Type, decimal InitialBalance);