using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ArtHouse.Models
{
    [ModelMetadataType(typeof(ArtworkMetaData))]
    public class Artwork
    {
        public int ID { get; set; }

        public string Summary
        {
            get
            {
                return Name + " - " + Completed.ToShortDateString();
            }
        }

        public string Name { get; set; } = "";

        public DateTime Completed { get; set; } = DateTime.Today;

        public string Description { get; set; } = "";

        public double Value { get; set; }

        public int ArtTypeID { get; set; }

        public ArtType? ArtType { get; set; }
    }
}
