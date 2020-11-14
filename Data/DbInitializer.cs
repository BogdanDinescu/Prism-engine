using System.Linq;
namespace Prism.Data
{
    public class DbInitializer
    {
        public static void Initialize(DatabaseCtx context)
        {
            context.Database.EnsureCreated();

            /*if (context.Users.Any())
            {
                return;
            }

            context.Users.Add(new User { Email = "gigel@gmail.com", Name = "Gigel", Password = "aaaaaaA1" });
            context.SaveChanges();*/
        }
    }
}
