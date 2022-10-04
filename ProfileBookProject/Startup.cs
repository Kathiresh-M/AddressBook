using AutoMapper;
using Contracts;
using Entities;
using Entities.Dto;
using Entities.Models;
using Entities.AddressBookProfiles;
using Microsoft.EntityFrameworkCore;
using Repository;
using Services;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProfileBookProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            services.AddSingleton<IJWTManagerServices, JWTManagerServices>();



            services.AddDbContext<BookRepository>(
               options => options.UseSqlServer(Configuration.GetConnectionString("AddressBookDatabase")));
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddScoped<IAddressBookServices, AddressBookServices>();
            services.AddScoped<IAuthenticateBookServices, AuthenticateBookServices>();
            services.AddAutoMapper(typeof(Startup));
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AddressBookProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });
        }
    }
}
