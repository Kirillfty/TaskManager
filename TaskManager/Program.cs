
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Repository;


namespace TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            string conn = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<ApplicationDB>(options => options.UseNpgsql(conn));
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ITaskRepository, TaskRepository>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            IConfigurationSection authConfiguration = builder.Configuration.GetSection("AuthOptions");
            AuthOptions authOptions = new AuthOptions(
                authConfiguration["ISSUER"] ?? throw new Exception("ISSUER is null!"),
                authConfiguration["AUDIENCE"] ?? throw new Exception("AUDIENCE is null!"),
                authConfiguration["KEY"]) ?? throw new Exception("KEY is null!");
            builder.Services.AddSingleton<AuthOptions>(authOptions);
            builder.Services.AddTransient<JwtCreator>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = authOptions.GetSymmetricKey,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));


            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDB>();
                db.Database.Migrate();
            }
            
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
                
            }


            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
