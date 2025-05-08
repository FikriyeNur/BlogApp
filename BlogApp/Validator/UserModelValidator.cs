using BlogApp.Models;
using FluentValidation;

namespace BlogApp.Validator
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta giriniz!")
                                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz!");

            RuleFor(x => x.FullName).NotEmpty().WithMessage("Ad Soyad giriniz!")
                                    .MinimumLength(5).WithMessage("Ad Soyad en az 5 karakter olmalıdır!");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre giriniz!")
                                    .MinimumLength(4).WithMessage("Şifre en az 4 karakter olmalıdır!");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Şifreler uyuşmuyor!")
                                           .NotEmpty().WithMessage("Şifre tekrarı giriniz!");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı giriniz!")
                                    .MinimumLength(4).WithMessage("Kullanıcı adı en az 4 karakter olmalıdır!");

            RuleFor(x => x.ImageFile).NotNull().WithMessage("Resim seçiniz!")
                                     .Must(file => file != null && (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".png"))).WithMessage("Sadece .jpg, .jpeg veya .png uzantılı dosyalar kabul edilir.");
        }
    }
}