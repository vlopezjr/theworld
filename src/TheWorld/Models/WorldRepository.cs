using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;

        public WorldRepository(WorldContext context)
        {
            _context = context;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void AddTrip(Trip newTrip)
        {
            _context.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.OrderBy(p => p.Name).ToList();
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            return _context.Trips
                .Include(t => t.Stops)
                .OrderBy(t => t.Name)
                .ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var trip = GetTripByName(tripName);
            newStop.Order = trip.Stops.Max(s => s.Order) + 1;
            _context.Stops.Add(newStop);
        }
    }
}
