using System;
using CSBank.DAL;
using CSBank.Model;
using System.Xml.Linq;
using System.Text;

public class Program
{
    private IUserAccountRepository _iUserAccountRepository;
    public Program()
    {
        this._iUserAccountRepository = new UserAccountRepository();
    }

    public static void Main()
    {

        Program program = new Program();
        program.Operation();
    }

    public void Operation()
    {
        Console.WriteLine("Hello, Welcome to CS Bank!");
        Console.WriteLine("Please choose one option from below - ");

        bool flag = true;

        do
        {
            Console.WriteLine(" - Press 0 to create a new User account");
            Console.WriteLine(" - Press 1 to proceed with existing account");
            string input = Console.ReadLine();
            if (input.Trim().Equals("0"))
            {
                NewAccountCreation();
                flag = false;
            }
            else if (input.Trim().Equals("1"))
            {
                Profile();
                flag = false;
            }
            else
                Console.WriteLine("Please Enter valid input!");

        } while (flag);
    }

    public void NewAccountCreation()
    {
        StringBuilder errorMsg = new StringBuilder();

        UserAccount us = new UserAccount();

        Console.WriteLine("Please Enter your Name!");
        string name = Console.ReadLine();
        if (name != null && name.Trim().Length != 0 && name.Trim().Length <= 50 && name.All(char.IsLetter))
            us.Name = name;
        else
            errorMsg.Append(" - Name mustbe alphabet with maximum 50 characters. \n");

        Console.WriteLine("Please Enter your phone number!");
        string phone_number = Console.ReadLine();
        if (phone_number != null && phone_number.Trim().Length == 10 && phone_number.All(char.IsNumber))
            us.PhoneNumber = Convert.ToUInt64(phone_number);
        else
            errorMsg.Append(" - Please enter a valid phone number of 10 digit. \n");

        Console.WriteLine("Please Enter your username!");
        string user_name = Console.ReadLine();
        if (user_name != null && user_name.Trim().Length != 0 && user_name.Trim().Length <= 50 && user_name.All(char.IsLetterOrDigit))
            us.UserName = user_name;
        else
            errorMsg.Append(" - Please enter a valid username with maximum 50 characters. Username can be alhanumeric only. \n");

        Console.WriteLine("Please Enter your password!");
        string password = Console.ReadLine();
        if (password != null && password.Trim().Length != 0 && password.Trim().Length <= 50)
            us.Password = password;
        else
            errorMsg.Append(" - Please enter a valid password with maximum 50 characters.\n");

        if (errorMsg.Length != 0)
        {
            Console.WriteLine("Please correct below errors!");
            Console.WriteLine(errorMsg);
            NewAccountCreation();
        }
        else
        {
            int id = _iUserAccountRepository.AddUserAccount(us);
            Console.WriteLine("Your account has been created!");
            BankingOperation(id);
        }
    }

    public void BankingOperation(int id)
    {
        bool continue_operation = true;
        do
        {
            Console.WriteLine("Hi! Which operation do you want to use.");
            Console.WriteLine(" - Press 1 to view account balance");
            Console.WriteLine(" - Press 2 to do a deposit");
            Console.WriteLine(" - Press 3 for withdrawal");
            Console.WriteLine(" - Press 4 to view transactions");
            Transaction transaction = new Transaction();
            switch (Console.ReadLine().Trim())
            {
                case "1":
                    decimal account_balance = _iUserAccountRepository.GetAccountBalance(id);
                    Console.WriteLine("Your account balance is :{0}", account_balance);
                    break;
                case "2":
                    bool deposit_amount_check = true;
                    transaction.TransactionType = 'C';
                    do
                    {
                        Console.WriteLine("Please enter the deposit amount in gbp");
                        string deposit_amount = Console.ReadLine();
                        decimal number = 0M;
                        if (decimal.TryParse(deposit_amount, out number) && Convert.ToDecimal(deposit_amount) <= 50000)
                        {
                            transaction.TransactionAmount = Convert.ToDecimal(deposit_amount);
                            transaction.DateOfTransaction = DateTime.Now;
                            _iUserAccountRepository.UpdateAccountBalanceAndTransaction(transaction, id);
                            Console.WriteLine("Amount deposited successfully!");
                            deposit_amount_check = false;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid amount to be deposited and amount should not exceed 50,000 gbp!");
                            Console.WriteLine("If you wish to exit press 0. Press any other key to continue.");
                            if (Console.ReadLine().Equals("0"))
                                deposit_amount_check = false;
                        }
                    } while (deposit_amount_check);
                    break;
                case "3":
                    bool withdraw_amount_check = true;
                    transaction.TransactionType = 'D';
                    decimal balance_remaining = _iUserAccountRepository.GetAccountBalance(id);
                    do
                    {
                        Console.WriteLine("Please enter the withdrawl amount in gbp");
                        string withdraw_amount = Console.ReadLine();
                        decimal number = 0M;
                        if (decimal.TryParse(withdraw_amount, out number) && Convert.ToDecimal(withdraw_amount) < balance_remaining)
                        {
                            transaction.TransactionAmount = Convert.ToDecimal(withdraw_amount);
                            transaction.DateOfTransaction = DateTime.Now;
                            _iUserAccountRepository.UpdateAccountBalanceAndTransaction(transaction, id);
                            Console.WriteLine("Amount withdrawn successfully!");
                            withdraw_amount_check = false;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid amount to be withdrawn and amount should not exceed your account balance!");
                            Console.WriteLine("If you wish to exit press 0. Press any other key to continue.");
                            if (Console.ReadLine().Equals("0"))
                                withdraw_amount_check = false;
                        }
                    } while (withdraw_amount_check);
                    break;
                case "4":
                    IEnumerable<XElement> transactions = _iUserAccountRepository.GetAllTransactions(id);
                    if (transactions.Any())
                    {
                        foreach (var trans in transactions)
                        {
                            char tran_type = Convert.ToChar(trans.Element("transactiontype").Value);
                            if (tran_type == 'C')
                                Console.WriteLine("{0} gbp deposited on {1}", trans.Element("transactionamount").Value, trans.Element("dateoftransaction").Value);
                            else if (tran_type == 'D')
                                Console.WriteLine("{0} gbp withdrawn on {1}", trans.Element("transactionamount").Value, trans.Element("dateoftransaction").Value);
                        }
                    }
                    else
                        Console.WriteLine("You have not done any transactions yet!");
                    
                    break;
                default:
                    Console.WriteLine("Please Enter a valid input!");
                    break;
            }

            Console.WriteLine("Do you wish to continue with other operation!");
            Console.WriteLine(" - Press 1 to continue.");
            Console.WriteLine(" - Any other key to exit.");
            if (Console.ReadLine().Trim() != "1")
                continue_operation = false;
        } while (continue_operation);

    }

    public void Profile()
    {
        Console.WriteLine("Please enter your UserName!");
        string username = Console.ReadLine();
        Console.WriteLine("Please enter your Password!");
        string password = Console.ReadLine();
        int id = _iUserAccountRepository.GetUserIdWithUserDetails(username, password);

        if (id != 0)
            BankingOperation(id);
        else
        {
            Console.WriteLine("You've entered incorrect username/password");
            Console.WriteLine(" - Press 1 to go back to the main menu!");
            Console.WriteLine(" - Press 2 to go back to the previous menu!");
            Console.WriteLine(" - Any other key to exit.");
            string option = Console.ReadLine();
            if (option.Trim().Equals("1"))
                Operation();
            else if (option.Trim().Equals("2"))
                Profile();
        }
    }
}

// See https://aka.ms/new-console-template for more information

