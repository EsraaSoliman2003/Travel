using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
namespace Project
{
    class BankAccount
    {
        public string AccountNumber { get; init; }
        public string AccountHolderName { get; set; }
        public decimal Balance { get; set; }
        public BankAccount(string type, string AccountHolderName, decimal Balance)
        {
            AccountNumber = type + (new Random().Next(9999999)).ToString();
            this.AccountHolderName = AccountHolderName;
            this.Balance = Balance;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount < 0)
            {
                Console.WriteLine("Error: Deposit amount must be non-negative.");
                return;
            }

            Balance += amount;
            Console.WriteLine($"Deposited E£{amount:F2} into account {AccountNumber}. New balance: E£{Balance:F2}");

        }
        public virtual void Withdraw(decimal amount)
        {
            if (amount < 0)
            {
                Console.WriteLine("Error: Withdrawal amount must be non-negative.");
                return;
            }

            if (amount > Balance)
            {
                Console.WriteLine($"Error: Insufficient funds in account {AccountNumber}.");
                return;
            }

            Balance -= amount;
            Console.WriteLine($"Withdrawn E£{amount:F2} from account {AccountNumber}. New balance: E£{Balance:F2}");
        }
        public virtual void CheckBalance()
        {
            Console.WriteLine($"Account Type: {GetType().Name}");
            Console.WriteLine($"AccountHolder: {AccountHolderName}");
            Console.WriteLine($"Account {AccountNumber} balance: E£{Balance:F2}");
        }

    }
    public interface IInterestEarning
    {
        decimal CalculateInterest();
        void ApplyInterest();
    }
    class SavingsAccount : BankAccount, IInterestEarning
    {
        public SavingsAccount(string accountHolderName, decimal balance) : base("SAV", accountHolderName, balance) { }
        public decimal CalculateInterest()
        {
            return Balance * 0.03m;
        }

        public void ApplyInterest()
        {
            decimal interest = CalculateInterest();
            Deposit(interest);
            Console.WriteLine($"Interest earned on savings account {AccountNumber}: E£{interest:F2}. New balance: E£{Balance:F2}");
        }
    }
    class CheckingAccount : BankAccount
    {
        public CheckingAccount(string accountHolderName, decimal balance) : base("CHA", accountHolderName, balance) { }
    }
    class InvestmentAccount : BankAccount, IInterestEarning
    {
        public InvestmentAccount(string accountHolderName, decimal balance) : base("INV", accountHolderName, balance) { }
        public decimal CalculateInterest()
        {
            return Balance * 0.05m;
        }

        public void ApplyInterest()
        {
            decimal interest = CalculateInterest();
            Deposit(interest / 2);
            Console.WriteLine($"Interest earned on investment account {AccountNumber}: E£{interest:F2}. New balance: E£{Balance:F2}");
        }
    }
    class CreditAccount : BankAccount
    {
        public decimal CreditLimit { get; set; }
        public CreditAccount(string accountHolderName, decimal balance, decimal creditLimit) : base("CRA", accountHolderName, balance)
        {
            CreditLimit = creditLimit;
        }
        public override void Withdraw(decimal amount)
        {
            if (amount < 0)
            {
                Console.WriteLine("Error: Withdrawal amount must be non-negative.");
                return;
            }
            if (amount > (Balance + CreditLimit))
            {
                Console.WriteLine($"Error: Withdrawal amount exceeds credit limit. Limit reached for account {AccountNumber}.");
                return;
            }

            if (amount <= Balance)
            {
                Balance -=amount;
                Console.WriteLine($"Withdrawn E£{amount:F2} from account {AccountNumber} using credit. Balance: E£{Balance:F2}, Credit: E£{CreditLimit:F2}");
            }
            else if(amount <= CreditLimit)
            {
                CreditLimit -=amount;
                Console.WriteLine($"Withdrawn E£{amount:F2} from account {AccountNumber} using credit. Balance: E£{Balance:F2}, Credit: E£{CreditLimit:F2}");
            }
            else
            {
                amount = amount - Balance;
                Balance = 0;
                CreditLimit -= amount;
                Console.WriteLine($"Withdrawn E£{amount:F2} from account {AccountNumber} using credit. Balance: E£{Balance:F2}, Credit: E£{CreditLimit:F2}");
                return;
            }
        }

    }
    public static class InterestExtensions
    {
        public static decimal CalculateTotalInterest(this IInterestEarning account, DateTime startDate, DateTime endDate)
        {
            int months = (int)(endDate.Subtract(startDate).Days / (365.25 / 12));
            return account.CalculateInterest() * months;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<BankAccount> accounts = new List<BankAccount>();

            // Create SavingsAccount
            Console.WriteLine("Enter details for Savings Account (HolderName, Balance):");
            SavingsAccount savingsAccount = new SavingsAccount(Console.ReadLine(), decimal.Parse(Console.ReadLine()));
            accounts.Add(savingsAccount);

            // Create CheckingAccount
            Console.WriteLine("Enter details for Checking Account (HolderName, Balance):");
            CheckingAccount checkingAccount = new CheckingAccount(Console.ReadLine(), decimal.Parse(Console.ReadLine()));
            accounts.Add(checkingAccount);

            // Create InvestmentAccount
            Console.WriteLine("Enter details for Investment Account (HolderName, Balance):");
            InvestmentAccount investmentAccount = new InvestmentAccount(Console.ReadLine(), decimal.Parse(Console.ReadLine()));
            accounts.Add(investmentAccount);

            // Create CreditAccount
            Console.WriteLine("Enter details for Credit Account (HolderName, Balance, Credit Limit):");
            CreditAccount creditAccount = new CreditAccount(Console.ReadLine(), decimal.Parse(Console.ReadLine()), decimal.Parse(Console.ReadLine()));
            accounts.Add(creditAccount);

            //Testing
            foreach (var account in accounts)
            {
                try
                {
                    account.CheckBalance();
                    Console.WriteLine($"Please enter amount deposited:");
                    account.Deposit(decimal.Parse(Console.ReadLine()));
                    Console.WriteLine($"Please enter amount withdrawn:");
                    account.Withdraw(decimal.Parse(Console.ReadLine()));

                    if (account is IInterestEarning interestAccount)
                    {
                        Console.WriteLine("Calculating interest between two dates");
                        Console.WriteLine("Please insert start date: (dd/MM/yyyy)");
                        DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        Console.WriteLine("Please insert end date: (dd/MM/yyyy)");
                        DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        decimal totalInterest = interestAccount.CalculateTotalInterest(startDate, endDate);
                        Console.WriteLine($"Total interest earned in {Math.Abs((endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month)} months: E£{totalInterest:F2}\n");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}