namespace C_Arrays
{
    public class SavingsAccount: BankAccount , IAuditable
    {
        private readonly decimal interestRate;
        public SavingsAccount(decimal startBalance, decimal rate, string accountNumber = null)
    : base(startBalance, accountNumber)
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
        public string GetAuditInfo()
        {
            return $"Тип: сберегательный счёт, баланс: {balance}, процентная ставка {interestRate:P}";
        }
    }
}
