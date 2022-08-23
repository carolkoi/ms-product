using Microsoft.EntityFrameworkCore;

namespace ProductMcService.DBContexts
{
    public class DbInitializer
    {
        public static void Initialize(ProductContext context)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }
    }
}
