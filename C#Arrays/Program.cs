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

            PrintWorkerInfo(new Employee("Иван", "Кассир", 1000m));
            var a = new TransferRequest("RU-0001", "RU-0002", 100m);
            var b = new TransferRequest("RU-0001", "RU-0002", 100m);
            Console.WriteLine(a == b);
            var c = new TransferRequest("RU-0001", "RU-0002", 100m);
            var d = new TransferRequest("RU-0003", "RU-0004", 500m);
            Console.WriteLine(c == d);
            var request = new TransferRequest("RU-0001", "RU-0002", 100m);
            var bigger = request with { Amount = 500m };
            Console.WriteLine(request);
            Console.WriteLine(bigger);
            try
            {
                var bad = new SavingsAccount(1000m, 5m); // ставка 5 = 500%
                Console.WriteLine("Создался?! Так не должно быть");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Исключение поймано: {ex.Message}");
            }
            Console.WriteLine(bank.FindAccount("RU-0005")!.IsEmpty);
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
        static void Swap<T>(IList<T> list, int i, int j)
        {
            T data;
            data = list[i];
            list[i] = list[j];
            list[j] = data;
        }
        static T FindMax<T>(List<T> items) where T : IComparable<T>
        {
            T max = items[0];
            foreach (var item in items)
            {
                if (item.CompareTo(max) > 0)
                {
                    max = item;
                }
            }
                return max;
        }
        static T CreateDefault<T>() where T : new()
        {
            return new T();
        }
        static void PrintAll <T>(List<T> items) where T : IPrintable
        {
            foreach (var item in items)
                Console.WriteLine(item.GetPrintableInfo());
        }
    }
}