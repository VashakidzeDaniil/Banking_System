using System;
using System.Collections.Generic;
using System.Text;

namespace C_Arrays
{
    public class AccountNotFoundException : BankException
    {
        public AccountNotFoundException(string accountNumber)
            : base($"Счёт {accountNumber} не найден")
        {
            AccountNumber = accountNumber;
        }
        public string AccountNumber { get; }
    }
}
