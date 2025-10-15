// 代码生成时间: 2025-10-15 18:07:45
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// Define the namespace for the dashboard program.
namespace DashboardProgram
{
    // Define a data model class for the dashboard data.
    public class DashboardData
    {
        public int Id { get; set; }
        public string MetricName { get; set; }
        public double Value { get; set; }
    }

    // Define the DbContext for the dashboard data.
    public class DashboardContext : DbContext
    {
        public DbSet<DashboardData> DashboardDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string and provider.
            optionsBuilder.UseSqlServer("Server=(localdb)\mssqllocaldb;Database=DashboardDB;Trusted_Connection=True;");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a new instance of the dashboard context.
                using (var context = new DashboardContext())
                {
                    // Fetch data from the database.
                    var dashboardData = context.DashboardDatas.ToList();

                    // Display the dashboard data.
                    Console.WriteLine("Data Dashboard: ");
                    foreach (var data in dashboardData)
                    {
                        Console.WriteLine($"Metric: {data.MetricName}, Value: {data.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle any errors that occur.
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}