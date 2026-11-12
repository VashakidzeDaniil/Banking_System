namespace BankingSystem
{
    class Program
    {
        static async Task Main()
        {
            var bank = new Bank();
            bank.Add(new CheckingAccount(1000m));
            bank.Add(new SavingsAccount(5000m, 0.05m));
            await bank.SaveToJsonAsync("bank.json");

            bool ok = await bank.LoadFromJsonAsync("bank.json");
            Console.WriteLine($"Загружено: {ok}");
            Console.WriteLine(bank.BuildReport());

            bank.Add(new CheckingAccount(100m));
            Console.WriteLine(bank.BuildReport());
        }
    }
}