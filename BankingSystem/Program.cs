namespace BankingSystem
{
    class Program
    {
        static async Task Main()
        {
            var bank = new Bank();
            bank.Add(new CheckingAccount(1000m));
            bank.Add(new SavingsAccount(5000m, 0.05m));
            bank.Add(new CheckingAccount(4000m));
            bank.Add(new SavingsAccount(999m, 0.05m));
            bank.Add(new SavingsAccount(999m, 0.05m));

            Console.WriteLine("Start");
            await Task.Delay(2000);
            Console.WriteLine("End 2s");

            await bank.SaveToJsonAsync("bank.json");

            var restored = await bank.LoadFromJsonAsync("bank.json");
            if (restored != null)
            {
                var restoredBank = new Bank();
                foreach (var pair in restored)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                    restoredBank.Add(pair.Value);
                }
                restoredBank.TestTransfer("RU-0001", "RU-0002", 100m);
            }
            else
            {
                Console.WriteLine("Файл не найден — банк не восстановлен");
            }
        }
    }
}