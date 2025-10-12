// 代码生成时间: 2025-10-13 02:17:21
// VirtualScrollingList.cs
// This class demonstrates the implementation of a virtual scrolling list using C# and Entity Framework.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualScrollingExample
{
    public class VirtualScrollingList<T> where T : class
    {
        private readonly DbContext _context;
        private readonly int _pageSize;
        private readonly Func<DbContext, IQueryable<T>> _query;
        private readonly int _totalItems;

        public VirtualScrollingList(DbContext context, int pageSize, Func<DbContext, IQueryable<T>> query)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));
            _query = query ?? throw new ArgumentNullException(nameof(query));

            // Calculate the total number of items in the database
            _pageSize = pageSize;
            _totalItems = query(_context).Count();
        }

        // Get a page of items from the database
        public async Task<List<T>> GetPageAsync(int page)
        {
            if (page < 0) throw new ArgumentOutOfRangeException(nameof(page));
            int skip = page * _pageSize;
            var items = await _query(_context).Skip(skip).Take(_pageSize).ToListAsync();
            return items;
        }

        // Get the total number of items in the list
        public int TotalItems => _totalItems;

        // Get the total number of pages
        public int TotalPages => (_totalItems + _pageSize - 1) / _pageSize;
    }

    // Example usage class
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a new instance of the DbContext (assuming it's named AppDbContext)
            using (var context = new AppDbContext())
            {
                // Create an instance of the virtual scrolling list
                var virtualList = new VirtualScrollingList<YourEntityType>(context, 20, db => db.Set<YourEntityType>().AsQueryable());

                // Get the first page of items
                var page = await virtualList.GetPageAsync(0);

                // Display the items on the page
                foreach (var item in page)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }
    }
}
