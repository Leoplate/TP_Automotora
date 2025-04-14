using FluentValidation;

public class VentaDTOValidator : AbstractValidator<VentaDTO>
{
    public VentaDTOValidator()
    {
        RuleFor(p => p.ClienteId)
            .NotEmpty().WithMessage("El id de Cliente es obligatorio.");
            //.Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres.");

        RuleFor(p => p.VehiculoId)
            .NotEmpty().WithMessage("El id de Vehiculo es obligatori");

        RuleFor(p => p.Total)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
    }
}
