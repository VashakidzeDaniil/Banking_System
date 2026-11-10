namespace C_Arrays
{
    class Program
    {
        static void Main()
        {
            PrintWorkerInfo(new Employee("Иван", "Кассир", 1000m));
            var bank = new Bank();
            bank.Add(new CheckingAccount(1000m));
            bank.Add(new SavingsAccount(5000m, 0.05m));
            bank.Add(new CheckingAccount(4000m));
            bank.Add(new SavingsAccount(999m, 0.05m));
            bank.Add(new SavingsAccount(999m, 0.05m));
            bank.SaveAccounts(Settings.SaveFileName);

            foreach (var account in bank.GetDistinctAccountTypes())
            {
                Console.WriteLine(account);
            }

            var bank1 = new Bank();
            bank1.LoadAccounts(Settings.SaveFileName);

            foreach (var account in bank1.GetDistinctAccountTypes())
            {
                Console.WriteLine(account);
            }

            var requests = new Queue<TransferRequest>();
            requests.Enqueue(new TransferRequest("RU-0001", "RU-0002", 100m));
            requests.Enqueue(new TransferRequest("RU-0002", "RU-0003", 50m));
            requests.Enqueue(new TransferRequest("RU-0001", "RU-0003", 25m));
            Console.WriteLine("Начинаю обработку очереди");

            while (requests.Count > 0)
            {
                TransferRequest req = requests.Dequeue();
                bool ok = bank.Transfer(req.From, req.To, req.Amount);
                Console.WriteLine(ok
                    ? $"Перевод выполнен: с аккаунта {req.From} на аккаунт {req.To} сумма: {req.Amount}"
                    : $"Перевод НЕ выполнен: с аккаунта {req.From} на аккаунт {req.To} сумма: {req.Amount}");

            }
            Console.WriteLine("Очередь пуста");

            PrintAuditInfo(bank.FindAccount("RU-0001")!);
            PrintAuditInfo(bank.FindAccount("RU-0002")!);

            var requestsStack = new Stack<TransferRequest>();
            requestsStack.Push(new TransferRequest("RU-0001", "RU-0002", 100m));
            requestsStack.Push(new TransferRequest("RU-0002", "RU-0003", 50m));
            requestsStack.Push(new TransferRequest("RU-0001", "RU-0003", 25m));
            Console.WriteLine("Начинаю обработку Стека");
            while (requestsStack.Count > 0)
            {
                TransferRequest req = requestsStack.Pop();
                bool ok = bank.Transfer(req.From, req.To, req.Amount);
                Console.WriteLine(ok
                    ? $"Перевод выполнен: с аккаунта {req.From} на аккаунт {req.To} сумма: {req.Amount}"
                    : $"Перевод НЕ выполнен: с аккаунта {req.From} на аккаунт {req.To} сумма: {req.Amount}");

            }
            Console.WriteLine("Стек пуст");
            foreach (var item in bank.GetAccountsByType(AccountType.Savings))
            {
                Console.WriteLine(item);
            }
            foreach (var item in bank.GetBalancesDescending())
            {
                Console.WriteLine(item);
            }
            foreach (var item in bank.GetAccountNumbersSorted())
            {
                Console.WriteLine(item);
            }

            var snapshot = bank.GetAccountsByType(AccountType.Savings);
            bank.Add(new SavingsAccount(3000m, 0.05m));
            foreach (var item in snapshot)
                Console.WriteLine(item);
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
    }
}