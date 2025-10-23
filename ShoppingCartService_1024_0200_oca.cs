// 代码生成时间: 2025-10-24 02:00:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// 定义购物车实体
public class ShoppingCart {
    public int ShoppingCartId { get; set; }
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}

// 定义购物车项实体
public class CartItem {
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}

// 定义产品实体
public class Product {
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 定义数据库上下文
public class ShoppingCartContext : DbContext {
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Product> Products { get; set; }

    public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) { }
}

// 购物车服务
public class ShoppingCartService {
    private readonly ShoppingCartContext _context;

    public ShoppingCartService(ShoppingCartContext context) {
        _context = context;
    }

    // 添加产品到购物车
    public async Task AddProductToCartAsync(int cartId, int productId, int quantity) {
        var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.ShoppingCartId == cartId);
        if (cart == null) {
            throw new Exception("Shopping cart not found");
        }

        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (existingItem != null) {
            existingItem.Quantity += quantity;
        } else {
            cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
        }

        await _context.SaveChangesAsync();
    }

    // 从购物车移除产品
    public async Task RemoveProductFromCartAsync(int cartId, int productId) {
        var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.ShoppingCartId == cartId);
        if (cart == null) {
            throw new Exception("Shopping cart not found");
        }

        var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (itemToRemove != null) {
            cart.CartItems.Remove(itemToRemove);
            await _context.SaveChangesAsync();
        } else {
            throw new Exception("Product not found in cart");
        }
    }

    // 获取购物车内容
    public async Task<List<CartItem>> GetCartItemsAsync(int cartId) {
        var cart = await _context.ShoppingCarts.Include(ci => ci.CartItems).FirstOrDefaultAsync(c => c.ShoppingCartId == cartId);
        if (cart == null) {
            throw new Exception("Shopping cart not found");
        }

        return cart.CartItems.ToList();
    }
}
