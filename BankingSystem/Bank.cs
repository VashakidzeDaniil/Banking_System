using System.Globalization;
using System.Text.Json;

namespace BankingSystem
{
    public class Bank
    {
        private readonly BankDbContext _context;
        public Bank(BankDbContext context)
        {
            _context = context;

            var allAccounts = _context.Accounts.ToList();
            if(allAccounts.Any())
            {
                int maxNumber = allAccounts
                    .Select(a => int.Parse(a.AccountNumber.Substring(3)))
                    .Max();
                BankAccount.SyncCounter(maxNumber);
            }
        }
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        public void Add(BankAccount account)
        {
            if (_context.Accounts.Any(a => a.AccountNumber == account.AccountNumber))
                throw new ArgumentException("Счёт с таким номером уже существует");

            _context.Accounts.Add(account);
            _context.SaveChanges();
        }
        public BankAccount? FindAccount(string accountNumber)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }
        public bool Transfer(string from, string to, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма перевода должна быть положительной");

            var withrrowAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == from);
            if (withrrowAccount == null) throw new AccountNotFoundException(from);
            var depositAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == to);
            if (depositAccount == null) throw new AccountNotFoundException(to);

            if(withrrowAccount.Withdraw(amount))
            {
                depositAccount.Deposit(amount);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public decimal GetTotalBalance()
        {
            return _context.Accounts.Sum(a => a.Balance);
        }
        public void TestTransfer(string from, string to, decimal amount)
        {
            try
            {
                bool ok = Transfer(from, to, amount);
                Console.WriteLine(ok ? "Перевод выполнен" : "Перевод не выполнен");
            }
            catch (BankException ex)
            {
                Console.WriteLine($"TestTransfer: перехватил банковскую ошибку — {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"TestTransfer: перехватил — {ex.Message}");
            }
        }
        public int CountByType(AccountType type)
        {
            return _context.Accounts.Count(a => a.Type == type);
        }
        public decimal GetAverageBalance()
        {
            if (!_context.Accounts.Any()) return 0m;
            return _context.Accounts.Average(a => a.Balance);
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
        public BankAccount? GetRichestAccount()
        {
            return _context.Accounts.OrderByDescending(a => a.Balance).FirstOrDefault();
        }
        public IEnumerable<string> GetAccountNumbersSorted()
        {
            return _context.Accounts.Select(a => a.AccountNumber).OrderBy(n => n);
        }
        public bool HasAccountAbove(decimal threshold)
        {
            return _context.Accounts.Any(a => a.Balance > threshold);
        }
        public IEnumerable<BankAccount> GetAccountsByType(AccountType type)
        {
            return _context.Accounts.Where(a => a.Type == type);
        }
        public decimal GetTotalBalanceByType(AccountType type)
        {
            return (_context.Accounts.Where(a => a.Type == type).Sum(a => a.Balance));
        }
        public BankAccount? GetFirstPoorAccount(decimal threshold)
        {
            return FindAccountBy(a => a.Balance < threshold);
        }
        public IEnumerable<BankAccount> GetAccountsDescendingByBalance()
        {
            return _context.Accounts.OrderByDescending(a => a.Balance);
        }
        public List<decimal> GetTopAccounts(int n)
        {
            return _context.Accounts.OrderByDescending(a => a.Balance).
                                   Take(n).
                                   Select(a => a.Balance).ToList();
        }
        public bool HasAnySavingsAccounts()
        {
            return _context.Accounts.Any(a => a.Type == AccountType.Savings);
        }
        public bool AllAccountsAbove(decimal threshold)
        {
            return _context.Accounts.All(a => a.Balance > threshold);
        }
        public bool HasAccountBelow(decimal threshold)
        {
            return _context.Accounts.Any(a => a.Balance < threshold);
        }
        public Dictionary<AccountType, List<string>> GetAccountsNumbersByType()
        {
            return _context.Accounts
                .Where(a => a.Balance < 5000m)
                .GroupBy(a => a.Type)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(a => a.AccountNumber).ToList()
                );
        }
        public Dictionary<AccountType, decimal> GetTotalBalanceByType()
        {
            return _context.Accounts.GroupBy(a => a.Type).
                                   ToDictionary(g => g.Key,
                                   g => g.Sum(a => a.Balance));
        }
        public List<string> GetAllSavingsAccountNumbers(decimal threshold)
        {
            return _context.Accounts.Where(a => a.Type == AccountType.Savings && a.Balance > threshold).
                                   OrderByDescending(a => a.Balance).
                                   Select(a => a.AccountNumber).ToList();
        }
        public AccountType? GetCommonAccount()
        {
            var group = _context.Accounts.GroupBy(a => a.Type)
                                       .OrderByDescending(g => g.Count())
                                       .FirstOrDefault();
            return group?.Key;
        }
        public bool HasAccountOutRange(decimal min, decimal max)
        {
            return _context.Accounts.Any(a => a.Balance < min || a.Balance > max);
        }
        public decimal GetRoundAverageCheckingBalance()
        {
            if (!_context.Accounts.Any(a => a.Type == AccountType.Checking)) return 0m;
            return Math.Round(_context.Accounts.Where(a => a.Type == AccountType.Checking).
                                              Average(a => a.Balance));
        }
        public Dictionary<AccountType, int> GetTotalCountByType()
        {
            return _context.Accounts.GroupBy(a => a.Type).
                                   ToDictionary(g => g.Key,
                                                g => g.Count());
        }
        public BankAccount? FindAccountBy(Func<BankAccount, bool> predicate)
        {
            return _context.Accounts.FirstOrDefault(predicate);
        }
        public void SaveAccounts(string path)
        {
            File.WriteAllLines(path, _context.Accounts.Select(a => a.ToSaveLine()));
        }
        public void LoadAccounts(string path)
        {
            List<string> loadedNumpers = new List<string>();

            foreach (string line in File.ReadAllLines(path))
            {
                string[] parts = line.Split(';');
                if (parts.Length < 3) continue;

                loadedNumpers.Add(parts[0]);

                AccountType? type = ParseAccountType(parts[1]);
                if (type == null) continue;

                decimal balance = decimal.Parse(parts[2], CultureInfo.InvariantCulture);

                BankAccount account;
                switch (type.Value)
                {
                    case AccountType.Checking:
                        account = new CheckingAccount(parts[0], balance);
                        break;
                    case AccountType.Savings:
                        if (parts.Length < 4) continue;
                        decimal rate = decimal.Parse(parts[3], CultureInfo.InvariantCulture);
                        account = new SavingsAccount(parts[0], balance, rate);
                        break;
                    default:
                        continue;
                }
                Add(account);
            }
            SyncCounterFromNumbers(loadedNumpers);
        }
        public HashSet<AccountType> GetDistinctAccountTypes()
        {
            return _context.Accounts.Select(a => a.Type).ToHashSet();
        }
        private static void SyncCounterFromNumbers(IEnumerable<string> numbers)
        {
            if (numbers.Any())
            {
                BankAccount.SyncCounter(numbers.Where(n => n.Length > 3)
                                                     .Select(n => int.TryParse(n.Substring(3), out int v) ? v : -1)
                                                     .Where(v => v >= 0)
                                                     .DefaultIfEmpty(0)
                                                     .Max());
            }
        }
        public decimal GetAverageBalanceOfType(AccountType type)
        {
            if (!_context.Accounts.Any(a => a.Type == type)) return 0m;
            return _context.Accounts.Where(a => a.Type == type)
                                  .Average(a => a.Balance);
        }
        public bool AnyAccountWithBalanceBelow(decimal threshold)
        {
            return _context.Accounts.Any(a => a.Balance < threshold);
        }
        public decimal GetMaxBalanceOfType(AccountType type)
        {
            if (!_context.Accounts.Any(a => a.Type == type)) return 0m;
            return _context.Accounts.Where(a => a.Type == type).Max(a => a.Balance);
        }
        /// <summary>
        /// Все счета указанного типа выше порога. На пустом наборе возвращает true
        /// (стандартная семантика LINQ All — vacuous truth).
        /// </summary>
        public bool AllAccountsOfTypeAbove(AccountType type, decimal threshold)
        {
            return _context.Accounts.Where(a => a.Type == type)
                                  .All(a => a.Balance > threshold);
        }
        public decimal GetMinBalanceOfType(AccountType type)
        {
            if (!_context.Accounts.Any(a => a.Type == type)) return 0m;
            return _context.Accounts.Where(a => a.Type == type)
                                  .Min(a => a.Balance);
        }
        public BankAccount? GetRichestOfType(AccountType type)
        {
            return _context.Accounts.Where(a => a.Type == type)
                                  .MaxBy(a => a.Balance);
        }
        public Dictionary<AccountType, decimal> GetAverageByType()
        {
            return _context.Accounts.GroupBy(a => a.Type)
                                  .ToDictionary(g => g.Key,
                                                g => g.Average(a => a.Balance));
        }
        public Dictionary<AccountType, List<string>> GetNumbersByTypeAbove(decimal threshold)
        {
            return _context.Accounts.Where(a => a.Balance > threshold)
                           .GroupBy(a => a.Type)
                           .ToDictionary(g => g.Key, g => g.Select(a => a.AccountNumber).ToList());
        }
        public decimal GetTotalBalanceOfTypes(IEnumerable<AccountType> types)
        {
            return _context.Accounts.Where(a => types.Contains(a.Type)).Sum(a => a.Balance);
        }
        public List<BankAccount> GetTopRichAccounts(int n)
        {
            return _context.Accounts.OrderByDescending(a => a.Balance).Take(n).ToList();
        }
        public Dictionary<AccountType, decimal> GetTotalBalanceByTypeAbove(decimal threshold)
        {
            return _context.Accounts.Where(a => a.Balance > threshold)
                                  .GroupBy(g => g.Type)
                                  .ToDictionary(g => g.Key,
                                                g => g.Sum(a => a.Balance));
        }
        public List<string> GetNumbersAboveSorted(decimal threshold)
        {
            return _context.Accounts.Where(a => a.Balance > threshold)
                                  .OrderByDescending(a => a.Balance)
                                  .Select(a => a.AccountNumber)
                                  .ToList();
        }
        public bool AnyAccountAboveInType(AccountType type, decimal threshold)
        {
            return _context.Accounts.Any(a => a.Type == type && a.Balance > threshold);
        }
        public List<BankAccount> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }
    }
}