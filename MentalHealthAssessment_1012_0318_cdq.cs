// 代码生成时间: 2025-10-12 03:18:23
using System;
using System.Linq;
using System.Data.Entity;

// 心理健康评估实体类
public class MentalHealthAssessment
{
    public int Id { get; set; }
    public string PatientName { get; set; }
    public DateTime AssessmentDate { get; set; }
    public int AnxietyScore { get; set; }
    public int DepressionScore { get; set; }
    public string Notes { get; set; }
}

// 数据库上下文
public class MentalHealthDbContext : DbContext
{
    public DbSet<MentalHealthAssessment> Assessments { get; set; }

    public MentalHealthDbContext() : base("MentalHealthConnection")
    {
    }
}

// 心理健康评估服务
public class MentalHealthService
{
    private readonly MentalHealthDbContext _context;

    public MentalHealthService(MentalHealthDbContext context)
    {
        _context = context;
    }

    // 新建心理健康评估
    public void CreateAssessment(MentalHealthAssessment assessment)
    {
        if (assessment == null)
        {
            throw new ArgumentNullException(nameof(assessment), "Assessment cannot be null");
        }

        try
        {
            _context.Assessments.Add(assessment);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // 异常处理
            Console.WriteLine($"Error occurred: {ex.Message}");
            throw;
        }
    }

    // 获取所有心理健康评估
    public IQueryable<MentalHealthAssessment> GetAllAssessments()
    {
        return _context.Assessments;
    }

    // 获取单个心理健康评估
    public MentalHealthAssessment GetAssessmentById(int id)
    {
        return _context.Assessments.FirstOrDefault(a => a.Id == id);
    }
}

// 程序入口点
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // 创建数据库上下文
            using (var context = new MentalHealthDbContext())
            {
                // 创建服务实例
                var service = new MentalHealthService(context);

                // 创建新的心理健康评估
                var newAssessment = new MentalHealthAssessment
                {
                    PatientName = "John Doe",
                    AssessmentDate = DateTime.Now,
                    AnxietyScore = 50,
                    DepressionScore = 30,
                    Notes = "Patient appears anxious and depressed."
                };

                // 添加评估
                service.CreateAssessment(newAssessment);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}