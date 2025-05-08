using BlogApp.Models;
using FluentValidation;

namespace BlogApp.Validator
{
    public class TagModelValidator : AbstractValidator<TagModel>
    {
        public TagModelValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("Etiket adı giriniz!");

            RuleFor(x => x.Url).NotEmpty().WithMessage("Etiket url giriniz!");
        }
    }
}