using System.Globalization;
using System.Text;
using System.Text.Json;

namespace BankingSystem
{
    public class Bank
    {
        private Dictionary<string, BankAccount> accounts = new();
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        public void Add(BankAccount account)
        {
            if (accounts.ContainsKey(account.AccountNumber))
                throw new ArgumentException("Счёт с таким номером уже существует");

            accounts[account.AccountNumber] = account;
        }
        public BankAccount? FindAccount(string accountNumber)
        {
            accounts.TryGetValue(accountNumber, out BankAccount found);
            return found;
        }
        public bool Transfer(string from, string to, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма перевода должна быть положительной");
            if (!accounts.TryGetValue(from, out BankAccount? withdrawAccount))
                throw new AccountNotFoundException(from);
            if (!accounts.TryGetValue(to, out BankAccount? depositAccount))
                throw new AccountNotFoundException(to);

            if (withdrawAccount.Withdraw(amount))
            {
                depositAccount.Deposit(amount);
                return true;
            }
            return false;
        }
        public decimal GetTotalBalance()
        {
            return accounts.Values.Sum(a => a.Balance);
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
            return accounts.Values.Count(a => a.Type == type);
        }
        public decimal GetAverageBalance()
        {
            if (accounts.Values.Count <= 0) return 0m;
            return accounts.Values.Average(a => a.Balance);
        }
        public string BuildReport()
        {

            var accountLines = accounts.Select(pair =>
            {
                return $"{pair.Key}: {pair.Value.Balance}";
            });

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==Отчет по банку==");
            sb.Append(string.Join(Environment.NewLine, accountLines));
            sb.AppendLine();
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
        public BankAccount? GetRichestAccount()
        {
            return accounts.Values.OrderByDescending(a => a.Balance).FirstOrDefault();
        }
        public IEnumerable<string> GetAccountNumbersSorted()
        {
            return accounts.Values.Select(a => a.AccountNumber).OrderBy(n => n);
        }
        public bool HasAccountAbove(decimal threshold)
        {
            return accounts.Values.Any(a => a.Balance > threshold);
        }
        public IEnumerable<BankAccount> GetAccountsByType(AccountType type)
        {
            return accounts.Values.Where(a => a.Type == type);
        }
        public decimal GetTotalBalanceByType(AccountType type)
        {
            return (accounts.Values.Where(a => a.Type == type).Sum(a => a.Balance));
        }
        public BankAccount? GetFirstPoorAccount(decimal threshold)
        {
            return FindAccountBy(a => a.Balance < threshold);
        }
        public IEnumerable<BankAccount> GetAccountsDescendingByBalance()
        {
            return accounts.Values.OrderByDescending(a => a.Balance);
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
        public AccountType? GetCommonAccount()
        {
            var group = accounts.Values.GroupBy(a => a.Type)
                                       .OrderByDescending(g => g.Count())
                                       .FirstOrDefault();
            return group?.Key;
        }
        public bool HasAccountOutRange(decimal min, decimal max)
        {
            return accounts.Values.Any(a => a.Balance < min || a.Balance > max);
        }
        public decimal GetRoundAverageCheckingBalance()
        {
            if (!accounts.Values.Any(a => a.Type == AccountType.Checking)) return 0m;
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
        public void SaveAccounts(string path)
        {
            File.WriteAllLines(path, accounts.Values.Select(a => a.ToSaveLine()));
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
        public void SaveReport(string path)
        {
            File.WriteAllText(path, BuildReport());
        }
        public HashSet<AccountType> GetDistinctAccountTypes()
        {
            return accounts.Values.Select(a => a.Type).ToHashSet();
        }
        public async Task<bool> LoadFromJsonAsync(string path)
        {
            if (!File.Exists(path)) return false;
            string json = await File.ReadAllTextAsync(path);
            Dictionary<string, BankAccount>? loaded = null;
            try
            {
                loaded = JsonSerializer.Deserialize<Dictionary<string, BankAccount>>(json, JsonOpts);
            }
            catch (JsonException js)
            {
                Console.WriteLine($"Ошибка в Десериализации — {js.Message}");
                return false;
            }
            if (loaded == null) return false;

            accounts.Clear();
            foreach (var pair in loaded)
            {
                Add(pair.Value);
            }
            SyncCounterFromNumbers(loaded.Keys);

            return true;
        }
        public async Task SaveToJsonAsync(string path)
        {
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(accounts, JsonOpts));
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
            if (!accounts.Values.Any(a => a.Type == type)) return 0m;
            return accounts.Values.Where(a => a.Type == type)
                                  .Average(a => a.Balance);
        }
        public bool AnyAccountWithBalanceBelow(decimal threshold)
        {
            return accounts.Values.Any(a => a.Balance < threshold);
        }
        public decimal GetMaxBalanceOfType(AccountType type)
        {
            if (!accounts.Values.Any(a => a.Type == type)) return 0m;
            return accounts.Values.Where(a => a.Type == type).Max(a => a.Balance);
        }
        /// <summary>
        /// Все счета указанного типа выше порога. На пустом наборе возвращает true
        /// (стандартная семантика LINQ All — vacuous truth).
        /// </summary>
        public bool AllAccountsOfTypeAbove(AccountType type, decimal threshold)
        {
            return accounts.Values.Where(a => a.Type == type)
                                  .All(a => a.Balance > threshold);
        }
        public decimal GetMinBalanceOfType(AccountType type)
        {
            if (!accounts.Values.Any(a => a.Type == type)) return 0m;
            return accounts.Values.Where(a => a.Type == type)
                                  .Min(a => a.Balance);
        }
        public BankAccount? GetRichestOfType(AccountType type)
        {
            return accounts.Values.Where(a => a.Type == type)
                                  .MaxBy(a => a.Balance);
        }
        public Dictionary<AccountType, decimal> GetAverageByType()
        {
            return accounts.Values.GroupBy(a => a.Type)
                                  .ToDictionary(g => g.Key,
                                                g => g.Average(a => a.Balance));
        }
        public Dictionary<AccountType, List<string>> GetNumbersByTypeAbove(decimal threshold)
        {
            return accounts.Values.Where(a => a.Balance > threshold)
                           .GroupBy(a => a.Type)
                           .ToDictionary(g => g.Key, g => g.Select(a => a.AccountNumber).ToList());
        }
        public decimal GetTotalBalanceOfTypes(IEnumerable<AccountType> types)
        {
            return accounts.Values.Where(a => types.Contains(a.Type)).Sum(a => a.Balance);
        }
        public List<BankAccount> GetTopRichAccounts(int n)
        {
            return accounts.Values.OrderByDescending(a => a.Balance).Take(n).ToList();
        }
        public Dictionary<AccountType, decimal> GetTotalBalanceByTypeAbove(decimal threshold)
        {
            return accounts.Values.Where(a => a.Balance > threshold)
                                  .GroupBy(g => g.Type)
                                  .ToDictionary(g => g.Key,
                                                g => g.Sum(a => a.Balance));
        }
        public List<string> GetNumbersAboveSorted(decimal threshold)
        {
            return accounts.Values.Where(a => a.Balance > threshold)
                                  .OrderByDescending(a => a.Balance)
                                  .Select(a => a.AccountNumber)
                                  .ToList();
        }
        public bool AnyAccountAboveInType(AccountType type, decimal threshold)
        {
            return accounts.Values.Any(a => a.Type == type && a.Balance > threshold);
        }
    }
}