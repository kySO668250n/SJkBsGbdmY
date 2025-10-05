// 代码生成时间: 2025-10-05 18:16:27
using System;
using System.Threading.Tasks;

namespace RandomNumberApp
{
    // 随机数生成器类
    public class RandomNumberGenerator
    {
        private readonly Random _random;

        // 构造函数，初始化随机数生成器
        public RandomNumberGenerator()
        {
            _random = new Random();
        }

        // 生成一个指定范围的随机数
        public int GenerateRandomNumber(int minValue, int maxValue)
        {
            // 检查参数有效性
            if (minValue > maxValue)
            {
                throw new ArgumentException("MinValue cannot be greater than MaxValue", nameof(minValue));
            }

            return _random.Next(minValue, maxValue + 1);
        }

        // 异步方法，生成一个指定范围的随机数
        public async Task<int> GenerateRandomNumberAsync(int minValue, int maxValue)
        {
            // 检查参数有效性
            if (minValue > maxValue)
            {
                throw new ArgumentException("MinValue cannot be greater than MaxValue", nameof(minValue));
            }

            // 模拟异步操作
            await Task.Delay(1);

            return _random.Next(minValue, maxValue + 1);
        }
    }
}
