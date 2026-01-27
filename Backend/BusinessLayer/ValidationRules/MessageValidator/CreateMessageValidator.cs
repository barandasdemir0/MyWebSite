using DtoLayer.MessageDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.MessageValidator
{
    public class CreateMessageValidator:AbstractValidator<CreateMessageDto>
    {
        public CreateMessageValidator()
        {

            RuleFor(x => x.SenderName).NotEmpty()
                .WithMessage("Bu alanı lütfen doldurunuz")
                .MinimumLength(5)
                .WithMessage("Lütfen Normal uzunluklarda Ad soyad giriniz 5 karakterden az olmamalı")
                .MaximumLength(100)
                .WithMessage("Lütfen Normal Uzunluklarda ad ve soyad giriniz 100 karakterden fazla olmamalı");

            RuleFor(x => x.SenderEmail).NotEmpty()
                .WithMessage("Bu alanı lütfen doldurunuz")
                .EmailAddress()
                .WithMessage("Lütfen Mail standartlarına uysun")
                .MaximumLength(100)
                .WithMessage("Lütfen Normal Uzunluklarda ad ve soyad giriniz 100 karakterden fazla olmamalı");

            RuleFor(x => x.ReceiverEmail).NotEmpty()
              .WithMessage("Bu alanı lütfen doldurunuz")
              .EmailAddress()
              .WithMessage("Lütfen Mail standartlarına uysun")
              .MaximumLength(100)
              .WithMessage("Lütfen Normal Uzunluklarda ad ve soyad giriniz 100 karakterden fazla olmamalı");

            RuleFor(x => x.Subject).NotEmpty()
              .WithMessage("Bu alanı lütfen doldurunuz")
              .Must(x => !string.IsNullOrWhiteSpace(x))
              .WithMessage("Konu Alanı Sadece Boşluklardan oluşamaz")
              .MinimumLength(5)
              .WithMessage("Lütfen Normal uzunluklarda konu giriniz 5 karakterden az olmamalı")
              .MaximumLength(200)
              .WithMessage("Lütfen Normal Uzunluklarda konu giriniz 100 karakterden fazla olmamalı");


            RuleFor(x => x.Body).NotEmpty()
              .WithMessage("Bu alanı lütfen doldurunuz")
               .Must(x => !string.IsNullOrWhiteSpace(x))
              .WithMessage("Konu Alanı Sadece Boşluklardan oluşamaz")
              .MinimumLength(5)
              .WithMessage("Lütfen Normal uzunluklarda konu giriniz 5 karakterden az olmamalı");


        }
    }
}
