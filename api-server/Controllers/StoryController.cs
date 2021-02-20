using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sheep.IHeartFiction.ApiServer.Models;

namespace Sheep.IHeartFiction.ApiServer.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IStore<Story> _store;

        public StoryController(IStore<Story> store)
        {
            _store = store;
        }

        /// <summary>
        /// Creates a new Story.
        /// </summary>
        /// <param name="story">The story to place.</param>
        /// <returns>The created story.</returns>
        /// <response code="201">The story was successfully placed.</response>
        /// <response code="400">The story is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Story), 201)]
        [ProducesResponseType(400)]
        public IActionResult Create([FromBody] Story story)
        {
            _store.Create(story);
            return CreatedAtAction(nameof(GetByGuid), new { id = story.UUID }, story);
        }

        /// <summary>
        /// Retrieves all stories.
        /// </summary>
        /// <returns>All available stories.</returns>
        /// <response code="200">The successfully retrieved stories.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Story>), 200)]
        public IActionResult Read() => Ok(_store.Read());

        /// <summary>
        /// Gets a single story.
        /// </summary>
        /// <param name="id">The requested story identifier.</param>
        /// <returns>The requested story.</returns>
        /// <response code="200">The story was successfully retrieved.</response>
        /// <response code="404">The story does not exist.</response>
        [HttpGet("guid/{id:guid}")]
        [ProducesResponseType(typeof(Story), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetByGuid(Guid id)
        {
            var result = _store.Read(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Gets a single story.
        /// </summary>
        /// <param name="id">The requested story identifier.</param>
        /// <returns>The requested story.</returns>
        /// <response code="200">The story was successfully retrieved.</response>
        /// <response code="404">The story does not exist.</response>
        [HttpGet("id/{id:int}")]
        [ProducesResponseType(typeof(Story), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            var result = _store.Read(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Update a story.
        /// </summary>
        /// <param name="story">The requested story to update.</param>
        /// <returns>The updated story.</returns>
        /// <response code="202">The story was successfully updated.</response>
        /// <response code="404">The story does not exist.</response>
        [HttpPut]
        [ProducesResponseType(typeof(Story), 202)]
        [ProducesResponseType(404)]
        public IActionResult Update([FromBody] Story story)
        {
            var updatedEntity = _store.Update(story);
            if (updatedEntity == null) return NotFound();
            return AcceptedAtAction("Read", new { id = updatedEntity.UUID });
        }

        /// <summary>
        /// Deletes a Story.
        /// </summary>
        /// <param name="id">The identifier of the story to delete.</param>
        /// <returns>No Content</returns>
        /// <response code="204">The story was successfully deleted.</response>
        /// <response code="404">The story does not exist.</response>
        [HttpDelete("guid/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id) => _store.Delete(id) ? NoContent() : NotFound();

        /// <summary>
        /// Deletes a Story.
        /// </summary>
        /// <param name="id">The identifier of the story to delete.</param>
        /// <returns>No Content</returns>
        /// <response code="204">The story was successfully deleted.</response>
        /// <response code="404">The story does not exist.</response>
        [HttpDelete("id/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id) => _store.Delete(id) ? NoContent() : NotFound();
    }
}
