using System;
namespace CSBank.Model
{
	public class Transaction
	{
        public char TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}

