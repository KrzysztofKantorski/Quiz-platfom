namespace WebApplication1.Extensions
{
    public static class CORS
    {

        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("http://localhost:5216", "http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            return services;

        }
    }
}
