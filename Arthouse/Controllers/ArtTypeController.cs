using ArtHouse.Data;
using ArtHouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtTypeController : ControllerBase
    {
        private readonly ArtContext _context;

        public ArtTypeController(ArtContext context)
        {
            _context = context;
        }
        // GET: api/ArtType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtTypeDTO>>> GetArtTypes()
        {
            var artTypeDTOs = await _context.ArtTypes
                .Select(d => new ArtTypeDTO
                {
                    ID = d.ID,
                    Type = d.Type
                })
                .ToListAsync();

            if (artTypeDTOs.Count() > 0)
            {
                return artTypeDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Art Type records found in the database." });
            }
        }

        // GET: api/ArtType/inc - Include the Artworks Collection
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<ArtTypeDTO>>> GetArtTypesInc()
        {
            var artTypeDTOs = await _context.ArtTypes
                .Include(d => d.Artworks)
                .Select(d => new ArtTypeDTO
                {
                    ID = d.ID,
                    Type = d.Type,
                    Artworks = d.Artworks.Select(dArtwork => new ArtworkDTO
                    {
                        ID = dArtwork.ID,
                        Name = dArtwork.Name,
                        Completed = dArtwork.Completed,
                        Description = dArtwork.Description,
                        Value = dArtwork.Value,
                        ArtTypeID = dArtwork.ArtTypeID
                    }).ToList()
                })
                .ToListAsync();

            if (artTypeDTOs.Count() > 0)
            {
                return artTypeDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Art Type records found in the database." });
            }
        }


        // GET: api/ArtType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtTypeDTO>> GetArtType(int id)
        {
            var artTypeDTO = await _context.ArtTypes
                .Select(d => new ArtTypeDTO
                {
                    ID = d.ID,
                    Type = d.Type
                })
                .FirstOrDefaultAsync(d => d.ID == id);

            if (artTypeDTO == null)
            {
                return NotFound(new { message = "Error: That Art Type was not found in the database." });
            }

            return artTypeDTO;
        }

        // GET: api/ArtType/inc/5
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<ArtTypeDTO>> GetArtTypeInc(int id)
        {
            var artTypeDTO = await _context.ArtTypes
                .Select(d => new ArtTypeDTO
                {
                    ID = d.ID,
                    Type = d.Type,
                    Artworks = d.Artworks.Select(dArtwork => new ArtworkDTO
                    {
                        ID = dArtwork.ID,
                        Name = dArtwork.Name,
                        Completed = dArtwork.Completed,
                        Description = dArtwork.Description,
                        Value = dArtwork.Value,
                        ArtTypeID = dArtwork.ArtTypeID
                    }).ToList()
                })
                .FirstOrDefaultAsync(d => d.ID == id);

            if (artTypeDTO == null)
            {
                return NotFound(new { message = "Error: That Art Type was not found in the database." });
            }

            return artTypeDTO;
        }

        private bool ArtTypeExists(int id)
        {
            return _context.ArtTypes.Any(e => e.ID == id);
        }
    }
}
