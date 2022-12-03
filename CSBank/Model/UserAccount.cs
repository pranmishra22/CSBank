using System;
namespace CSBank.Model
{
	public class UserAccount
	{
        public UserAccount()
        {
            UserTransactions = new List<Transaction>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal AccountBalance { get; set; }
        public ulong AccountNumber { get; set; }
        public ulong PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Transaction> UserTransactions { get; set; }
    }
}

