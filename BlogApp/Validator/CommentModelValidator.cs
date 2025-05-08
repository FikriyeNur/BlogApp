using BlogApp.Models;
using FluentValidation;

namespace BlogApp.Validator
{
    public class CommentModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentModelValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("Yorum giriniz!")
                                .MinimumLength(5).WithMessage("Yorum en az 5 karakter olmalıdır!");
        }
    }
}