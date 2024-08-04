using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Application.Validation
{
    public class CartValidator : IValidator<CartDto>
	{

        public void Validate(CartDto cart)
        {
            if (cart.CustomerId <= 0)
                throw new ValidationException("Customer ID must be positive.");

            if (cart.CartItems == null || !cart.CartItems.Any())
                throw new ValidationException("Cart must have at least one item.");

            foreach (var item in cart.CartItems)
            {
                if (item.Quantity <= 0)
                    throw new ValidationException("Each item's quantity must be greater than zero.");
                if (item.Price < 0)
                    throw new ValidationException("Item price cannot be negative.");
            }

            //if (cart.CreatedDate >= cart.LastUpdatedDate)
            //    throw new ValidationException("Last updated date must be after the created date.");
        }
    }
}

