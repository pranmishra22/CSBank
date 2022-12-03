
CS Bank application - 
This console application handles following operations -
 -- User can create a new account
 -- If an user have an account they can perform below operations on his/her account
    -- View balance
    -- Deposit money
    -- Withdraw money
    -- View transactions
This application stores/retrieves data from xml file using System.xml.Linq.

Design decisions -
This project is designed with the concept of Repository design pattern.
There is a DAL(Data Acess Layer) folder which contains logic for adding and updating data from/into xml file.
Like MVC concept, we've a Model folder here where all the properties are defined.
Validations of user input has been taken care by using different validators (e.g. string.All(char.IsNumber))
To prevent the application from exiting after each operation and giving the user a good experience, its uses condition to verify if the user wants to do any other operation or they want to exit.

Note -
In the xml file, one test user account is already there with 7 transactions.