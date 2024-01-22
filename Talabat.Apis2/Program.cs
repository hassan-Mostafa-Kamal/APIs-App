using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Talabat.Apis2.ErrorsResponseHandling;
using Talabat.Apis2.Helpers;
using Talabat.Apis2.MiddleWares;
using Talabat2.core;
using Talabat2.core.Entities;
using Talabat2.core.Entities.Identity;
using Talabat2.core.Repositories;
using Talabat2.core.Services;
using Talabat2.Repository;
using Talabat2.Repository.Data;
using Talabat2.Repository.Identity;
using Talabat2.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Talabat.Apis2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region 1: Configier Services (add services that need use DJ to the container

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });

            //allow DJ for sql server business Db & for options
            builder.Services.AddDbContext<StoreContext>(options =>

            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // allow DJ for Redis Db 
            builder.Services.AddSingleton<IConnectionMultiplexer>( option =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return  ConnectionMultiplexer.Connect(connection);

            });

            #region Identity serveses



            //allow DJ for sql server Identity Db & for options
            builder.Services.AddDbContext<AppIdentityDbcontext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });


            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {              // if I want configer the Password Default Configrations
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;

            }).AddEntityFrameworkStores<AppIdentityDbcontext>();// here I add the implementions for (Create & Delete & ... user)
                                                                //.AddDefaultTokenProviders();                       // of BuiltIn IUserStore() Interface at builtIn class EntityFramWorkImplemention()


            //add Identity service to container
            builder.Services.AddAuthentication(/*"Bearer"*/ options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {     //the paramters that i will use it to insure the token is coming at request is the token i was jenirat it bafor that
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],

                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))

                    };


                });



            builder.Services.AddScoped<ITokenService, TokenService>();

            #endregion





            //builder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>();
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>,GenericRepository<ProductBrand>>();
            //builder.Services.AddScoped<IGenericRepository<ProductType>,GenericRepository<ProductType>>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<IOrderService,OrderService>();


            // builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            // builder.Services.AddScoped<ProductPictureUrlResolver>();
            builder.Services.AddAutoMapper(typeof(MappingProfiles));


            builder.Services.AddScoped<IBasketRepository,BasketRepository>();
           // builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            builder.Services.AddScoped<IPaymentService,PaymentService>();

            builder.Services.AddSingleton<IResponseCachService,ResponseCachService>();

            #region Hahdling Validation Error Response

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>

                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(e => e.ErrorMessage)
                                                        .ToArray();

                    var validationResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationResponse);
                };
            });

            #endregion


            #endregion


            var app = builder.Build();

            #region ask ClR to create Object from StoreContext & AppIdentityDbContext Explictly to updata DataBase automatically & seed Data to DataBase
                      // DataSeeding & Migrations And Update DB Should be after builder.build();
            using var scop = app.Services.CreateScope();

            var service = scop.ServiceProvider;

            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = service.GetRequiredService<StoreContext>(); // ask Explicitly

                await dbContext.Database.MigrateAsync(); //update Business Database

                await StoreContextSeed.SeedAsync(dbContext);  // seed Business Data
                                                              //=========================================================================

                var IdentityContext = service.GetRequiredService<AppIdentityDbcontext>(); //ask Explicitly

                await IdentityContext.Database.MigrateAsync(); // update Identity Database 


                var userManger = service.GetRequiredService<UserManager<AppUser>>(); // ask Explicitly to create object of userManger Class 
                                                                                     // bafore that we should add this service of Identity to container 
                await AppIdentityDbContextSeed.SeedUsersAsync(userManger);



            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during apply the migration");
            }


            #endregion



            #region after build the app we configure the app to pass throw MidelWares
            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionMiddleWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            #endregion



            app.Run();
        }
    }
}
