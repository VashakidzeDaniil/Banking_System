namespace C_Arrays
{
    public class CheckingAccount : BankAccount , IAuditable
    {
        public CheckingAccount(decimal startBalance, string accountNumber = null) : base(startBalance, accountNumber) { }

        public override void ApplyMonthlyUpdate()
        {
            // у расчётного счёта нет ежемесячных операций
        }
        public string GetAuditInfo()
        {
            return $"Тип: расчетный счёт, баланс: {balance}";
        }
    }
}
