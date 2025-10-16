// 代码生成时间: 2025-10-17 02:22:28
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

// 角色动画系统的实体类
public class Animation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

// 角色动画系统的数据库上下文
public class AnimationContext : DbContext
{
    public DbSet<Animation> Animations { get; set; }

    public AnimationContext(DbContextOptions<AnimationContext> options) : base(options)
    {
    }
}

// 角色动画系统的服务
public class AnimationService
{
    private readonly AnimationContext _context;

    public AnimationService(AnimationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // 添加动画
    public Animation AddAnimation(string name, string description)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Name and description cannot be null or empty.");
        }

        var animation = new Animation
        {
            Name = name,
            Description = description,
        };
        _context.Add(animation);
        _context.SaveChanges();
        return animation;
    }

    // 获取所有动画
    public IEnumerable<Animation> GetAllAnimations()
    {
        return _context.Animations.ToList();
    }

    // 更新动画
    public Animation UpdateAnimation(int id, string name, string description)
    {
        var animation = _context.Animations.FirstOrDefault(a => a.Id == id);
        if (animation == null)
        {
            throw new KeyNotFoundException("Animation not found.");
        }

        animation.Name = name;
        animation.Description = description;
        _context.SaveChanges();
        return animation;
    }

    // 删除动画
    public void RemoveAnimation(int id)
    {
        var animation = _context.Animations.FirstOrDefault(a => a.Id == id);
        if (animation == null)
        {
            throw new KeyNotFoundException("Animation not found.");
        }

        _context.Animations.Remove(animation);
        _context.SaveChanges();
    }
}

// 角色动画系统的主程序
class Program
{
    static void Main(string[] args)
    {
        // 配置数据库连接字符串
        var builder = new DbContextOptionsBuilder<AnimationContext>();
        builder.UseSqlServer("YOUR_CONNECTION_STRING_HERE");

        using (var context = new AnimationContext(builder.Options))
        {
            var animationService = new AnimationService(context);

            try
            {
                // 添加动画
                var newAnimation = animationService.AddAnimation("Run", "The character runs forward.");
                Console.WriteLine($"Added Animation: {newAnimation.Name}");

                // 获取所有动画
                var animations = animationService.GetAllAnimations();
                foreach (var animation in animations)
                {
                    Console.WriteLine($"Animation: {animation.Name} - {animation.Description}");
                }

                // 更新动画
                var updatedAnimation = animationService.UpdateAnimation(newAnimation.Id, "Run Faster", "The character runs faster forward.");
                Console.WriteLine($"Updated Animation: {updatedAnimation.Name}");

                // 删除动画
                animationService.RemoveAnimation(updatedAnimation.Id);
                Console.WriteLine($"Removed Animation: {updatedAnimation.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}