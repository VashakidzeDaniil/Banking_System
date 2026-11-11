using System.Text.Json.Serialization;

namespace BankingSystem
{
    public class CheckingAccount : BankAccount, IAuditable, IPrintable
    {
        public CheckingAccount(decimal balance) : base(balance, AccountType.Checking) { }
        
        [JsonConstructor]
        public CheckingAccount(string accountNumber, decimal balance)
    : base(accountNumber, balance, AccountType.Checking) { }

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
