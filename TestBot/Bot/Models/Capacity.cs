﻿namespace TestBot.Bot.Models
{
    public class Capacity
    {
        public Beds Beds { get; set; }

        public Capacity()
        {
            this.Beds = new Beds();
        }

        public override string ToString()
        {
            return $"Beds: {this.Beds}";
        }
    }
}
