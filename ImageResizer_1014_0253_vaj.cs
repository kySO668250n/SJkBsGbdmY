// 代码生成时间: 2025-10-14 02:53:24
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// 图片尺寸批量调整器
public class ImageResizer
{
    // 目标尺寸
    private readonly int width;
    private readonly int height;

    // 构造函数
    public ImageResizer(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    // 批量调整图片尺寸
    public async Task ResizeImagesAsync(string sourceDirectory, string targetDirectory)
    {
        if (string.IsNullOrEmpty(sourceDirectory))
        {
            throw new ArgumentException("Source directory cannot be null or empty.");
# TODO: 优化性能
        }

        if (string.IsNullOrEmpty(targetDirectory))
        {
            throw new ArgumentException("Target directory cannot be null or empty.");
        }

        // 检查源目录是否存在
        if (!Directory.Exists(sourceDirectory))
        {
            throw new DirectoryNotFoundException("Source directory not found.");
        }

        // 检查目标目录是否存在，如果不存在，则创建
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        // 获取源目录下所有图片文件
        var imageFiles = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
            .Where(file => Image.Formats.Any(f => f.FileExtensions.Contains(Path.GetExtension(file))))
            .ToList();

        // 批量处理每个图片文件
        foreach (var file in imageFiles)
        {
            try
            {
                using (Image image = Image.Load(file))
                {
                    // 调整图片尺寸
                    image.Mutate(x => x.Resize(width, height));

                    // 构建目标文件路径
                    var targetFilePath = Path.Combine(targetDirectory, Path.GetFileName(file));

                    // 保存调整后的图片到目标目录
                    image.Save(targetFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resizing image {file}: {ex.Message}");
            }
        }
    }
}

// 程序入口和示例用法
public class Program
{
# 添加错误处理
    public static async Task Main(string[] args)
    {
        var sourceDirectory = "C:/SourceImages";
        var targetDirectory = "C:/ResizedImages";
        var width = 800;
        var height = 600;

        try
        {
            var imageResizer = new ImageResizer(width, height);
# 改进用户体验
            await imageResizer.ResizeImagesAsync(sourceDirectory, targetDirectory);
            Console.WriteLine("Image resizing completed.");
# 扩展功能模块
        }
        catch (Exception ex)
        {
# NOTE: 重要实现细节
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
# TODO: 优化性能
    }
}