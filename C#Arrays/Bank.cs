namespace C_Arrays
{
    public class Bank
    {
        private Dictionary<string, BankAccount> accounts = new();
        public void Add(BankAccount account)
        {
            if (accounts.ContainsKey(account.AccountNumber))
                throw new ArgumentException("Счёт с таким номером уже существует");

            accounts[account.AccountNumber] = account;
        }
        public BankAccount FindAccount(string accountNumber)
        {
            accounts.TryGetValue(accountNumber, out BankAccount found);
            return found;
        }
        public bool Transfer(string from, string to, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма перевода должна быть положительной");

            if (!accounts.TryGetValue(from, out BankAccount withdrawAccount)) return false;
            if (!accounts.TryGetValue(to, out BankAccount depositAccount)) return false;

            if (withdrawAccount.Withdraw(amount))
            {
                depositAccount.Deposit(amount);
                return true;
            }
            return false;
        }
        public decimal GetTotalBalance()
        {
            decimal total = 0;

            foreach (var pair in accounts.Values)
            {
                total += pair.Balance;
            }
            return total;
        }
        public void TestTransfer(string from, string to, decimal amount)
        {
            try
            {
                bool ok = Transfer(from, to, amount);
                Console.WriteLine(ok ? "Перевод выполнен" : "Перевод не выполнен");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"TestTransfer: перехватил — {ex.Message}");
            }
        }
        public int CountByType(Type type)
        {
            int result = 0;
            foreach (var account in accounts.Values)
            {
                if (account.GetType() == type)
                    result++;
            }
            return result;
        }
        public decimal GetAverageBalance()
        {
            decimal sumValues = 0m;

            foreach (var account in accounts.Values)
                sumValues += account.Balance;

            return accounts.Count == 0 ? 0m : sumValues / accounts.Count;
        }
    }
}