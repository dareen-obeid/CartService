using System;
using Application.DTOs;
using Domain.Models;

namespace Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartByCustomerId(int customerId);
        Task AddItemToCart(CartItemDto cartItemDto);
        Task UpdateCartItem(CartItemDto cartItemDto);
        Task RemoveCartItem(int cartItemId);
        Task RemoveCartAndItems(int cartId);
        Task<IEnumerable<CartItemDto>> GetCartItemsByCartId(int cartId);
    }

}

