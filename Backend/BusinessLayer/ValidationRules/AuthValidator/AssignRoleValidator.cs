using DtoLayer.AuthDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.AuthValidator;

public class AssignRoleValidator:AbstractValidator<AssignRoleDto>
{
    public AssignRoleValidator()
    {
        RuleFor(x => x.UserId)
          .NotEmpty().WithMessage("Kullanıcı kimliği zorunludur");
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Rol alanı zorunludur");
    }
}
