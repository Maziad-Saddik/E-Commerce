using Interceptors;

namespace Extensions
{
    public static class GrpcRegisterExtension
    {
        public static void AddGrpcWithValidators(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc(options =>
            {
                // options.EnableMessageValidation();

                options.Interceptors.Add<HandleErrorInterceptor>();
            });

            AddValidators(services);
        }

        private static void AddValidators(IServiceCollection services)
        {
            //services.AddGrpcValidation();

            //services.AddValidator<>();
        }
    }
}
