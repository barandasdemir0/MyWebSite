using CV.EntityLayer.Entities;
using DtoLayer.BlogpostDto;
using DtoLayer.CertificateDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class CertificateMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Certificate, CertificateDto.CertificateDto>();
            config.NewConfig<CreateCertificateDto, Certificate>().Ignore(x => x.Id);
            config.NewConfig<UpdateCertificateDto, Certificate>().Ignore(x => x.Id);
        }
    }
}
