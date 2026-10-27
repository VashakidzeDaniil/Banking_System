namespace C_Arrays
{
    class Program
    {
        static void Main()
        {
            var accounts = new BankAccount[]
            {
                new BankAccount(1000m),
                new SavingsAccount(2000m, 0.05m),
                new BankAccount(300m),
                new SavingsAccount(500m, 0.10m)
            };

            foreach (var account in accounts)
            {
                if (account is SavingsAccount savings)
                {
                    savings.AddInterest();
                }
                account.PrintInfo();
            }
        }
    }
}