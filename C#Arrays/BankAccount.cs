namespace C_Arrays
{
    public class BankAccount
    {
        protected decimal balance;
        private string accountNumber;
        private static int totalAccounts;


        public BankAccount(decimal startBalance)
        {
            totalAccounts++;
            accountNumber = $"RU-{totalAccounts:D4}";
            balance = startBalance;
        }
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Нельзя прибавить денег меньше или равное 0");
            }
            balance += amount;
        }
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма должна быть больше нуля");
            }
            if (amount > balance)
            {
                throw new InvalidOperationException("Недостаточно средств");
            }
            balance -= amount;
        }
        public static int GetTotalAccountsCount()
        {
            return totalAccounts;
        }
        public virtual void PrintInfo()
        {
            Console.WriteLine($"Номер Аккаунта:{accountNumber}\nБаланс:{balance}");
        }
    }
}
