using ArtHouse.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ArtHouse.Data
{
    public static class ArtInitializer
    {
        /// <summary>
        /// Prepares the Database and seeds data as required
        /// </summary>
        /// <param name="serviceProvider">DI Container</param>
        /// <param name="DeleteDatabase">Delete the database and start from scratch</param>
        /// <param name="UseMigrations">Use Migrations or EnsureCreated</param>
        /// <param name="SeedSampleData">Add optional sample data</param>
        public static void Initialize(IServiceProvider serviceProvider,
            bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new ArtContext(
                serviceProvider.GetRequiredService<DbContextOptions<ArtContext>>()))
            {
                //Refresh the database as per the parameter options
                #region Prepare the Database
                try
                {
                    //Note: .CanConnect() will return false if the database is not there!
                    if (DeleteDatabase || !context.Database.CanConnect())
                    {
                        context.Database.EnsureDeleted(); //Delete the existing version 
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Create the Database and apply all migrations
                        }
                        else
                        {
                            context.Database.EnsureCreated(); //Create and update the database as per the Model
                        }
                        //Now create any additional database objects such as Triggers or Views
                        //--------------------------------------------------------------------

                    }
                    else //The database is already created
                    {
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Apply all migrations
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion

                //Seed data needed for production and during development
                #region Seed Required Data
                try
                {
                    if (!context.ArtTypes.Any())
                    {
                        context.ArtTypes.AddRange(
                         new ArtType
                         {
                             Type = "Painting"
                         },
                         new ArtType
                         {
                             Type = "Drawing"
                         },
                         new ArtType
                         {
                             Type = "Sculpture"
                         },
                         new ArtType
                         {
                             Type = "Plastic Art"
                         },
                         new ArtType
                         {
                             Type = "Decorative Art"
                         },
                         new ArtType
                         {
                             Type = "Visual Art"
                         }
                       );
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion

                //Seed meaningless data as sample data during development
                #region Seed Sample Data 
                if (SeedSampleData)
                {
                    //To randomly generate data
                    Random random = new Random();

                    //Seed a few specific Artworks
                    try
                    {
                        if (!context.Artworks.Any())
                        {
                            context.Artworks.AddRange(
                             new Artwork
                             {
                                 Name = "Red Dot",
                                 Value = 12000d,
                                 Completed = DateTime.Parse("2002-06-06"),
                                 Description = "Painting of a large Red Dot on a white backgraound.",
                                 ArtTypeID = context.ArtTypes.Where(d => d.Type == "Painting").FirstOrDefault().ID
                             },
                             new Artwork
                             {
                                 Name = "Rossini Regal",
                                 Value = 99000d,
                                 Completed = DateTime.Parse("2009-12-06"),
                                 Description = "Photograph of a regal horse.",
                                 ArtTypeID = context.ArtTypes.Where(d => d.Type == "Visual Art").FirstOrDefault().ID
                             },
                             new Artwork
                             {
                                 Name = "Love Sublime",
                                 Value = 19.99d,
                                 Completed = DateTime.Parse("2015-09-21"),
                                 Description = "Soapstone Sculpture of woman's face gazing at an unknown figure.",
                                 ArtTypeID = context.ArtTypes.Where(d => d.Type == "Sculpture").FirstOrDefault().ID
                             },
                             new Artwork
                             {
                                 Name = "Igor Smashes",
                                 Value = 750000.50d,
                                 Completed = DateTime.Parse("1976-07-11"),
                                 Description = "Abstract concept of smashed emotion done in crumpled paper.",
                                 ArtTypeID = context.ArtTypes.Where(d => d.Type == "Plastic Art").FirstOrDefault().ID
                             });
                            context.SaveChanges();
                        }


                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.GetBaseException().Message);
                    }

                }
                #endregion
            }
        }
    }
}
