using System;

namespace CAP.API.Models
{
    // Don't forget to add this to the DbContext class:
    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>().UseTimestampedProperty();
    }
    */
    public class WeatherForecast : TimeStamp
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
