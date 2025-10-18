// 代码生成时间: 2025-10-18 13:06:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// 定义贷款实体
public class Loan
{
    public int LoanId { get; set; }
    public string ApplicantName { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
}

// 定义贷款审批上下文
public class LoanApprovalContext : DbContext
{
    public LoanApprovalContext(DbContextOptions<LoanApprovalContext> options) : base(options)
    {
    }

    public DbSet<Loan> Loans { get; set; }
}

// 贷款审批服务
public class LoanApprovalService
{
    private readonly LoanApprovalContext _context;

    public LoanApprovalService(LoanApprovalContext context)
    {
        _context = context;
    }

    // 提交贷款申请
    public async Task<Loan> SubmitLoanAsync(string applicantName, decimal amount)
    {
        if (string.IsNullOrEmpty(applicantName))
        {
            throw new ArgumentException("Applicant name cannot be null or empty.");
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Loan amount must be greater than zero.");
        }

        var loan = new Loan { ApplicantName = applicantName, Amount = amount, Status = "Pending" };
        await _context.Loans.AddAsync(loan);
        await _context.SaveChangesAsync();

        return loan;
    }

    // 审批贷款
    public async Task<Loan> ApproveLoanAsync(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan == null)
        {
            throw new KeyNotFoundException("Loan not found.");
        }

        loan.Status = "Approved";
        await _context.SaveChangesAsync();

        return loan;
    }

    // 拒绝贷款
    public async Task<Loan> DenyLoanAsync(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan == null)
        {
            throw new KeyNotFoundException("Loan not found.");
        }

        loan.Status = "Denied";
        await _context.SaveChangesAsync();

        return loan;
    }
}

// 贷款审批程序入口
class Program
{
    static async Task Main(string[] args)
    {
        // 配置数据库上下文
        var optionsBuilder = new DbContextOptionsBuilder<LoanApprovalContext>();
        optionsBuilder.UseSqlServer("Your Connection String Here");

        var context = new LoanApprovalContext(optionsBuilder.Options);
        var service = new LoanApprovalService(context);

        try
        {
            // 提交贷款申请
            var loan = await service.SubmitLoanAsync("John Doe", 10000);
            Console.WriteLine($"Loan submitted with ID: {loan.LoanId}");

            // 审批贷款
            var approvedLoan = await service.ApproveLoanAsync(loan.LoanId);
            Console.WriteLine($"Loan {loan.LoanId} approved with status: {approvedLoan.Status}");

            // 拒绝贷款
            // var deniedLoan = await service.DenyLoanAsync(loan.LoanId);
            // Console.WriteLine($"Loan {loan.LoanId} denied with status: {deniedLoan.Status}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}