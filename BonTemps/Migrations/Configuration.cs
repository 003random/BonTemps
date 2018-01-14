using BonTemps.Models;

namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BonTemps.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BonTemps.Models.ApplicationDbContext context)
        {
            for (var i = 1; i < 10; i++)//loop 9 times
            {
                for (var j = 1; j < 10; j++)//loop 9 times
                {
                    context.Table_layout.AddOrUpdate(
                        new Table_layout { LayoutX = i, LayoutY = j, IsTable = true}
                    );
                }
            }

        }
    }
}
