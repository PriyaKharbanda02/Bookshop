using System;
using System.Collections.Generic;
using System.Data.SqlClient;

struct Book
{
    public string Title;
    public string Author;
    public int Quantity;
    public double Price;
}

class Program
{
    // Update with your database connection details
    static string connectionString = @"Server = (localdb)\MSSQLLocalDB;Database=Bookshop1;Integrated Security= True";

    static void AddBook()
    {
        Book newBook = new Book();
        Console.Write("\nEnter book title: ");
        newBook.Title = Console.ReadLine();
        Console.Write("Enter author name: ");
        newBook.Author = Console.ReadLine();
        Console.Write("Enter quantity: ");
        newBook.Quantity = int.Parse(Console.ReadLine());
        Console.Write("Enter price: ");
        newBook.Price = double.Parse(Console.ReadLine());

        // SQL command to insert into database
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Books (Title, Author, Quantity, Price) VALUES (@Title, @Author, @Quantity, @Price)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", newBook.Title);
            command.Parameters.AddWithValue("@Author", newBook.Author);
            command.Parameters.AddWithValue("@Quantity", newBook.Quantity);
            command.Parameters.AddWithValue("@Price", newBook.Price);

            connection.Open();
            command.ExecuteNonQuery();
        }

        Console.WriteLine("\nBook added successfully!\n");
    }

    static void DisplayBooks()
    {
        Console.WriteLine("\nInventory:");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, Title, Author, Quantity, Price FROM Books";
            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Id"]}, Title: {reader["Title"]}, Author: {reader["Author"]}, Quantity: {reader["Quantity"]}, Price: ${reader["Price"]}");
            }
        }
    }

    static void SearchBook(string title)
    {
        bool found = false;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, Title, Author, Quantity, Price FROM Books WHERE Title = @Title";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", title);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("\nBook found:");
                Console.WriteLine($"ID: {reader["Id"]}, Title: {reader["Title"]}, Author: {reader["Author"]}, Quantity: {reader["Quantity"]}, Price: ${reader["Price"]}");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("\nBook not found.");
        }
    }

    static void Main()
    {
        int choice;
        string searchTitle;

        do
        {
            Console.WriteLine("\n---------------------------------------Bookshop Management System---------------------------------");
            Console.WriteLine("1. Add a book");
            Console.WriteLine("2. Display all books");
            Console.WriteLine("3. Search for a book by title");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddBook();
                    break;
                case 2:
                    DisplayBooks();
                    break;
                case 3:
                    Console.Write("Enter book title to search: ");
                    searchTitle = Console.ReadLine();
                    SearchBook(searchTitle);
                    break;
                case 4:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number from 1 to 4.");
                    break;
            }
        } while (choice != 4);
    }
}
