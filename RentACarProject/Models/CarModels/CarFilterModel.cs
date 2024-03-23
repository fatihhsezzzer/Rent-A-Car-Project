namespace RentACarProject.Models.CarModels
{
    public class CarFilterModel
    {
        public decimal? MaxPrice { get; set; }
        public string Location { get; set; }
        public DateTime? PickUpDate { get; set; }
        public int? Passengers { get; set; }
        public string FuelType { get; set; }

        // Diğer filtreleme alanları...
    }

}
