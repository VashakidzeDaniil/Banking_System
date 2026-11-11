namespace BankingSystem
{
    public record Employee(string FullName, string Position, decimal Salary):IAuditable,IPrintable
    {
        public string GetAuditInfo()
        {
            return $"Имя сотрудника: {FullName}\n" +
                   $"Позиция сотрудника: {Position}\n" +
                   $"Зарплата сотрудника: {Salary}";
        }
        public string GetPrintableInfo()
        {
            return $"Имя сотрудника: {FullName}\n" +
                   $"Позиция сотрудника: {Position}\n";
        }
    }
}
