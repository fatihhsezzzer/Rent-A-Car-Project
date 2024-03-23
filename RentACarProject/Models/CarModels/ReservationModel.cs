namespace RentACarProject.Models.CarModels
{
    public class ReservationModel
    {
        public decimal PricePerDay { get; set; }
        public int Days { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalInformation { get; set; }
    }


}
