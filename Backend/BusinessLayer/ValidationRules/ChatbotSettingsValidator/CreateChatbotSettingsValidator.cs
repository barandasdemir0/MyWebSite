using DtoLayer.ChatbotSettingsDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules.ChatbotSettingsValidator;

public class CreateChatbotSettingsValidator:AbstractValidator<CreateChatbotSettingsDto>
{
    public CreateChatbotSettingsValidator()
    {
        RuleFor(x => x.AssistantName)
           .NotEmpty().WithMessage("Asistan adı boş geçilemez.")
           .MaximumLength(100).WithMessage("Asistan adı 100 karakterden fazla olamaz.");
        RuleFor(x => x.WelcomeMessage)
            .MaximumLength(500).WithMessage("Karşılama mesajı 500 karakteri geçmemelidir.");
        RuleFor(x => x.SystemPrompt)
            .NotEmpty().WithMessage("System Prompt (Talimatlar) boş olamaz, yapay zeka ne yapacağını bilmeli.")
            .MaximumLength(5000).WithMessage("System Prompt çok uzun.");
        RuleFor(x => x.ModelName)
           .MaximumLength(100).WithMessage("Model adı çok uzun.");
    }
}
