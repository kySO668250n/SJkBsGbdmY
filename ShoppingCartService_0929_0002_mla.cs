// 代码生成时间: 2025-09-29 00:02:14
using System;
# 扩展功能模块
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
# 改进用户体验

// 数据模型
public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class CartItem
# NOTE: 重要实现细节
{
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}

public class ShoppingCart
# NOTE: 重要实现细节
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}

// 数据库上下文
public class ShoppingCartContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
    {
    }
}

// 购物车服务
public class ShoppingCartService
{
# 扩展功能模块
    private readonly ShoppingCartContext _context;

    public ShoppingCartService(ShoppingCartContext context)
    {
        _context = context;
    }

    // 添加商品到购物车
    public async Task AddProductToCartAsync(int productId, int quantity)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }

        var cartItem = new CartItem { ProductId = productId, Quantity = quantity };
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
    }

    // 获取购物车内容
    public async Task<ShoppingCart> GetCartAsync()
    {
        var cart = new ShoppingCart();
        var cartItems = await _context.CartItems.Include(c => c.Product).ToListAsync();
        cart.Items = cartItems;
        return cart;
    }

    // 更新购物车中的商品数量
    public async Task UpdateCartQuantityAsync(int cartItemId, int quantity)
    {
        var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        if (cartItem == null)
        {
            throw new ArgumentException("CartItem not found.");
        }

        cartItem.Quantity = quantity;
        await _context.SaveChangesAsync();
    }

    // 从购物车中移除商品
    public async Task RemoveProductFromCartAsync(int cartItemId)
# TODO: 优化性能
    {
        var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        if (cartItem == null)
        {
# TODO: 优化性能
            throw new ArgumentException("CartItem not found.");
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }
}
