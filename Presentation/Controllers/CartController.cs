using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCartByCustomerId(int customerId)
        {
            return Ok(await _cartService.GetCartByCustomerId(customerId));
        }

        [HttpGet("items/{cartId}")]
        public async Task<IActionResult> GetCartItems(int cartId)
        {
            return Ok(await _cartService.GetCartItemsByCartId(cartId));
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemDto cartItemDto)
        {
            try
            {
                await _cartService.AddItemToCart(cartItemDto);
                return CreatedAtAction(nameof(GetCartItems), new { cartId = cartItemDto.CartId }, cartItemDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] CartItemDto cartItemDto)
        {
            if (cartItemId != cartItemDto.CartItemId)
            {
                return BadRequest("Mismatched Cart Item ID");
            }

            try
            {
                await _cartService.UpdateCartItem(cartItemDto);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("removeCartItem/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            await _cartService.RemoveCartItem(cartItemId);
            return NoContent();
        }

        [HttpDelete("removeCart/{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            await _cartService.RemoveCartAndItems(cartId);
            return NoContent(); 
        }
    }
}
