namespace C_Arrays
{
    class Program
    {
        static void Main()
        {
            var bank = new Bank();
            bank.Add(new CheckingAccount(1000m));
            bank.Add(new SavingsAccount(5000m, 0.05m));
            bank.Add(new CheckingAccount(4000m));
            bank.Add(new SavingsAccount(999m, 0.05m));
            bank.Add(new SavingsAccount(999m, 0.05m));

            bank.ProcessAccounts(a => a.BalanceChanged += PrintBalanceChange);
            bank.ProcessAccounts(a => a.LowBalance += PrintLowBalanceChange);
            bank.ProcessAccounts(a => a.Withdraw(950m));

            Console.WriteLine(Max(3, 7));
            Console.WriteLine(Max(10.5m, 9.9m));
            Console.WriteLine(Max(new CheckingAccount(100m), new CheckingAccount(200m)));

            var stringBox = new Box<string>();
            Console.WriteLine(stringBox.IsEmpty);
            stringBox.Put("HelloAlisa");
            Console.WriteLine(stringBox.IsEmpty);
            Console.WriteLine(stringBox.Get());

            var bankBox = new Box<BankAccount>();
            bankBox.Put(new CheckingAccount(1000m));
            Console.WriteLine(bankBox.Get().AccountNumber);

            var myList = new MyList<string>();
            for (int i = 0; i < 10; i++)
            {
                myList.Add(i.ToString());
            }
            Console.WriteLine(myList.Contains("1"));
            Console.WriteLine(myList.Contains("11"));
            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }
        }

        static public void PrintAuditInfo(IAuditable item)
        {
            Console.WriteLine(item.GetAuditInfo());
        }
        static public void PrintWorkerInfo(IPrintable item)
        {
            Console.WriteLine(item.GetPrintableInfo());
        }
        static public void PrintDictionary(Dictionary<string, BankAccount> bank)
        {
            foreach (var pair in bank)
            {
                Console.WriteLine($"Номер: {pair.Key}");
                pair.Value.PrintInfo();
            }
        }
        static public void PrintAccount(Dictionary<string, BankAccount> bank, string accountNumber)
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
        static void PrintBalanceChange(string number, decimal newBalance)
        {
            Console.WriteLine($"Счёт {number}: {newBalance}");
        }
        static void PrintLowBalanceChange(string number, decimal newBalance)
        {
            Console.WriteLine($"На счете {number} осталось мало средств: {newBalance}");
        }
        static void Swap<T>(List<T> list, int i, int j)
        {
            T data;
            data = list[i];
            list[i] = list[j];
            list[j] = data;
        }
        static T Max<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) > 0 ? a : b;
        }
        static T Min<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) < 0 ? a : b;
        }
    }
}