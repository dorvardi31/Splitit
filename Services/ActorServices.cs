
//functions that used for the controller 
using ImdbScraperApi.Models;

using ImdbScraperApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImdbScraperApi.Services
{
    public class ActorServices
    {
        private readonly AppDbContext _context;

        public ActorServices(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Actor>> GetAllActorsAsync(string name, int? rankStart, int? rankEnd, string? source, int pageNumber, int pageSize)
        {
            var query = _context.Actors.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => EF.Functions.Like(a.Name, $"%{name}%"));
            }
            if (rankStart.HasValue)
            {
                query = query.Where(a => a.Rank >= rankStart.Value);
            }
            if (rankEnd.HasValue)
            {
                query = query.Where(a => a.Rank <= rankEnd.Value);
            }
            if (!string.IsNullOrEmpty(source))
            {
                query = query.Where(a => EF.Functions.Like(a.Source, source));
            }

            var totalCount = await query.CountAsync(); 
            Console.WriteLine($"Total matching actors before pagination: {totalCount}");

            var actors = await query
                .OrderBy(a => a.Id) // Ensure there's an order when applying Skip/Take
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Console.WriteLine($"Actors fetched: {actors.Count}"); 
            return actors;
        }


        public async Task<Actor> GetActorByIdAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return null ;
            }

            return actor;
        }

        public async Task<bool> CreateActorAsync(Actor actor)
        {
            if (_context.Actors.Any(a => a.Rank == actor.Rank))
            {
                return false;
            }

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateActorAsync(int id, Actor actor)
        {
            if (id != actor.Id)
            {
                return false;
            }

            if (_context.Actors.Any(a => a.Rank == actor.Rank && a.Id != id))
            {
                return false;
            }

            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }


        public async Task<Actor> AddActorAsync(Actor actorDto)
        {
            var actor = new Actor
            {

                Name = actorDto.Name,
                Description = actorDto.Description,
                Source = actorDto.Source,
                Type = actorDto.Type,
                Rank = actorDto.Rank
            };

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;
        }



        public async Task<bool> DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return false;

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IsRankUniqueAsync(int? id, int rank, string source)
        {
            if (id.HasValue)
            {
                // Updating an actor: Ensure no other actor has the same rank and source
                return !await _context.Actors.AnyAsync(a => a.Id != id.Value && a.Rank == rank && a.Source == source);
            }
            else
            {
                // Adding a new actor: Ensure no actor has the same rank and source
                return !await _context.Actors.AnyAsync(a => a.Rank == rank && a.Source == source);
            }
        }

    }
}
