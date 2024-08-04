using System;
using Application.DTOs;
using Application.Services.Interfaces;
using Application.Validation;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Domain.RepositoriyInterfaces;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CartDto> _cartValidator;
        private readonly IValidator<CartItemDto> _cartItemValidator;

        public CartService(ICartRepository cartRepository, IMapper mapper, IValidator<CartDto> cartValidator, IValidator<CartItemDto> cartItemValidator)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _cartValidator = cartValidator;
            _cartItemValidator = cartItemValidator;
        }

        public async Task<CartDto> GetCartByCustomerId(int customerId)
        {
            var cart = await _cartRepository.GetCartByCustomerId(customerId);
            if (cart == null || !cart.IsActive)
            {
                throw new NotFoundException($"Cart with CustomerID {customerId} not found or is inactive.");
            }
            var cartDto = _mapper.Map<CartDto>(cart);
            _cartValidator.Validate(cartDto);  // Optional, depends on context
            return cartDto;
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartId(int cartId)
        {
            var cartItems = await _cartRepository.GetCartItems(cartId);
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task AddItemToCart(CartItemDto cartItemDto)
        {
            _cartItemValidator.Validate(cartItemDto);
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            await _cartRepository.AddToCart(cartItem);
        }

        public async Task UpdateCartItem(CartItemDto cartItemDto)
        {
            _cartItemValidator.Validate(cartItemDto);
            var cartItem = await _cartRepository.GetCartItemById(cartItemDto.CartItemId);
            if (cartItem == null || !cartItem.IsActive)
            {
                throw new NotFoundException($"CartItem with ID {cartItemDto.CartItemId} not found or is inactive.");
            }
            _mapper.Map(cartItemDto, cartItem);
            cartItem.LastUpdatedDate = DateTime.UtcNow;
            await _cartRepository.UpdateCartItem(cartItem);
        }

        public async Task RemoveCartItem(int cartItemId)
        {
            var cartItem = await _cartRepository.GetCartItemById(cartItemId);
            if (cartItem == null || !cartItem.IsActive)
            {
                throw new NotFoundException($"CartItem with ID {cartItemId} not found or is inactive.");
            }
            cartItem.IsActive = false;
            cartItem.LastUpdatedDate = DateTime.UtcNow;
            await _cartRepository.UpdateCartItem(cartItem); 
        }


        public async Task RemoveCartAndItems(int cartId)
        {
            await _cartRepository.RemoveCart(cartId);
        }
    }
}
