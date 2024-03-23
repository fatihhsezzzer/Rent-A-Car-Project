namespace RentACar.Models.CarModels
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } 
        public string ImageUrl { get; set; }
        public decimal PricePerDay { get; set; }
        public string Transmission { get; set; } 
        public int Passengers { get; set; }
        public string FuelType { get; set; }
        public string Location { get; set; }

    }
}
