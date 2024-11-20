using CatalogStructureAPI.Mapping;
using CatalogStructureAPI.ORM;
using CatalogStructureAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CatalogStructureAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<CatalogItemsService>();

            var app = builder.Build();

            // For Blazor WebAssembly hosting
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
