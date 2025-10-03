// 代码生成时间: 2025-10-03 22:16:30
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// 定义财务管理模块
namespace FinancialManagement
{
    // 定义财务实体类
    public class FinancialRecord
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    // 定义财务管理服务
    public class FinancialManagementService
    {
        private readonly FinancialDbContext _context;

        public FinancialManagementService(FinancialDbContext context)
        {
            _context = context;
        }

        // 添加财务记录
        public async Task AddRecordAsync(FinancialRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            await _context.FinancialRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        // 获取所有财务记录
        public async Task<List<FinancialRecord>> GetAllRecordsAsync()
        {
            return await _context.FinancialRecords.ToListAsync();
        }

        // 获取特定日期的财务记录
        public async Task<List<FinancialRecord>> GetRecordsByDateAsync(DateTime date)
        {
            return await _context.FinancialRecords
                .Where(r => r.Date == date)
                .ToListAsync();
        }
    }

    // 定义数据库上下文
    public class FinancialDbContext : DbContext
    {
        public FinancialDbContext(DbContextOptions<FinancialDbContext> options)
            : base(options)
        {
        }

        public DbSet<FinancialRecord> FinancialRecords { get; set; }
    }

    // 定义数据库配置
    public static class FinancialDbConfiguration
    {
        public static void Register(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\mssqllocaldb;Database=FinancialDb;Trusted_Connection=True;");
        }
    }
}
