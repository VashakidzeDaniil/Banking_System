namespace C_Arrays
{
    class Program
    {
        private static int transferNumber = 0;
        static void PrintAuditInfo(IAuditable item)
        {
            Console.WriteLine(item.GetAuditInfo());
        }
        static void PrintWorkerInfo(IPrintable item)
        {
            Console.WriteLine(item.GetPrintableInfo());
        }
        static void Describe(object item)
        {
            if (item is IAuditable auditable)
            {
                PrintAuditInfo(auditable);
            }
            if (item is IPrintable printable)
            {
                PrintWorkerInfo(printable);
            }
            if (item is not IAuditable and not IPrintable)
            {
                Console.WriteLine("Объект неизвестного типа");
            }
        }
        static void Main()
        {
          //var worker = new Employee("Dannil", "Cook",1000m);
            var bank = new Dictionary<string, BankAccount>();
            var acc1 = new CheckingAccount(1000);
            var acc2 = new SavingsAccount(2200m,0.05m);
            var acc3 = new CheckingAccount(2020);
            var acc4 = new SavingsAccount(5100m, 0.05m);

            bank[acc1.AccountNumber] = acc1;
            bank.Add(acc2.AccountNumber, acc2);
            bank.Add(acc3.AccountNumber, acc3);
            bank.Add(acc4.AccountNumber, acc4);

            Console.WriteLine($"Количество расчетных счетов: {CountByType(bank,typeof(CheckingAccount))}");
            Console.WriteLine($"Количество сберегательных счетов: {CountByType(bank, typeof(SavingsAccount))}");


            /*
            var accounts = new List<BankAccount>
            {
                new CheckingAccount(1000m),
                new SavingsAccount(2000m, 0.05m)
            };
            accounts.Add(new SavingsAccount(5000m, 0.05m));
            accounts.Add(new CheckingAccount(5000m));

            var accountToAdd = new CheckingAccount(1000m, "TEST-123");
            accounts.Add(accountToAdd);

            Console.WriteLine($"До удаления: {accounts.Count}");

            accounts.Remove(new CheckingAccount(1000m, "TEST-123"));

            Console.WriteLine($"После удаления: {accounts.Count}");

            foreach (var account in accounts)
            {
                account.ApplyMonthlyUpdate();
                //account.PrintInfo();

                if (account is IAuditable auditable)
                {
                    PrintAuditInfo(auditable);
                }
            }*/
        }
        static public void PrintAccount(Dictionary<string, BankAccount>bank, string accountNumber)
        {
            if (bank.TryGetValue(accountNumber, out BankAccount found))
            {
                found.PrintInfo();
            }
            else
            {
                Console.WriteLine("Счет не найден");
            }
        }
        static public void PrintDictionary(Dictionary<string, BankAccount>bank)
        {
            foreach (var pair in bank)
            {
                Console.WriteLine($"Номер: {pair.Key}");
                pair.Value.PrintInfo();
            }
        }
        static public decimal GetTotalBalance(Dictionary<string, BankAccount>bank)
        {
            decimal total = 0;

            foreach (var pair in bank.Values)
            {
                total += pair.Balance;
            }
            return total;
        }

        static bool Transfer(Dictionary<string, BankAccount> bank, string from, string to, decimal amount)
        {
            if (!bank.TryGetValue(from, out BankAccount withdrawAccount)) return false;
            if (!bank.TryGetValue(to, out BankAccount depositAccount)) return false;

            if(withdrawAccount.Withdraw(amount))
            {
                depositAccount.Deposit(amount);
                return true;
            }
            return false;
        }
        static void TestTransfer(Dictionary<string,BankAccount>bank, string from, string to, decimal amount)
        {
            transferNumber++;
            Console.WriteLine($"Test transfer number:{transferNumber:D4}");
            Console.WriteLine("До перевода");
            PrintDictionary(bank);
            Console.WriteLine();
            bool ok = Transfer(bank, from, to, amount);
            Console.WriteLine(ok ? "Перевод выполнен" : "Перевод не выполнен: счёт не найден или недостаточно средств");
            Console.WriteLine("После перевода");
            PrintDictionary(bank);
            Console.WriteLine($"Общий счет всех аккаунтов: {GetTotalBalance(bank)}");
            Console.WriteLine();
        }
        static bool TryDivide(decimal a, decimal b, out decimal result)
        {
            if (b == 0)
            {
                result = 0m;
                return false;
            }
            result = a / b;
            return true;
        }

        static int CountByType(Dictionary<string, BankAccount> bank, Type type)
        {
            int result = 0;
            foreach (var account  in bank.Values)
            {
                if (account.GetType() == type)
                    result++;
            }
            return result;
        }
    }
}