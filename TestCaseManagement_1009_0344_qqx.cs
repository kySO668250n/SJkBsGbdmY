// 代码生成时间: 2025-10-09 03:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// 定义测试用例模型
public class TestCase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

// 定义数据库上下文
public class TestCaseContext : DbContext
{
    public DbSet<TestCase> TestCases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestCaseManagementDB;Integrated Security=True");
    }
}

public class TestCaseManager
{
    private readonly TestCaseContext _context;

    public TestCaseManager(TestCaseContext context)
    {
        _context = context;
    }

    // 添加测试用例
    public async Task AddTestCase(TestCase testCase)
    {
        try
        {
            await _context.TestCases.AddAsync(testCase);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // 错误处理
            throw new Exception("添加测试用例失败", ex);
        }
    }

    // 获取所有测试用例
    public async Task<List<TestCase>> GetAllTestCases()
    {
        try
        {
            return await _context.TestCases.ToListAsync();
        }
        catch (Exception ex)
        {
            // 错误处理
            throw new Exception("获取测试用例失败", ex);
        }
    }

    // 更新测试用例
    public async Task UpdateTestCase(TestCase testCase)
    {
        try
        {
            _context.TestCases.Update(testCase);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // 错误处理
            throw new Exception("更新测试用例失败", ex);
        }
    }

    // 删除测试用例
    public async Task DeleteTestCase(int id)
    {
        try
        {
            var testCase = await _context.TestCases.FindAsync(id);
            if (testCase != null)
            {
                _context.TestCases.Remove(testCase);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("测试用例不存在");
            }
        }
        catch (Exception ex)
        {
            // 错误处理
            throw new Exception("删除测试用例失败", ex);
        }
    }
}
