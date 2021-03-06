﻿using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }
        //once you have added a dbset create a migration in terminal window(dont have API server running)
        //creates a new table in database
        //dotnet ef migration add "ActivityEntity" -p Persistence/ -s API/

        public DbSet<Value> Values { get; set; }

        public DbSet<Activity> Activities { get; set; }


        //entityframework configuration so that we could add data into migrations class
        //annoying as you have tomanually creat ids
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Value>()
                .HasData(
                    new Value {Id = 1, Name = "Value 101"},
                    new Value {Id = 2, Name = "Value 101"},
                    new Value {Id = 3, Name = "Value 103"}
                );
        } 
    }
}
