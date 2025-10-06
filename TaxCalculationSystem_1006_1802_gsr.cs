// 代码生成时间: 2025-10-06 18:02:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// 定义税务计算系统的实体类
public class TaxEntity
{
    public int Id { get; set; }
    public double Income { get; set; }
    public double TaxRate { get; set; }
    public double TaxAmount { get; set; }
}

// 定义税务计算系统的数据库上下文
public class TaxDbContext : DbContext
{
    public DbSet<TaxEntity> Taxes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 设置数据库连接字符串
        optionsBuilder.UseSqlServer("Server=(localdb)\mssqllocaldb;Database=TaxDb;Trusted_Connection=True;");
    }
}

// 税务计算系统服务
public class TaxService
{
    private readonly TaxDbContext _context;

    public TaxService(TaxDbContext context)
    {
        _context = context;
    }

    // 计算税务金额
    public double CalculateTax(double income, double taxRate)
    {
        if (income < 0 || taxRate < 0 || taxRate > 1)
        {
            throw new ArgumentException("Income and tax rate must be non-negative and tax rate must be between 0 and 1.");
        }

        return income * taxRate;
    }

    // 添加税务记录
    public async Task<int> AddTaxRecordAsync(double income, double taxRate)
    {
        var taxAmount = CalculateTax(income, taxRate);
        var taxRecord = new TaxEntity { Income = income, TaxRate = taxRate, TaxAmount = taxAmount };

        await _context.Taxes.AddAsync(taxRecord);
        await _context.SaveChangesAsync();

        return taxRecord.Id;
    }
}

// 税务计算系统的主程序类
public class Program
{
    public static async Task Main(string[] args)
    {
        // 创建数据库上下文
        var context = new TaxDbContext();
        var taxService = new TaxService(context);

        try
        {
            // 添加税务记录
            double income = 50000;
            double taxRate = 0.2; // 20%
            var taxRecordId = await taxService.AddTaxRecordAsync(income, taxRate);
            Console.WriteLine($"Tax record added with ID: {taxRecordId}");
        }
        catch (Exception ex)
        {
            // 错误处理
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}