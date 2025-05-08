using BlogApp.Data.Abstract.IRepository;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Data.Concrete.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mysql_connection");
    //options.UseSqlite(connectionString);
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 1, 0)));
});

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.LoginPath = "/User/Login";
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

await SeedData.CreateTestDataAsync(app);

app.MapControllerRoute(
    name: "post_detail",
    pattern: "post/detail/{url}",
    defaults: new { controller = "Post", action = "Detail" }
);

app.MapControllerRoute(
    name: "post_filter_tag",
    pattern: "post/tag/{tagUrl}",
    defaults: new { controller = "Post", action = "Index" }
);

app.MapControllerRoute(
    name: "user_profile",
    pattern: "profile/{userName}",
    defaults: new { controller = "User", action = "Profile" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}"
);

app.MapGet("/", context =>
{
    context.Response.Redirect("/post");
    return Task.CompletedTask;
});

app.Run();
