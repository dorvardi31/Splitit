using Microsoft.AspNetCore.Mvc;
using ImdbScraperApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ImdbScraperApi.Data;
using ImdbScraperApi.Services;

namespace ImdbScraperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ActorServices _actorServices;

        public ActorsController(ActorServices actorServices)
        {
            _actorServices = actorServices;
        }

        /// <summary>
        /// Retrieves actors filtered by Name and optional parameters.
        /// </summary>
        /// <param name="name">Not Optional. The name to filter actors by.</param>
        /// <param name="rankStart">Optional. The minimum rank to filter actors by.</param>
        /// <param name="rankEnd">Optional. The maximum rank to filter actors by.</param>
        /// <param name="source">Optional. The source to filter actors by.</param>
        /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
        /// <param name="pageSize">The number of records per page for pagination. Default is 10.</param>
        /// <returns>A list of actors matching the criteria.</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Actor>>>> GetActors(
           [FromQuery] string name,
           [FromQuery] int? rankStart,
           [FromQuery] int? rankEnd,
           [FromQuery] string? source,
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10)
        {
            var actors = await _actorServices.GetAllActorsAsync(name, rankStart, rankEnd, source, pageNumber, pageSize);
            return Ok(new ApiResponse<IEnumerable<Actor>>(200, "Actors fetched successfully.", actors));
        }

        /// <summary>
        /// Retrieves a specific actor by ID.
        /// </summary>
        /// <param name="id">The ID of the actor to retrieve.</param>
        /// <returns>The actor with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Actor>>> GetActor(int id)
        {
            var actor = await _actorServices.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound(new ApiResponse<Actor>(204, $"Actor not found for id '{id}'"));
            }
            return Ok(new ApiResponse<Actor>(200, "Actor fetched successfully.", actor));
        }

        /// <summary>
        /// Adds a new actor.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Actor>>> AddActor(Actor actor)
        {
            var existingActors = await _actorServices.GetAllActorsAsync(actor.Name, null, null, actor.Source, 1, 1);
            if (existingActors.Count > 0)
            {
                return Conflict(new ApiResponse<Actor>(209, $"An actor with the name '{actor.Name}' and source '{actor.Source}' already exists."));
            }
            var newActor = await _actorServices.AddActorAsync(actor);
            return CreatedAtAction(nameof(GetActor), new { id = newActor.Id }, new ApiResponse<Actor>(200, "Actor added successfully", newActor));
        }

        /// <summary>
        /// Updates an existing actor.
        /// </summary>
        /// <param name="id">The ID of the actor to update.</param>
        /// <param name="actor">The updated actor information.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Actor>>> UpdateActor(int id, Actor actor)
        {
            var updatedActor = await _actorServices.UpdateActorAsync(id, actor);
            if (updatedActor == null)
            {
                return NotFound(new ApiResponse<Actor>(204, "Actor not found"));
            }
            return Ok(new ApiResponse<Actor>(200, "Actor updated successfully", actor));
        }

        /// <summary>
        /// Deletes an actor by ID.
        /// </summary>
        /// <param name="id">The ID of the actor to delete.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var result = await _actorServices.DeleteActorAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse<Actor>(204, $"Actor not found by id ' { id}'"));
            }
            return Ok(new ApiResponse<Actor>(200, "Actor deleted successfully"));
        }
    }
}
