using System;
using CSBank.Model;
using System.Xml.Linq;

namespace CSBank.DAL
{
	public class UserAccountRepository : IUserAccountRepository
	{
        XDocument xmldoc = XDocument.Load("../../../UserAccounts.xml");
        public int AddUserAccount(UserAccount userAccount)
        {
            int id = 1;
            ulong account_number = 111111111111;

            if (xmldoc.Descendants("UserAccount").LastOrDefault() != null)
            {
                var accounts = xmldoc.Descendants("UserAccount").Reverse().Take(1);
                foreach (var account in accounts)
                {
                    id = Convert.ToInt32(account.Element("id").Value) + 1;
                    account_number = Convert.ToUInt64(account.Element("accountnumber").Value) + 1;
                }
            }

            XElement xmlUserAccount = new XElement("UserAccount",
            new XElement("id", id),
            new XElement("name", userAccount.Name),
            new XElement("accountbalance", 0),
            new XElement("accountnumber", account_number),
            new XElement("phonenumber", userAccount.PhoneNumber),
            new XElement("username", userAccount.UserName),
            new XElement("password", userAccount.Password),
            new XElement("Transactions"));

            xmldoc.Root.Add(xmlUserAccount);
            xmldoc.Save("../../../UserAccounts.xml");
            return id;
        }
        public void UpdateAccountBalanceAndTransaction(Transaction transaction, int id)
        {
            XElement xmlUserAccount = xmldoc.Descendants("UserAccount").FirstOrDefault(p => p.Element("id").Value == id.ToString());

            if (transaction.TransactionType == 'C')
                xmlUserAccount.Element("accountbalance").Value = (Convert.ToDecimal(xmlUserAccount.Element("accountbalance").Value) + transaction.TransactionAmount).ToString();
            else if (transaction.TransactionType == 'D')
                xmlUserAccount.Element("accountbalance").Value = (Convert.ToDecimal(xmlUserAccount.Element("accountbalance").Value) - transaction.TransactionAmount).ToString();


            XElement xmlTransaction = xmldoc.Descendants("UserAccount").FirstOrDefault(p => p.Element("id").Value == id.ToString())
                                     .Descendants("Transactions").FirstOrDefault();
            xmlTransaction.Add(new XElement("Transaction",
                new XElement("transactiontype", transaction.TransactionType),
                new XElement("transactionamount", transaction.TransactionAmount),
                new XElement("dateoftransaction", transaction.DateOfTransaction)));

            xmldoc.Save("../../../UserAccounts.xml");
        }
        public IEnumerable<XElement> GetAllTransactions(int id)
        {
            return xmldoc.Descendants("UserAccount").FirstOrDefault(p => p.Element("id").Value == id.ToString())
                                     .Descendants("Transactions").Descendants("Transaction");
        }
        public decimal GetAccountBalance(int id)
        {
            XElement xmlUserAccount = xmldoc.Descendants("UserAccount").FirstOrDefault(p => p.Element("id").Value == id.ToString());

            return Convert.ToDecimal(xmlUserAccount.Element("accountbalance").Value);
        }
        public int GetUserIdWithUserDetails(string userName, string password)
        {
            XElement xmlUserAccount = xmldoc.Descendants("UserAccount").FirstOrDefault(p => p.Element("username").Value == userName && p.Element("password").Value == password);
            if (xmlUserAccount != null)
                return Convert.ToInt32(xmlUserAccount.Element("id").Value);
            else
                return 0;
        }
    }
}

