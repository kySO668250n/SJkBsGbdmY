// 代码生成时间: 2025-11-01 07:08:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace
{
    // 假设有一个DbContext类继承自DbContext，用于数据库操作
    public class YourDbContext : DbContext
    {
        public DbSet<SortableItem> SortableItems { get; set; }
    }

    // 实体类SortableItem，代表可排序项
    public class SortableItem
    {
        public int Id { get; set; }
        public int? PreviousItemId { get; set; }  // 指向上一个项的Id，用于排序
        public int? NextItemId { get; set; }     // 指向下一个项的Id，用于排序
    }

    // 服务类DragAndDropService，实现拖拽排序逻辑
    public class DragAndDropService
    {
        private readonly YourDbContext _context;

        public DragAndDropService(YourDbContext context)
        {
            _context = context;
        }

        // 更新排序位置
        public async Task UpdateSortOrder(int itemId, int previousItemId)
        {
            try
            {
                var itemToMove = await _context.SortableItems.FindAsync(itemId);
                if (itemToMove == null)
                {
                    throw new InvalidOperationException($"Item with id {itemId} not found.");
                }

                var previousItem = await _context.SortableItems.FindAsync(previousItemId);
                if (previousItem == null)
                {
                    throw new InvalidOperationException($"Previous item with id {previousItemId} not found.");
                }

                // 交换位置
                (itemToMove.PreviousItemId, previousItem.PreviousItemId) = (previousItem.PreviousItemId, itemToMove.PreviousItemId);

                // 更新相关项的NextItemId
                if (previousItem.PreviousItemId.HasValue)
                {
                    var previousPreviousItem = await _context.SortableItems.FindAsync(previousItem.PreviousItemId.Value);
                    if (previousPreviousItem != null)
                    {
                        previousPreviousItem.NextItemId = itemToMove.Id;
                    }
                }
                else
                {
                    // 如果previousItemId是根节点
                    _context.SortableItems.FirstOrDefault()?.PreviousItemId = itemToMove.Id;
                }

                if (itemToMove.PreviousItemId.HasValue)
                {
                    var itemAfterItemToMove = await _context.SortableItems.FindAsync(itemToMove.PreviousItemId.Value);
                    if (itemAfterItemToMove != null)
                    {
                        itemAfterItemToMove.NextItemId = previousItem.Id;
                    }
                }
                else
                {
                    // 如果itemId是根节点
                    _context.SortableItems.FirstOrDefault()?.NextItemId = previousItem.Id;
                }

                // 保存更改
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 错误处理
                Console.WriteLine($"Error updating sort order: {ex.Message}");
                throw;
            }
        }
    }
}
