using System;
using CSBank.Model;
using System.Xml.Linq;

namespace CSBank.DAL
{
	public interface IUserAccountRepository
	{
        int AddUserAccount(UserAccount userAccount);
        void UpdateAccountBalanceAndTransaction(Transaction transaction, int id);
        IEnumerable<XElement> GetAllTransactions(int id);
        decimal GetAccountBalance(int id);
        int GetUserIdWithUserDetails(string userName, string password);
    }
}

