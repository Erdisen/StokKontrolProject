using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StokKontrolProject.Repositories.Abstract;
using StokKontrolProject.Repositories.Concrete;
using StokKontrolProject.Repositories.Context;
using StokKontrolProject.Service.Abstract;
using StokKontrolProject.Service.Concrete;

namespace StokKontrolProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option=> option.SerializerSettings.ReferenceLoopHandling=ReferenceLoopHandling.Ignore);
           

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StokKontrolContext>(option =>
            {
                option.UseSqlServer("Server=DESKTOP-JH0OB08; Database=StokKontrolDB; uid = sa; pwd=1;");
            });


            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
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