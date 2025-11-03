// 代码生成时间: 2025-11-03 10:32:58
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// 定义实体类
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTime LastUpdated { get; set; }
}

// 定义数据库上下文
public class PriceMonitoringContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("your_connection_string");
    }
}

// 定义服务类
public class PriceMonitoringService
{
    private readonly PriceMonitoringContext _context;

    public PriceMonitoringService(PriceMonitoringContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // 获取所有产品价格
    public List<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    // 更新产品价格
    public Product UpdateProductPrice(int productId, decimal newPrice)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }

        product.Price = newPrice;
        product.LastUpdated = DateTime.Now;
        _context.SaveChanges();
        return product;
    }
}

// 定义程序入口
class Program
{
    static void Main(string[] args)
    {
        using (var context = new PriceMonitoringContext())
        {
            // 确保数据库迁移已应用
            context.Database.Migrate();

            // 创建服务实例
            var service = new PriceMonitoringService(context);

            try
            {
                // 获取所有产品价格
                var products = service.GetAllProducts();
                foreach (var product in products)
                {
                    Console.WriteLine($"Product: {product.Name}, Price: {product.Price}, Last Updated: {product.LastUpdated}");
                }

                // 更新产品价格
                var updatedProduct = service.UpdateProductPrice(1, 99.99m);
                Console.WriteLine($"Updated Product: {updatedProduct.Name}, New Price: {updatedProduct.Price}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}