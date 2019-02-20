using Loop.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private ReservationContext ReservationContext;

        public ReservationController(ReservationContext reservationContext)
        {
            ReservationContext = reservationContext;

            if (ReservationContext.Reservations.Count() == 0)
            {
                ReservationContext.Reservations.Add(new Reservation { ClientName = "Alice", Location = "Board Room" });
                ReservationContext.Reservations.Add(new Reservation { ClientName = "Bob", Location = "Lecture Hall" });
                ReservationContext.Reservations.Add(new Reservation { ClientName = "Joe", Location = "Meeting Room 1" });
                ReservationContext.Reservations.Add(new Reservation { ClientName = "Loop In Accounting", Location = "Helicopter Pad" });
                ReservationContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<Reservation>> Get() => await ReservationContext.Reservations.AsNoTracking().ToListAsync();

        [HttpGet("{id}")]
        public async Task<Reservation> Get(int id) => await ReservationContext.Reservations.FindAsync(id);

        [HttpPost]
        public async Task<Reservation> Post([FromBody] Reservation res)
        {
            await ReservationContext.Reservations.AddAsync(res);
            await ReservationContext.SaveChangesAsync();
            return await ReservationContext.Reservations.FindAsync(res.ReservationId);
        }

        [HttpPut]
        public async Task<Reservation> Put([FromBody] Reservation res)
        {
            ReservationContext.Reservations.Update(res);
            await ReservationContext.SaveChangesAsync();

            return await ReservationContext.Reservations.FindAsync(res.ReservationId);
        }

        [HttpPatch("{id}")]
        public async Task<StatusCodeResult> Patch(int id,
        [FromBody]JsonPatchDocument<Reservation> patch)
        {
            Reservation reservation = await ReservationContext.Reservations.FindAsync(id);
            if (reservation != null)
            {
                patch.ApplyTo(reservation);
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            ReservationContext.Reservations.Remove(await ReservationContext.Reservations.FindAsync(id));
            await ReservationContext.SaveChangesAsync();
        }
    }
}