using EF;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        // using (var dbContext = new ChatContext());
        var optionsBuilder = new DbContextOptionsBuilder<ChatContext>()
            .UseSqlServer("Server=localhost; Database=GB; Trusted_Connection=True;")
            .UseLazyLoadingProxies();
    }
}