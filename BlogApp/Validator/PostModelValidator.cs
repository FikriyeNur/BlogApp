
using BlogApp.Models;
using FluentValidation;

namespace BlogApp.Validator
{
    public class PostModelValidator : AbstractValidator<PostModel>
    {
        public PostModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık ekleyiniz!");

            RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik ekleyiniz!");

            RuleFor(x => x.ImageFile).NotNull()
                                    .When(x => x.Image == null).WithMessage("Resim seçiniz!")
                                    .Must(file => file != null && (file.FileName.EndsWith(".jpg")
                                    || file.FileName.EndsWith(".jpeg")
                                    || file.FileName.EndsWith(".png")))
                                    .When(x => x.ImageFile != null || x.Image == null).WithMessage("Sadece .jpg, .jpeg veya .png uzantılı dosyalar kabul edilir.");

            RuleFor(x => x.SelectedTags).NotEmpty().WithMessage("En az bir etiket seçiniz!")
                                        .NotEqual("[]").WithMessage("En az bir etiket seçiniz!"); 

            RuleFor(x => x.Url).NotEmpty().WithMessage("Url ekleyiniz!");
        }
    }

}