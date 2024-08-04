using System;
using Domain.Models;

namespace Domain.RepositoriyInterfaces
{
	public interface ICartRepository
	{
        Task<Cart> GetCartByCustomerId(int customerId);
        Task<CartItem> GetCartItemById(int cartItemId);
        Task AddToCart(CartItem cartItem);
        Task UpdateCartItem(CartItem cartItem);
        Task RemoveCartItem(int cartItemId);
        Task RemoveCart(int cartId);
        Task<IEnumerable<CartItem>> GetCartItems(int cartId);

    }
}

