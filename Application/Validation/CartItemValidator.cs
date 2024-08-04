using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Application.Validation
{
	public class CartItemValidator : IValidator<CartItemDto>
	{

        public void Validate(CartItemDto cartItem)
        {
            if (cartItem.ProductId <= 0)
                throw new ValidationException("Invalid Product ID.");

            if (cartItem.Quantity <= 0)
                throw new ValidationException("Quantity must be greater than zero.");

            if (cartItem.Price <= 0)
                throw new ValidationException("Price must not be negative.");

            if (cartItem.CreatedDate == default)
                throw new ValidationException("Creation date is required.");

            if (cartItem.LastUpdatedDate == default)
                throw new ValidationException("Last updated date is required.");
        }
    }
}

