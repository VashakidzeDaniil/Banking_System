using Xunit;
using BankingSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Tests
{
    public class BankTests
    {
        // Хелпер: создает чистый банк с временной базой данных в памяти
        private Bank CreateBankWithTestDb()
        {
            // 1. Создаем подключение в памяти
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open(); // Важно держать открытым, иначе база удалится

            // 2. Настраиваем DbContext на использование этого подключения
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseSqlite(connection)
                .Options;

            // 3. Создаем саму базу и таблицы
            var context = new BankDbContext(options);
            context.Database.EnsureCreated();

            // 4. Возвращаем Bank, передав ему наш тестовый контекст
            return new Bank(context);
        }

        [Fact]
        public void Transfer_PositiveAmount_MovesMoney()
        {
            // Arrange
            var bank = CreateBankWithTestDb(); // Используем хелпер!
            var acc1 = new CheckingAccount(1000m);
            var acc2 = new CheckingAccount(500m);
            bank.Add(acc1);
            bank.Add(acc2);

            // Act
            bool ok = bank.Transfer(acc1.AccountNumber, acc2.AccountNumber, 200m);

            // Assert
            Assert.True(ok);
            Assert.Equal(800m, bank.FindAccount(acc1.AccountNumber)!.Balance);
            Assert.Equal(700m, bank.FindAccount(acc2.AccountNumber)!.Balance);
        }

        // Точно так же перепиши остальные 4 теста (T2, T3, T4, T5), 
        // используя `var bank = CreateBankWithTestDb();` в самом начале Arrange.
        [Fact]
        public void Transfer_AmountGreaterThanBalance_ReturnsFalse()
        {
            var bank = CreateBankWithTestDb();
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
            var bank = CreateBankWithTestDb();
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
            var bank = CreateBankWithTestDb();
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
            var bank = CreateBankWithTestDb();
            bank.Add(new CheckingAccount(1000m));

            // Act
            decimal result = bank.GetTotalBalanceOfTypes(Array.Empty<AccountType>());

            // Assert
            Assert.Equal(0m, result);
        }
    }
}
