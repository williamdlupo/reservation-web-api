using Microsoft.EntityFrameworkCore;
using System;

namespace Loop.Api.Models
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options)
            : base(options)
        { }

        public DbSet<Reservation> Reservations { get; set; }
    }

    public class Reservation
    {
        public int ReservationId { get; set; }
        public string ClientName { get; set; }
        public string Location { get; set; }        
    }
}
