using System;

namespace Domain
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string city { get; set; }
        public string Venue { get; set; }
    }
}

//use entity framework to create a table in sqlite database for new entity
//go into datacontext and add a dbset of activity

//city does not have a capital letter