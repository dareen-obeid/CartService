using System;
using Application.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
	public class MappingProfile : Profile
    {
		public MappingProfile()
        {
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartItem, CartItemDto>();

            CreateMap<CartItemDto, CartItem>();
        }
    }
}

