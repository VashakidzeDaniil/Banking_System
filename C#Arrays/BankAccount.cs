using System.Globalization;

namespace C_Arrays
{
    public abstract class BankAccount : IComparable<BankAccount>, IAuditable, IPrintable
    {
        public AccountType Type { get; init; }
        protected decimal balance;
        private string accountNumber;
        private static int totalAccounts;
        public decimal Balance => balance;
        public string AccountNumber => accountNumber;
        private const decimal LowBalanceThreshold = 100m;
        private bool lowBalanceNotified;

        static BankAccount()
        {
            Console.WriteLine("Статический конструктор BankAccount запущен");
        }
        public BankAccount(decimal startBalance, AccountType type)
        {
            if (startBalance < 0)
                throw new ArgumentException("Созданный счет не может быть ниже нуля");
            Type = type;
            totalAccounts++;
            accountNumber = $"RU-{totalAccounts:D4}";
            balance = startBalance;
        }
        protected BankAccount(string accountNumber, decimal startBalance, AccountType type)
        {
            if (startBalance < 0)
                throw new ArgumentException("Счёт не может быть ниже нуля");
            Type = type;
            this.accountNumber = accountNumber;
            balance = startBalance;
        }
        public delegate void BalanceChangedHandler(string accountNumber, decimal newBalance);
        public event BalanceChangedHandler? BalanceChanged;
        public event BalanceChangedHandler? LowBalance;
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Нельзя прибавить денег меньше или равное 0");
            balance += amount;
            BalanceChanged?.Invoke(AccountNumber, balance);
        }
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма должна быть больше нуля");

            if (amount > balance)
            {
                Console.WriteLine("Недостаточно средств, операция не выполнена");
                return false;
            }
            balance -= amount;

            BalanceChanged?.Invoke(AccountNumber, balance);
            if (balance <= LowBalanceThreshold && !lowBalanceNotified)
            {
                LowBalance?.Invoke(AccountNumber, balance);
                lowBalanceNotified = true;
            }
            if (balance > LowBalanceThreshold)
            {
                lowBalanceNotified = false;
            }
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
        public int CompareTo(BankAccount? other)
        {
            if (other is null) return 1;
            return balance.CompareTo(other.balance);
        }
        public virtual string ToSaveLine() =>
            $"{AccountNumber};{Type};{balance.ToString(CultureInfo.InvariantCulture)}";
        public static void SyncCounter(int maxValue)
        {
            if (maxValue > totalAccounts)
                totalAccounts = maxValue;
        }
        public abstract void ApplyMonthlyUpdate();
        public abstract string GetAuditInfo();
        public abstract string GetPrintableInfo();
        public override string ToString() => GetAuditInfo();
    }

}
