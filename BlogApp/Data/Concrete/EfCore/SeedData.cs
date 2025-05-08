
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static async Task CreateTestDataAsync(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<BlogContext>();

            if (context != null)
            {
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await context.Database.MigrateAsync();
                }

                if (!await context.Tags.AnyAsync())
                {
                    await context.Tags.AddRangeAsync(
                       new Tag { Text = "web programlama", Url = "web-programlama" },
                       new Tag { Text = "backend", Url = "backend" },
                       new Tag { Text = "frontend", Url = "frontend" },
                       new Tag { Text = "C#", Url = "csharp" },
                       new Tag { Text = "Asp.Net Core", Url = "asp-net-core" },
                       new Tag { Text = "Python", Url = "python" },
                       new Tag { Text = "Angular", Url = "angular" },
                       new Tag { Text = "React", Url = "react" }
                    );

                    await context.SaveChangesAsync();
                }

                if (!await context.Users.AnyAsync())
                {
                    await context.Users.AddRangeAsync(
                        new User { UserName = "Fikriye Nur", Image = "user.jpg", Password = "1234", FullName = "Fikriye Nur Harmandar", Email = "fikriyenur@gmail.com" },
                        new User { UserName = "Anıl", Image = "user.jpg", Password = "1234", FullName = "Anıl Akay", Email = "anilakay@gmail.com" },
                        new User { UserName = "Zeynep", Image = "user.jpg", Password = "1234", FullName = "Zeynep Yılmaz", Email = "zeynepyilmaz@gmail.com" },
                        new User { UserName = "Semih", Image = "user.jpg", Password = "1234", FullName = "Semih Soyer", Email = "semihsoyer@gmail.com" }
                    );

                    await context.SaveChangesAsync();
                }

                if (!await context.Posts.AnyAsync())
                {
                    await context.Posts.AddRangeAsync(
                        new Post
                        {
                            Title = "Web Programlama",
                            Content = "Web programlama, internet tarayıcılarında çalışan dinamik ve etkileşimli web siteleri veya uygulamalar geliştirme sürecidir. Bu süreçte kullanılan teknolojiler arasında HTML (Hypertext Markup Language), CSS (Cascading Style Sheets) ve JavaScript bulunur. HTML, web sayfasının iskeletini oluştururken CSS tasarımı ve görünümü belirler. JavaScript ise sayfayı dinamik hale getirerek kullanıcı etkileşimlerini mümkün kılar. " +
                            "\n\nWeb programlama, istemci (client) ve sunucu (server) tarafında farklı diller ve teknolojilerle gerçekleştirilir. İstemci tarafında React, Angular, veya Vue.js gibi JavaScript kütüphaneleri ve frameworkleri yaygın olarak kullanılır. Sunucu tarafında ise ASP.NET, PHP, Python (Django, Flask), Node.js gibi teknolojiler veri işleme ve yönetim görevlerini yerine getirir." +
                            "\n\nModern web geliştirme, yalnızca içerik sunmanın ötesine geçmiştir. Web programlama ile sosyal medya platformları, e-ticaret siteleri, yönetim panelleri ve daha fazlası oluşturulabilir. Bunun yanı sıra API'ler (Application Programming Interfaces), web uygulamalarının farklı sistemlerle entegrasyonunu sağlar. RESTful ve GraphQL gibi API standartları, veri alışverişini kolaylaştırır." +
                            "\n\nWeb güvenliği, web programlamanın önemli bir parçasıdır. XSS (Cross-Site Scripting), CSRF (Cross-Site Request Forgery) ve SQL Injection gibi yaygın tehditlere karşı önlem alınması gerekir. Bunun için güvenlik standartlarına uygun kodlama ve güvenlik duvarı gibi araçlar kullanılır." +
                            "\n\nWeb programlama gelecekte de önemli bir rol oynamaya devam edecek. Progressive Web Apps (PWA), yapay zeka entegrasyonu ve Web 3.0 teknolojileri, bu alanın evrimini şekillendiren başlıca yeniliklerdir. Bu nedenle web programlama, sürekli öğrenme ve gelişim gerektiren bir alan olarak ön plana çıkar.",
                            Image = "1.jpg",
                            Url = "web-programlama",
                            IsActive = true,
                            PublishedDate = DateTime.Now.AddDays(-10),
                            Tags = await context.Tags.Take(3).ToListAsync(),
                            User = await context.Users.FirstOrDefaultAsync() ?? new User { UserName = "Default User" },
                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Text = "Harika bir yazı olmuş",
                                    PublishedDate = DateTime.Now.AddDays(-10),
                                    Post = await context.Posts.FirstOrDefaultAsync() ?? new Post(),
                                    User = await context.Users.FirstOrDefaultAsync() ?? new User(),
                                },
                            }
                        },
                        new Post
                        {
                            Title = "Backend",
                            Content = "Backend, bir web uygulamasının veya yazılımın sunucu tarafında çalışan kısmıdır. Kullanıcıların doğrudan görmediği bu katman, uygulamanın iş mantığını yönetir, verileri işler ve kullanıcıdan gelen talepleri karşılar. Backend, istemci tarafından yapılan isteklerin sunucu tarafından işlenip uygun cevapların döndürülmesini sağlar. " +
                            "\n\nBackend geliştirmede kullanılan teknolojiler arasında PHP, Python, Ruby, Java, C#, Node.js gibi programlama dilleri bulunur. Bu diller genellikle framework veya kütüphanelerle desteklenir. Örneğin, Python'da Django ve Flask, PHP'de Laravel, C#'ta ASP.NET Core yaygın olarak kullanılır." +
                            "\n\nVeritabanları da backend geliştirme sürecinin önemli bir parçasıdır. Veriler genellikle ilişkisel veritabanları (MySQL, PostgreSQL, SQL Server) veya NoSQL veritabanları (MongoDB, Cassandra) üzerinde saklanır. Backend tarafında, ORM (Object-Relational Mapping) araçları kullanılarak veritabanı işlemleri kolaylaştırılır." +
                           "\n\nBackend aynı zamanda API'ler (Application Programming Interfaces) aracılığıyla istemci ile veri alışverişini yönetir. RESTful API'ler ve GraphQL, veri alışverişi için sık kullanılan yaklaşımlardır. Bu sayede farklı istemci uygulamaları (mobil uygulamalar, web uygulamaları, IoT cihazları) backend ile iletişim kurabilir." +
                           "\n\nGüvenlik, backend geliştirmede kritik bir konudur. Kullanıcı doğrulama, veri şifreleme, güvenli veri saklama ve saldırılara karşı savunma mekanizmalarının (örneğin, SQL Injection, XSS) oluşturulması backend geliştiricilerin sorumluluğundadır." +
                           "\n\nPerformans optimizasyonu da backend geliştirmede önemlidir. Yüksek trafikli uygulamalarda veritabanı sorgularını optimize etmek, önbellekleme mekanizmalarını kullanmak ve sunucu kaynaklarını etkili yönetmek performansı artırır. Ayrıca, yük dengeleme (load balancing) ve mikro servis mimarisi gibi yöntemler büyük ölçekli sistemler için tercih edilir." +
                           "\n\nSonuç olarak, backend, bir uygulamanın iş mantığını ve veri yönetimini sağlayan temel katmandır. Modern yazılım geliştirme süreçlerinde, backend geliştiricilerinin güvenlik, performans ve ölçeklenebilirlik gibi konulara odaklanması hayati öneme sahiptir.",
                            Image = "2.jpg",
                            Url = "backend",
                            IsActive = true,
                            PublishedDate = DateTime.Now.AddDays(-5),
                            Tags = await context.Tags.Take(2).ToListAsync(),
                            User = await context.Users.Skip(1).FirstOrDefaultAsync() ?? new User { UserName = "Default User" },
                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Text = "Teşekkürler",
                                    PublishedDate = DateTime.Now.AddDays(-1),
                                    UserId = (await context.Users.Skip(2).FirstOrDefaultAsync())?.UserId ?? 0
                                },
                                new Comment
                                {
                                    Text = "Yazılarınızı büyük bir ilgiyle takip ediyorum. Harika!",
                                    PublishedDate = DateTime.Now.AddDays(-3),
                                    UserId = (await context.Users.Skip(3).FirstOrDefaultAsync())?.UserId ?? 0
                                }
                            }
                        },
                        new Post
                        {
                            Title = "C# ve Asp.Net Core",
                            Content = "C# ve ASP.NET Core, modern web uygulamaları geliştirmek için Microsoft tarafından sunulan güçlü teknolojilerdir. C#, geniş kullanım alanına sahip, nesne yönelimli, tip güvenli bir programlama dilidir ve genellikle Microsoft platformlarında yazılım geliştirme için tercih edilir. " +
                            "\n\nASP.NET Core ise açık kaynaklı, platformlar arası çalışan ve bulut tabanlı uygulamalar için optimize edilmiş bir web uygulama çerçevesidir. ASP.NET Core, hem performans hem de esneklik açısından önceki ASP.NET sürümlerine göre önemli avantajlar sunar." +
                            "\n\nC# ile ASP.NET Core'un güçlü bir şekilde entegrasyonu sayesinde, geliştiriciler dinamik, ölçeklenebilir ve yüksek performanslı web uygulamaları oluşturabilirler. Bu teknoloji kombinasyonu, Model-View-Controller (MVC) desenini destekler ve kullanıcı arabirimi, iş mantığı ve veri erişim katmanlarını temiz bir şekilde ayırmayı mümkün kılar. Razor Pages, Blazor gibi modern yaklaşımlar ise geliştiricilere daha fazla esneklik sunar." +
                            "\n\nASP.NET Core, HTTP taleplerini yönetmek için minimal API ve middleware kullanımı ile performansı optimize eder. Ayrıca, Dependency Injection (DI) desteği, uygulamanın modüler yapıda geliştirilmesini sağlar ve kodun test edilebilirliğini artırır." +
                            "\n\nGüvenlik, ASP.NET Core ile geliştirilmiş uygulamalarda temel bir önceliktir. Identity framework ile kimlik doğrulama ve yetkilendirme süreçleri kolayca yönetilebilir. Ayrıca, HTTPS kullanımı, veri şifreleme ve cross-site scripting (XSS) gibi tehditlere karşı yerleşik koruma mekanizmaları sunar." +
                            "\n\nC# ve ASP.NET Core kullanarak geliştirilen uygulamalar, hem klasik sunucu tabanlı web çözümleri hem de RESTful API'ler veya mikro servis mimarisi gibi modern yaklaşımlar için idealdir. Bu teknolojiler ayrıca, bulut platformlarına (Azure, AWS) kolayca entegre edilebilir ve CI/CD süreçleriyle otomasyon sağlanabilir." +
                            "\n\nSonuç olarak, C# ve ASP.NET Core, hem küçük ölçekli projeler hem de büyük, kurumsal uygulamalar için uygun bir teknoloji yığınıdır. Geliştiricilere hem güçlü bir araç seti hem de modern yazılım geliştirme standartlarına uygun bir ortam sunar.",
                            Image = "3.jpg",
                            Url = "csharp-asp-net-core",
                            IsActive = true,
                            PublishedDate = DateTime.Now.AddDays(-3),
                            Tags = await context.Tags.Skip(3).Take(2).ToListAsync(),
                            User = await context.Users.FirstOrDefaultAsync() ?? new User { UserName = "Default User" },
                        },
                        new Post
                        {
                            Title = "Python",
                            Content = "Python, 1990 yılında Guido van Rossum tarafından geliştirilen, yüksek seviyeli, genel amaçlı bir programlama dilidir. Kolay okunabilir sözdizimi ve geniş standart kütüphanesi sayesinde hem yeni başlayanlar hem de deneyimli yazılımcılar tarafından tercih edilir. " +
                            "\n\nPython, dinamik tip sistemi ve otomatik bellek yönetimi ile programlama sürecini kolaylaştırır. Web geliştirme, veri analitiği, makine öğrenimi, yapay zeka, bilimsel hesaplamalar, oyun geliştirme ve daha birçok alanda kullanılır. Django ve Flask gibi popüler web çerçeveleri, Python’un web geliştirme alanındaki etkinliğini artırır. " +
                            "\n\nPython, platform bağımsızdır, yani aynı kod farklı işletim sistemlerinde çalışabilir. Ayrıca, açık kaynaklı olması, geniş bir topluluk desteği sunar ve kullanıcıların sorunlarına hızlı çözümler bulmasını sağlar." +
                            "\n\nKısacası, Python, sadeliği, esnekliği ve geniş uygulama alanları ile modern yazılım dünyasının vazgeçilmez araçlarından biridir.",
                            Image = "4.jpg",
                            Url = "python",
                            IsActive = true,
                            PublishedDate = DateTime.Now.AddDays(-1),
                            Tags = await context.Tags.Skip(5).Take(1).ToListAsync(),
                            User = await context.Users.Skip(2).FirstOrDefaultAsync() ?? new User { UserName = "Default User" },
                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Text = "Harika bir yazı olmuş",
                                    PublishedDate = DateTime.Now.AddDays(-10),
                                    UserId = (await context.Users.FirstOrDefaultAsync())?.UserId ?? 0
                                },
                                new Comment
                                {
                                    Text = "Yazılarınızı büyük bir ilgiyle takip ediyorum. Harika!",
                                    PublishedDate = DateTime.Now.AddDays(-3),
                                    UserId = (await context.Users.Skip(1).FirstOrDefaultAsync())?.UserId ?? 0
                                }
                            }
                        },
                        new Post
                        {
                            Title = "Angular",
                            Content = "Angular, Google tarafından geliştirilen ve desteklenen, modern web uygulamaları oluşturmak için kullanılan bir açık kaynaklı JavaScript framework'üdür. Angular, tek sayfa uygulamaları (SPA) oluşturmayı kolaylaştırır ve büyük ölçekli uygulamalar için güçlü bir yapı sağlar." +
                            "\n\nAngular'ın güçlü özelliklerinden biri, bileşen tabanlı mimarisi sayesinde kodun daha modüler ve yeniden kullanılabilir hale gelmesidir. Ayrıca, TypeScript dili üzerine inşa edilmiş olması, yazılımın daha okunabilir ve hataların erken tespit edilebilir olmasını sağlar. " +
                            "\n\nİki yönlü veri bağlama, güçlü form yönetimi, yerleşik yönlendirme desteği ve RxJS ile reaktif programlama gibi özellikler Angular'ı öne çıkarır. Kurumsal düzeyde uygulamalardan, küçük ölçekli projelere kadar birçok alanda Angular tercih edilmektedir." +
                            "\n\nModern web geliştirme için sunduğu performans ve esneklik, Angular'ı popüler bir seçenek haline getirmiştir.",
                            Image = "6.jpg",
                            Url = "angular",
                            IsActive = true,
                            PublishedDate = DateTime.Now,
                            Tags = await context.Tags.Skip(6).Take(1).ToListAsync(),
                            User = await context.Users.Skip(3).FirstOrDefaultAsync() ?? new User { UserName = "Default User" },
                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Text = "Harika bir yazı olmuş",
                                    PublishedDate = DateTime.Now.AddDays(-10),
                                    User = await context.Users.Skip(3).FirstOrDefaultAsync() ?? new User(),
                                },
                            }
                        },
                        new Post
                        {
                            Title = "React",
                            Content = "React, Facebook tarafından geliştirilen ve desteklenen bir JavaScript kütüphanesidir. Kullanıcı arayüzleri oluşturmak için optimize edilmiş olan React, bileşen tabanlı yapısı sayesinde kodun modüler ve yeniden kullanılabilir olmasını sağlar. " +
                            "\n\nReact, 'Virtual DOM' kullanarak, kullanıcı arayüzlerindeki değişikliklerin hızlı bir şekilde işlenmesini sağlar. Bu, performansı artırır ve özellikle yüksek dinamikliğe sahip uygulamalarda avantaj sağlar. " +
                            "\n\nReact'ın en dikkat çeken özelliklerinden biri, öğrenim eğrisinin düşük olmasıdır. Ayrıca, React Native gibi yan ürünleri, mobil uygulama geliştirme için de güçlü bir çözüm sunar. Redux gibi kütüphanelerle birlikte kullanılabilmesi, büyük ölçekli projelerde veri yönetimini kolaylaştırır." +
                            "\n\nGünümüzde birçok büyük şirket, React'ı web ve mobil uygulamalarının temel teknolojisi olarak kullanmaktadır. Esneklik ve geniş topluluk desteği, React'ın güçlü bir seçenek olmasını sağlar.",
                            Image = "5.jpg",
                            Url = "react",
                            IsActive = true,
                            PublishedDate = DateTime.Now,
                            Tags = await context.Tags.Skip(7).Take(1).ToListAsync(),
                            User = await context.Users.Skip(3).FirstOrDefaultAsync() ?? new User { UserName = "Default User" },
                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                    Text = "Eline sağlık",
                                    PublishedDate = DateTime.Now.AddDays(-5),
                                    User = await context.Users.Skip(2).FirstOrDefaultAsync() ?? new User(),
                                },
                            }
                        }
                    );

                    await context.SaveChangesAsync();
                }
            }

        }
    }
}