namespace BankingSystem.Tests
{
    public class BankTests
    {
        [Fact]
        public void Transfer_positiveAmount_MovesMoney()
        {
            var bank = new Bank();

            var acc1 = new CheckingAccount(1000m);
            var acc2 = new CheckingAccount(500m);

            bank.Add(acc1);
            bank.Add(acc2);
            bool ok = bank.Transfer(acc1.AccountNumber, acc2.AccountNumber, 200m);

            Assert.True(ok);

            Assert.Equal(800m, bank.FindAccount(acc1.AccountNumber)!.Balance);
            Assert.Equal(700m, bank.FindAccount(acc2.AccountNumber)!.Balance);
        }
        [Fact]
        public void Transfer_AmountGreaterThanBalance_ReturnsFalse()
        {
            var bank = new Bank();
            var acc1 = new CheckingAccount(100m);
            var acc2 = new CheckingAccount(500m);
            bank.Add(acc1);
            bank.Add(acc2);
            bool ok = bank.Transfer(acc1.AccountNumber, acc2.AccountNumber, 200m);

            Assert.False(ok);
            Assert.Equal(100m, bank.FindAccount(acc1.AccountNumber)!.Balance);
            Assert.Equal(500m, bank.FindAccount(acc2.AccountNumber)!.Balance);
        }
        [Fact]
        public void Transfer_NonExistentAccount_ThrowsAccountNotFoundException()
        {
            // Arrange
            var bank = new Bank();
            var acc1 = new CheckingAccount(1000m);
            bank.Add(acc1);

            // Act & Assert
            // Проверяем, что вызов бросит именно AccountNotFoundException
            Assert.Throws<AccountNotFoundException>(() =>
                bank.Transfer("НЕСУЩЕСТВУЮЩИЙ_СЧЕТ", acc1.AccountNumber, 100m));
        }
        [Fact]
        public void Transfer_NegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            var bank = new Bank();
            var acc1 = new CheckingAccount(1000m);
            var acc2 = new CheckingAccount(1000m);
            bank.Add(acc1);
            bank.Add(acc2);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                bank.Transfer(acc1.AccountNumber, acc2.AccountNumber, -50m));
        }
        [Fact]
        public void GetTotalBalanceOfTypes_EmptyTypes_ReturnsZero()
        {
            // Arrange
            var bank = new Bank();
            bank.Add(new CheckingAccount(1000m));

            // Act
            decimal result = bank.GetTotalBalanceOfTypes(Array.Empty<AccountType>());

            // Assert
            Assert.Equal(0m, result);
        }
    }
}
