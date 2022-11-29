using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Personal_Finance_Management_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Personal_Finance_Management_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            StartupHelper.RegisterDbContext(services, Configuration);
            RegisterCustomTypesInIoC(services);
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void RegisterCustomTypesInIoC(IServiceCollection services)
        {
            var asm = Assembly.Load("Core");
            var classes =
                asm.GetTypes().Where(p =>
                    p.Namespace != null && p.Namespace.StartsWith("Core.Entities") &&
                    p.IsClass
                && (p.IsSubclassOf(typeof(BaseEntity)))

                ).ToList();
            foreach (var c in classes)
            {
                // GenericRepository registration
                var iGenericRepositoryType = typeof(IGenericRepository<>).MakeGenericType(c);
                var genericRepositoryType = typeof(GenericEFRepository<>).MakeGenericType(c);
                services.AddScoped(iGenericRepositoryType, genericRepositoryType);

                // GenericService registration
                var iGenericServiceType = typeof(IGenericService<>).MakeGenericType(c);
                var genericServiceType = typeof(GenericService<>).MakeGenericType(c);
                services.AddScoped(iGenericServiceType, genericServiceType);
            }

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IMccCodeService, MccCodeService>();
            services.AddTransient<ITransactionService, TransactionService>();
        }

    }
}
