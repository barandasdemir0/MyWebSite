using DtoLayer.CertificateDto;
using DtoLayer.ContactDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.ContactValidator;

public class CreateContactValidator : AbstractValidator<CreateContactDto>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .EmailAddress()
            .WithMessage("Geçerli Bir E-Posta Adresi giriniz")
            .MaximumLength(75)
            .WithMessage("Mailiniz Standarttan uzundur tekrar Kontrol ediniz");

        RuleFor(x => x.Phone).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .Matches(@"^\+?\d{10,20}$")
            .WithMessage("Telefon numarası geçerli formatta olmalıdır")
            .MinimumLength(10)
            .WithMessage("Standart Gereği Telefon Numaranız en az 10 haneli olmalıdır")
            .MaximumLength(20)
            .WithMessage("Standart Gereği Telefon Numaranız en fazla 20 haneli olmalıdır");

        RuleFor(x => x.Location).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MaximumLength(200)
            .WithMessage("Adresiniz Standarttan uzundur tekrar Kontrol ediniz");

        RuleFor(x => x.LocationPicture).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MaximumLength(200)
            .WithMessage("Adresiniz Resmi Standarttan uzundur tekrar Kontrol ediniz");

        RuleFor(x => x.ContactTitle).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MaximumLength(50)
            .WithMessage("Başlık 50 Karakterden fazla olamaz");


        RuleFor(x => x.ContactText).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MaximumLength(200)
            .WithMessage("Başlık 200 Karakterden fazla olamaz");

        RuleFor(x => x.SuccessMessageText).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MaximumLength(150)
            .WithMessage("Başlık 150 Karakterden fazla olamaz");

        RuleFor(x => x.WorkStatus).NotEmpty()
            .WithMessage("Bu Alan Boş Geçilemez")
            .MaximumLength(100)
            .WithMessage("Başlık 100 Karakterden fazla olamaz");


    }
}
