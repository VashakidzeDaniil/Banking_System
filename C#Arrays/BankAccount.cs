namespace C_Arrays
{
    public abstract class BankAccount
    {
        protected decimal balance;
        private string accountNumber;
        private static int totalAccounts;

        public decimal Balance => balance;
        public string AccountNumber => accountNumber;


        public BankAccount(decimal startBalance, string accountNumber = null)
        {
            totalAccounts++;
            this.accountNumber = accountNumber ?? $"RU-{totalAccounts:D4}";
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
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Сумма должна быть больше нуля");
                return false;
            }
            if (amount > balance)
            {
                Console.WriteLine("Недостаточно средств, операция не выполнена");
                return false;
            }
            balance -= amount;
            return true;
        }
        public static int GetTotalAccountsCount()
        {
            return totalAccounts;
        }
        public virtual void PrintInfo()
        {
            Console.WriteLine($"Номер Аккаунта:{accountNumber}\nБаланс:{balance}");
        }

        public override bool Equals(object obj)
        {
            //if (obj == null) return false;
            //if (obj is not BankAccount) return false;  // или: if (!(obj is BankAccount))
            //var other = (BankAccount)obj;              // теперь приведение безопасно
            //return AccountNumber == other.AccountNumber;
            
            if (obj is not BankAccount other)
                return false;

            return accountNumber == other.accountNumber;
        }
        public override int GetHashCode()
        {
            return accountNumber.GetHashCode();
        }
        public abstract void ApplyMonthlyUpdate();
    }
}
