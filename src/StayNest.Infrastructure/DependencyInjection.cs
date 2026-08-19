using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StayNest.Domain.Listings.Interface;
using StayNest.Domain.Reservations.Interface;
using StayNest.Domain.Shared;
using StayNest.Domain.Users.Interface;
using StayNest.Infrastructure.Repositories;

namespace StayNest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        AddPersistence(services, configuration);

        return services;
    }

    public static void AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString =
            configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IListingRepository, ListingRepository>();

        services.AddScoped<IReservationRepository, ReservationRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }
}
