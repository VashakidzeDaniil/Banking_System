using System.Globalization;

namespace C_Arrays
{
    public class SavingsAccount : BankAccount, IAuditable, IPrintable
    {
        private readonly decimal interestRate;
        public decimal GetRate() => interestRate;
        public SavingsAccount(decimal startBalance, decimal rate) : base(startBalance, AccountType.Savings)
        {
            interestRate = rate;
        }
        public SavingsAccount(string accountNumber, decimal startBalance, decimal rate)
            : base(accountNumber, startBalance, AccountType.Savings)
        {
            interestRate = rate;
        }

        public void AddInterest()
        {
            balance += balance * interestRate;
        }
        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Процентная ставка: {interestRate:P}");
        }
        public override void ApplyMonthlyUpdate()
        {
            AddInterest();
        }
        public override string GetAuditInfo()
        {
            return $"Тип: сберегательный счёт, баланс: {balance}, процентная ставка {interestRate:P}";
        }
        public override string ToSaveLine() =>
            $"{base.ToSaveLine()};{interestRate.ToString(CultureInfo.InvariantCulture)}";
        public override string GetPrintableInfo()
        {
            return $"Тип: сберегательный счёт, баланс: {balance}, процентная ставка {interestRate:P}";
        }
    }
}
