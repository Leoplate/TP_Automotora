using FluentValidation;
using technical_tests_backend_ssr.Models;

public class ClienteValidator : AbstractValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres.");
        RuleFor(p => p.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .Length(3, 100).WithMessage("El apellido debe tener entre 3 y 100 caracteres.");
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("El Email es obligatorio.")
            .Length(3, 30).WithMessage("El Email debe tener entre 3 y 30 caracteres.");
        RuleFor(p => p.Telefono)
            .NotEmpty().WithMessage("El telefono es obligatorio.")
            .Length(3, 12).WithMessage("El telefono debe tener entre 3 y 12 caracteres.");

    }
}
