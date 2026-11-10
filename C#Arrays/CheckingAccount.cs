namespace C_Arrays
{
    public class CheckingAccount : BankAccount, IAuditable, IPrintable
    {
        public CheckingAccount(decimal startBalance) : base(startBalance, AccountType.Checking) { }
        public CheckingAccount(string accountNumber, decimal startBalance)
    : base(accountNumber, startBalance, AccountType.Checking) { }

        public override void ApplyMonthlyUpdate()
        {
            // у расчётного счёта нет ежемесячных операций
        }
        public override string GetAuditInfo()
        {
            return $"Тип: расчетный счёт, баланс: {Balance}";
        }
        public override string GetPrintableInfo()
        {
            return $"Тип: расчетный счёт, баланс: {Balance}";
        }
    }
}
