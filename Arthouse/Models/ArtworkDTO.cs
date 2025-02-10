using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ArtHouse.Models
{
    [ModelMetadataType(typeof(ArtworkMetaData))]
    public class ArtworkDTO
    {
        public int ID { get; set; }

        public string Name { get; set; } = "";

        public DateTime Completed { get; set; }

        public string Description { get; set; } = "";

        public double Value { get; set; }

        public int ArtTypeID { get; set; }

        public ArtTypeDTO? ArtType { get; set; }
    }
}
