namespace CSTS.API.ApiServices
{
    public static class CstsConfigureServices
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

        }


    }
}
