using System.Globalization;
using System.Text.Json.Serialization;

namespace C_Arrays
{
    public class SavingsAccount : BankAccount, IAuditable, IPrintable
    {
        private decimal interestRate;

        public decimal InterestRate
        {
            get => interestRate;
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Ставка должна быть от 0 до 1");
                interestRate = value;
            }
        }
        public SavingsAccount(decimal balance, decimal rate) : base(balance, AccountType.Savings)
        {
            InterestRate = rate;
        }

        [JsonConstructor]
        public SavingsAccount(string accountNumber, decimal balance, decimal interestRate)
            : base(accountNumber, balance, AccountType.Savings)
        {
            InterestRate = interestRate;
        }

        public void AddInterest()
        {
            Balance += Balance * InterestRate;
        }
        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Процентная ставка: {InterestRate:P}");
        }
        public override void ApplyMonthlyUpdate()
        {
            AddInterest();
        }
        public override string GetAuditInfo()
        {
            return $"Тип: сберегательный счёт, баланс: {Balance}, процентная ставка {InterestRate:P}";
        }
        public override string ToSaveLine() =>
            $"{base.ToSaveLine()};{InterestRate.ToString(CultureInfo.InvariantCulture)}";
        public override string GetPrintableInfo()
        {
            return $"Тип: сберегательный счёт, баланс: {Balance}, процентная ставка {InterestRate:P}";
        }
    }
}
