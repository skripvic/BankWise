using Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Data.Settings;

public static class DataSeeder
{
    public static void SeedData(this ModelBuilder builder)
    {
        var denomination1 = new Denomination { Id = 1, Value = 10 };
        var denomination2 = new Denomination { Id = 2, Value = 50 };
        var denomination3 = new Denomination { Id = 3, Value = 100 };
        var denomination4 = new Denomination { Id = 4, Value = 500 };
        var denomination5 = new Denomination { Id = 5, Value = 1000 };
        var denomination6 = new Denomination { Id = 6, Value = 5000 };
        
        builder.Entity<Denomination>().HasData(denomination1, denomination2, denomination3, 
            denomination4, denomination5, denomination6);
        
        builder.Entity<Cassette>().HasData(
            new { Id = 1, Capacity = 500, CurrentCount = 50, DenominationId = denomination1.Id },
            new { Id = 2, Capacity = 1000, CurrentCount = 100, DenominationId = denomination2.Id },
            new { Id = 3, Capacity = 1000, CurrentCount = 100, DenominationId = denomination3.Id },
            new { Id = 4, Capacity = 2000, CurrentCount = 100, DenominationId = denomination4.Id },
            new { Id = 5, Capacity = 2000, CurrentCount = 100, DenominationId = denomination5.Id },
            new { Id = 6, Capacity = 1500, CurrentCount = 250, DenominationId = denomination6.Id }
        );

    }
}