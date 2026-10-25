namespace C_Arrays
{
    public class SavingsAccount: BankAccount
    {
        private readonly decimal interestRate;

        public SavingsAccount(decimal startBalance, decimal interestRate) : base(startBalance)
        {
            this.interestRate = interestRate;
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
    }
}
