using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductMcService.DBContexts;
using ProductMcService.Repository;
using System.Configuration;

namespace ProductMcService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }
        public IConfiguration Configuration { get; }
        public string ConnectionString { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddDbContext<ProductContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            //});
            services.AddDbContext<ProductContext>(options =>
           options.UseMySQL(ConnectionString));
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddSwaggerGen(c =>
            {
                // c.IncludeXmlComments(XmlCommentsFilePath);
                //  c.SwaggerDoc("v1", new OpenApiInfo { Title = "SLA FIRESTONE APIs", Version = "v1" });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Product Micro Service",
                    Description = ".NET Micro Service",

                    Contact = new OpenApiContact
                    {
                        Name = "Lets Go!",
                        Email = "carolgitonga45@gmail.com"

                    }
                });

            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");


            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product MicroService");
                //    c.RoutePrefix = string.Empty;
                //});
            }
            // Configure the HTTP request pipeline.
            //if (env.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}



            //app.InitializeDatabase();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            // app.UseAuthorization();

            //app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            //app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
