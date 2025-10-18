// 代码生成时间: 2025-10-19 05:41:54
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

// 定义任务分配系统的数据模型
public class Task {
    public int TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AssigneeId { get; set; }
    public User Assignee { get; set; }
}

public class User {
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<Task> Tasks { get; set; }
}

// 数据库上下文
public class TaskContext : DbContext {
    public DbSet<Task> Tasks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer("YourConnectionStringHere");
    }
}

// 任务分配系统服务
public class TaskAllocationService {
    private readonly TaskContext _context;

    public TaskAllocationService(TaskContext context) {
        _context = context;
    }

    // 分配任务给用户
    public void AssignTask(int taskId, int assigneeId) {
        var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
        if (task == null) {
            throw new Exception("Task not found.");
        }

        var user = _context.Users.FirstOrDefault(u => u.UserId == assigneeId);
        if (user == null) {
            throw new Exception("User not found.");
        }

        task.AssigneeId = assigneeId;
        _context.SaveChanges();
    }

    // 获取用户的任务列表
    public IEnumerable<Task> GetTasksByUser(int userId) {
        return _context.Tasks
            .Where(t => t.AssigneeId == userId)
            .Include(t => t.Assignee)
            .ToList();
    }
}

// 程序入口点
class Program {
    static void Main(string[] args) {
        // 创建数据库上下文实例
        using (var context = new TaskContext()) {
            // 确保数据库已创建
            context.Database.EnsureCreated();

            // 创建任务分配服务实例
            var service = new TaskAllocationService(context);

            try {
                // 分配任务示例
                service.AssignTask(1, 1);

                // 获取任务列表示例
                var tasks = service.GetTasksByUser(1);
                foreach (var task in tasks) {
                    Console.WriteLine(\$"Task: {task.Title}, Assigned to: {task.Assignee.Name}");
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}