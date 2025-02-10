using ArtHouse.Data;
using ArtHouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworkController : ControllerBase
    {
        private readonly ArtContext _context;

        public ArtworkController(ArtContext context)
        {
            _context = context;
        }

        // GET: api/Artwork
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtworkDTO>>> GetArtworks()
        {
            var artworkDTOs = await _context.Artworks
                .Include(a => a.ArtType)
                .Select(a=> new ArtworkDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Completed = a.Completed,
                    Description = a.Description,
                    Value = a.Value,
                    ArtTypeID = a.ArtTypeID,
                    ArtType = a.ArtType != null ? new ArtTypeDTO
                    {
                        ID = a.ArtType.ID,
                        Type = a.ArtType.Type
                    } : null
                })
                .ToListAsync();

            if (artworkDTOs.Count > 0)
            {
                return artworkDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Artworks found in the database." });
            }

        }

        // GET: api/Artwork/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtworkDTO>> GetArtwork(int id)
        {
            var artworkDTO = await _context.Artworks
                .Include(a => a.ArtType)
                .Select(a => new ArtworkDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Completed = a.Completed,
                    Description = a.Description,
                    Value = a.Value,
                    ArtTypeID = a.ArtTypeID,
                    ArtType = a.ArtType != null ? new ArtTypeDTO
                    {
                        ID = a.ArtType.ID,
                        Type = a.ArtType.Type
                    } : null
                })
                .FirstOrDefaultAsync(a => a.ID == id);

            if (artworkDTO == null)
            {
                return NotFound(new { message = "Error: That Artwork was not found in the database." });
            }

            return artworkDTO;
        }

        // GET: api/ArtworksByArtType
        [HttpGet("ByArtType/{id}")]
        public async Task<ActionResult<IEnumerable<ArtworkDTO>>> GetArtworksByArtType(int id)
        {
            var artworkDTOs = await _context.Artworks
                .Include(a => a.ArtType)
                .Where(a=>a.ArtTypeID==id)
                .Select(a => new ArtworkDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Completed = a.Completed,
                    Description = a.Description,
                    Value = a.Value,
                    ArtTypeID = a.ArtTypeID,
                    ArtType = a.ArtType != null ? new ArtTypeDTO
                    {
                        ID = a.ArtType.ID,
                        Type = a.ArtType.Type
                    } : null
                })
                .ToListAsync();

            if (artworkDTOs.Count > 0)
            {
                return artworkDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Artworks of the type." });
            }
        }

        // PUT: api/Artwork/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtwork(int id, ArtworkDTO artworkDTO)
        {
            if (id != artworkDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Artwork" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var artworkToUpdate = await _context.Artworks.FindAsync(id);

            //Check that you got it
            if (artworkToUpdate == null)
            {
                return NotFound(new { message = "Error: Artwork record not found." });
            }

            artworkToUpdate.ID = artworkDTO.ID;
            artworkToUpdate.Name = artworkDTO.Name;
            artworkToUpdate.Completed = artworkDTO.Completed;
            artworkToUpdate.Description = artworkDTO.Description;
            artworkToUpdate.Value = artworkDTO.Value;
            artworkToUpdate.ArtTypeID = artworkDTO.ArtTypeID;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                //True we are not checking for concurrency, but plan ahead.
                if (!ArtworkExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Artwork has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Artwork has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Artwork (Name, Art Type and Date Completed)." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Artwork
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Artwork>> PostArtwork(ArtworkDTO artworkDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Artwork artwork = new Artwork
            {
                ID = artworkDTO.ID,
                Name = artworkDTO.Name,
                Completed = artworkDTO.Completed,
                Description = artworkDTO.Description,
                Value = artworkDTO.Value,
                ArtTypeID = artworkDTO.ArtTypeID
            };

            try
            {
                _context.Artworks.Add(artwork);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                artworkDTO.ID = artwork.ID;

                return CreatedAtAction(nameof(GetArtwork), new { id = artwork.ID }, artworkDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Artwork (Name, Art Type and Date Completed)." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Artwork/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtwork(int id)
        {
            var artwork = await _context.Artworks.FindAsync(id);

            if (artwork == null)
            {
                return NotFound(new { message = "Delete Error: Artwork has already been removed." });
            }

            try
            {
                _context.Artworks.Remove(artwork);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Artwork." });
            }
        }
        private bool ArtworkExists(int id)
        {
            return _context.Artworks.Any(e => e.ID == id);
        }
    }
}
