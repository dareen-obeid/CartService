using System;
using Domain.Exceptions;
using System.Net;
using Domain.Models;
using Domain.RepositoriyInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByCustomerId(int customerId)
        {
            var cart =  await _context.Carts.Include(c => c.CartItems)
                                      .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.IsActive);

            if (cart == null)
            {
                throw new NotFoundException($"Cart with CustomerID {customerId} not found.");
            }

            return cart;
        }


        public async Task<IEnumerable<CartItem>> GetCartItems(int cartId)
        {
            return await _context.CartItems.Where(ci => ci.CartId == cartId && ci.IsActive).ToListAsync();
        }

        public async Task<CartItem> GetCartItemById(int cartItemId)
        {
           var cartItem = await _context.CartItems
                                 .Where(ci => ci.CartItemId == cartItemId && ci.IsActive)
                                 .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                throw new NotFoundException($"CartItem with ID {cartItemId} not found.");
            }

            return cartItem;

        }

        public async Task UpdateCartItem(CartItem cartItem)
        {
            var cartExists = await _context.Carts.AnyAsync(c => c.CartId == cartItem.CartId && c.IsActive);
            if (!cartExists)
            {
                throw new NotFoundException($"Cart with ID {cartItem.CartId} not found.");
            }

            _context.Entry(cartItem).State = EntityState.Modified;
            cartItem.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task AddToCart(CartItem cartItem)
        {
            var cartExists = await _context.Carts.AnyAsync(c => c.CartId == cartItem.CartId && c.IsActive);
            if (!cartExists)
            {
                throw new NotFoundException($"Cart with ID {cartItem.CartId} not found.");
            }

            cartItem.LastUpdatedDate = DateTime.Now;
            cartItem.CreatedDate = DateTime.Now;

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);


            if (cartItem == null)
            {
                throw new NotFoundException("CartItem not found.");
            }

            cartItem.IsActive = false;
            cartItem.LastUpdatedDate = DateTime.UtcNow;

            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task RemoveCart(int cartId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)
                                           .FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart == null)
            {
                throw new NotFoundException($"Cart with ID {cartId} not found.");
            }

            cart.IsActive = false;
            cart.LastUpdatedDate = DateTime.UtcNow;
            foreach (var item in cart.CartItems)
            {
                item.IsActive = false;
                item.LastUpdatedDate = DateTime.UtcNow;

            }
            await _context.SaveChangesAsync();
        }
    }
}