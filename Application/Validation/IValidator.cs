using System;
using Application.DTOs;

namespace Application.Validation
{
    public interface IValidator<T>
    {
        void Validate(T entity);
    }
}

