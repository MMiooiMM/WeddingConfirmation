namespace WeddingConfirmation.Data
{
    public class Confirmation
    {
        public string Name { get; set; } = "";
        public string? Relationship;
        public string Joining { get; set; } = Constraints.Joining.Yes;
        public int? NumberOfPeople;
        public int NumberOfChildSeat { get; set; } = 0;
        public int NumberOfVegetarian { get; set; } = 0;
        public string Send { get; set; } = Constraints.Send.Email;
        public string Email { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Blessing { get; set; } = "";
    }
}