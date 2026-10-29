namespace C_Arrays
{
    class Program
    {
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
            }
        }
    }
}