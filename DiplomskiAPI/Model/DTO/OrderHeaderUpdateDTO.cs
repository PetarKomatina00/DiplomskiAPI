namespace DiplomskiAPI.Model.DTO
{
    public class OrderHeaderUpdateDTO
    {
        public int OrderHeaderID { get; set; }
        public string PickupName { get; set; }
        public string PickupPhoneNumber { get; set; }
        public string PickupEmail { get; set; }

        public string StripePaymentID { get; set; }
        public string Status { get; set; }
    }
}
