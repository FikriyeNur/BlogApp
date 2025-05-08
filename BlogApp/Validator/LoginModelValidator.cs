using BlogApp.Models;
using FluentValidation;

namespace BlogApp.Validator
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta giriniz!")
                                 .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz!");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre giriniz!")
                                    .MinimumLength(4).WithMessage("Şifre en az 4 karakter olmalıdır!");
        }
    }
}