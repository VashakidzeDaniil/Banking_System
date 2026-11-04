using System.Text;

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
            return SumBy(a => a.Balance);
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
        public int CountByType(AccountType type)
        {
            return accounts.Values.Count(a => a.Type == type);
        }
        public decimal GetAverageBalance()
        {
            if (accounts.Values.Count <= 0) return 0m;
            return accounts.Values.Average(a => a.Balance);
        }
        public string BuildReport()
        {
            StringBuilder sb = new();
            sb.AppendLine("===Отчет по банку ===");
            foreach (var pair in accounts)
            {
                sb.AppendLine($"{pair.Key}: {pair.Value.Balance}");
            }
            sb.AppendLine($"Общий счет: {GetTotalBalance()}");

            return sb.ToString();
        }
        static public string DescribeAccount(AccountType type)
        {
            switch (type)
            {
                case AccountType.Checking:
                    return "Расчётный счёт: снятия без ограничений";
                case AccountType.Savings:
                    return "Накопительный счёт: начисляются проценты";
                default:
                    return "Неизвестный тип";
            }
        }
        static public string DescribeAccountShort(AccountType type) => type switch
        {
            AccountType.Checking => "Расчётный счёт: снятия без ограничений",
            AccountType.Savings => "Накопительный счёт: начисляются проценты",
            _ => "Неизвестный тип"
        };
        static public AccountType? ParseAccountType(string input)
        {
            if (Enum.TryParse<AccountType>(input.Trim(), ignoreCase: true, out var result))
                return result;
            return null;
        }
        public BankAccount GetRichestAccount()
        {
            return accounts.Values.OrderByDescending(a => a.Balance).FirstOrDefault();
        }
        public List<string> GetAccountNumbersSorted()
        {
            return accounts.Values.Select(a => a.AccountNumber).OrderBy(n => n).ToList();
        }
        public bool HasAccountAbove(decimal threshold)
        {
            return accounts.Values.Any(a => a.Balance > threshold);
        }
        public List<BankAccount> GetAccountsByType(AccountType type)
        {
            return accounts.Values.Where(a => a.Type == type).ToList();
        }
        public decimal GetTotalBalanceByType(AccountType type)
        {
            return SumBy( a => a.Type == type ? a.Balance : 0m);
        }
        public BankAccount GetFirstPoorAccount(decimal threshold)
        {
            return FindAccountBy(a => a.Balance < threshold);
        }
        public List<decimal> GetBalancesDescending()
        {
            return accounts.Values.OrderByDescending(a => a.Balance)
                                  .Select(a => a.Balance).ToList();
        }
        public List<decimal> GetTopAccounts(int n)
        {
            return accounts.Values.OrderByDescending(a => a.Balance).
                                   Take(n).
                                   Select(a => a.Balance).ToList();
        }
        public bool HasAnySavingsAccounts()
        {
            return accounts.Values.Any(a => a.Type == AccountType.Savings);
        }
        public bool AllAccountsAbove(decimal threshold)
        {
            return accounts.Values.All(a => a.Balance > threshold);
        }
        public bool HasAccountBelow(decimal threshold)
        {
            return accounts.Values.Any(a => a.Balance < threshold);
        }
        public Dictionary<AccountType, List<string>> GetAccountsNumbersByType()
        {
            return accounts.Values
                .Where(a => a.Balance < 5000m)
                .GroupBy(a => a.Type)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(a => a.AccountNumber).ToList()
                );
        }
        public Dictionary<AccountType, decimal> GetTotalBalanceByType()
        {
            return accounts.Values.GroupBy(a => a.Type).
                                   ToDictionary(g => g.Key,
                                   g => g.Sum(a => a.Balance));
        }
        public List<string> GetAllSavingsAccountNumbers(decimal threshold)
        {
            return accounts.Values.Where(a => a.Type == AccountType.Savings && a.Balance > threshold).
                                   OrderByDescending(a => a.Balance).
                                   Select(a => a.AccountNumber).ToList();
        }
        public AccountType GetCommonAccount()
        {
            return accounts.Values.GroupBy(a => a.Type).
                                   OrderByDescending(g => g.Count()).
                                   First().Key;
        }
        public bool HasAccountWithRange(decimal min, decimal max)
        {
            return accounts.Values.Any(a => a.Balance < min || a.Balance > max);
        }
        public decimal GetRoundAverageCheckingBalance()
        {
            if (accounts.Values.Count <= 0) return 0m;
            return Math.Round(accounts.Values.Where(a => a.Type == AccountType.Checking).
                                              Average(a => a.Balance));                       
        }
        public Dictionary<AccountType, int> GetTotalCountByType()
        {
            return accounts.Values.GroupBy(a => a.Type).
                                   ToDictionary(g => g.Key,
                                                g => g.Count());
        }
        public void ProcessAccounts(Action<BankAccount> action)
        {
            foreach (var account in accounts.Values)
            {
                action?.Invoke(account);
            }
        }
        public BankAccount? FindAccountBy(Func<BankAccount, bool> predicate)
        {
            return accounts.Values.FirstOrDefault(predicate);
        }
        public decimal SumBy(Func<BankAccount, decimal> selector)
        {
            return accounts.Values.Sum(selector);
        }

    }
}