namespace C_Arrays
{
    public class Employee : IAuditable, IPrintable
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }

        public Employee(string fullName, string position, decimal salary)
        {
            this.FullName = fullName;
            this.Position = position;
            this.Salary = salary;
        }
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
