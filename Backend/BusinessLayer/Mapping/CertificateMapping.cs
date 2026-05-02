using CV.EntityLayer.Entities;
using DtoLayer.CertificateDtos;
using Mapster;

namespace DtoLayer.Mapping;

public sealed class CertificateMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Certificate, CertificateDtos.CertificateDto>();
        config.NewConfig<CreateCertificateDto, Certificate>().Ignore(x => x.Id);
        config.NewConfig<UpdateCertificateDto, Certificate>().Ignore(x => x.Id);
    }
}
