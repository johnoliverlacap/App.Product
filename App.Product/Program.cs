using ConsoleTableExt;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Product
{
    public class Program
    {
        static void Main(string[] args)
        {
            string transactAgain = string.Empty;

            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Product Management");               
                Console.WriteLine("[A] - Add New Product");
                Console.WriteLine("[B] - Display All");
                Console.WriteLine("[C] - Delete Product");
                Console.WriteLine("[D] - Update Product");
                Console.WriteLine("[E] - Sort Product");
                Console.WriteLine("[F] - Search");
                Console.WriteLine("[G] - Get by Color");
                Console.Write("Please select transaction: ");
                string selection = Console.ReadLine();

                if (selection.Equals("A"))
                    Add();

                if (selection.Equals("B"))
                    Display();

                if (selection.Equals("C"))
                    Delete();

                if (selection.Equals("D"))
                    Update();

                if (selection.Equals("E"))
                    Sort();

                if (selection.Equals("F"))
                    Search();

                if (selection.Equals("G"))
                    GetByColor();

                Console.Write("Perform another task? ");
                transactAgain = Console.ReadLine();

            } while (transactAgain.Equals("Yes"));
        }

        public static void Add()
        {
            Display();
            Product product = new Product();
            Console.Write("Name: ");
            product.Name = Console.ReadLine();
            Console.Write("Color: ");
            product.Color = Console.ReadLine();
            Console.Write("Brand: ");
            product.Brand = Console.ReadLine();
            Console.Write("Quantity: ");
            product.Quantity = Convert.ToInt32(Console.ReadLine());
            Console.Write("Price: ");
            product.Price = Convert.ToDecimal(Console.ReadLine());

            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string query = "INSERT INTO dbo.Product (Name, Color, Brand, Quantity, Price) VALUES (@Name, @Color, @Brand, @Quantity, @Price)";

                sqlConnection.Execute(query, product);

                Console.WriteLine("Product added");
            }
        }

        public static void Display()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                IEnumerable<Product> products = sqlConnection.Query<Product>("select * from dbo.product");

                ConsoleTableBuilder
                    .From(products.ToList())
                    .ExportAndWriteLine();
            }
        }

        public static void Delete()
        {
            Display();

            Console.Write("Select Id to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string query = "DELETE FROM dbo.Product WHERE Id = @id";

                sqlConnection.Execute(query, new { id = id });
                Console.WriteLine("Product deleted");
            }
        }

        public static void Update()
        {
            Display();
            Product product = new Product();
            Console.Write("Id: ");
            product.Id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Name: ");
            product.Name = Console.ReadLine();
            Console.Write("Color: ");
            product.Color = Console.ReadLine();
            Console.Write("Brand: ");
            product.Brand = Console.ReadLine();
            Console.Write("Quantity: ");
            product.Quantity = Convert.ToInt32(Console.ReadLine());
            Console.Write("Price: ");
            product.Price = Convert.ToDecimal(Console.ReadLine());

            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string query = "UPDATE dbo.Product set Name=@Name, Color=@Color, Brand=@Brand, Quantity=@Quantity, Price=@Price WHERE Id=@Id";

                sqlConnection.Execute(query, product);

                Console.WriteLine("Product updated!");

            }
        }

        public static void Sort()
        {
            Display();
            Console.Write("Enter column name: ");
            string column = Console.ReadLine();
            Console.Write("Enter order type (asc or desc): ");
            string orderType = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = string.Format("select * from dbo.product order by {0} {1}", column, orderType);

                sqlConnection.Open();
                IEnumerable<Product> products = sqlConnection.Query<Product>(query);

                ConsoleTableBuilder
                    .From(products.ToList())
                    .ExportAndWriteLine();
            }
        }

        public static void Search()
        {
            Console.Write("Enter search key: ");
            string key = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = string.Format("select * from dbo.product where name like '%{0}%'", key);

                sqlConnection.Open();
                IEnumerable<Product> products = sqlConnection.Query<Product>(query);

                ConsoleTableBuilder
                    .From(products.ToList())
                    .ExportAndWriteLine();
            }
        }

        public static void GetByColor()
        {
            Console.Write("Enter color: ");
            string color = Console.ReadLine();

            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = string.Format("select * from dbo.product where color='{0}'", color);

                sqlConnection.Open();
                IEnumerable<Product> products = sqlConnection.Query<Product>(query);

                ConsoleTableBuilder
                    .From(products.ToList())
                    .ExportAndWriteLine();
            }
        }
    }
}
