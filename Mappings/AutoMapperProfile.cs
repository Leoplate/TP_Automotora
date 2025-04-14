using AutoMapper;
using technical_tests_backend_ssr.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Producto, ProductoDTO>().ReverseMap();
        CreateMap<ProductoDTO, Producto>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Cliente, ClienteDTO>().ReverseMap();
        CreateMap<ClienteDTO, Cliente>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Venta, VentaDTO>().ReverseMap();
        CreateMap<VentaDTO, Venta>().ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Venta, VentaCompletaDTO>()
            .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId))
            .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.ClienteEmail, opt => opt.MapFrom(src => src.Cliente.Email))
            .ForMember(dest => dest.ProductoId, opt => opt.MapFrom(src => src.VehiculoId))
            .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Vehiculo.Nombre))
            .ForMember(dest => dest.ProductoPrecio, opt => opt.MapFrom(src => src.Vehiculo.Precio))
            //.ForMember(dest => dest.ProductoStock, opt => opt.MapFrom(src => src.Vehiculo.Stock))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
    }



}


