using System.Globalization;
using System.Text.Json.Serialization;

namespace C_Arrays
{
    [JsonDerivedType(typeof(CheckingAccount), "checking")]
    [JsonDerivedType(typeof(SavingsAccount), "savings")]
    public abstract class BankAccount : IComparable<BankAccount>, IAuditable, IPrintable
    {
        public AccountType Type { get; init; }
        private string accountNumber;
        private static int totalAccounts;
        public decimal Balance { get; protected set; }
        public string AccountNumber => accountNumber;
        private const decimal LowBalanceThreshold = 100m;
        private bool lowBalanceNotified;
        public bool IsEmpty => Balance == 0;

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
            Balance = startBalance;
        }
        protected BankAccount(string accountNumber, decimal startBalance, AccountType type)
        {
            if (startBalance < 0)
                throw new ArgumentException("Счёт не может быть ниже нуля");
            Type = type;
            this.accountNumber = accountNumber;
            Balance = startBalance;
        }
        public delegate void BalanceChangedHandler(string accountNumber, decimal newBalance);
        public event BalanceChangedHandler? BalanceChanged;
        public event BalanceChangedHandler? LowBalance;
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Нельзя прибавить денег меньше или равное 0");
            Balance += amount;
            BalanceChanged?.Invoke(AccountNumber, Balance);
        }
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма должна быть больше нуля");

            if (amount > Balance)
            {
                Console.WriteLine("Недостаточно средств, операция не выполнена");
                return false;
            }
            Balance -= amount;

            BalanceChanged?.Invoke(AccountNumber, Balance);
            if (Balance <= LowBalanceThreshold && !lowBalanceNotified)
            {
                LowBalance?.Invoke(AccountNumber, Balance);
                lowBalanceNotified = true;
            }
            if (Balance > LowBalanceThreshold)
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
            Console.WriteLine($"Номер Аккаунта:{accountNumber}\nБаланс:{Balance}");
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
            return Balance.CompareTo(other.Balance);
        }
        public virtual string ToSaveLine() =>
            $"{AccountNumber};{Type};{Balance.ToString(CultureInfo.InvariantCulture)}";
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
