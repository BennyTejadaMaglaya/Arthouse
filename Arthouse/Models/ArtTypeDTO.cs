using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArtHouse.Models
{
    [ModelMetadataType(typeof(ArtTypeMetaData))]
    public class ArtTypeDTO
    {
        public int ID { get; set; }

        public string Type { get; set; } = "";

        public ICollection<ArtworkDTO>? Artworks { get; set; }

    }
}
