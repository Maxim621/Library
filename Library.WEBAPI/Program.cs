using Library.DAL;
using Microsoft.EntityFrameworkCore;

namespace Library.WEBAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<LibraryContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Library")));

            builder.Services.AddControllers();

            var app = builder.Build();


            app.MapControllers();

            app.Run();
        }
    }
}