namespace BankingSystem
{
    public record TransferRequest(string From, string To, decimal Amount);
}
