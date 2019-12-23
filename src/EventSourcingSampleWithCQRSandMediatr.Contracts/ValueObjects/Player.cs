namespace EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects
{
    public class Player
    {
        public int JerseyNumber { get; set; }
        public string Name { get; set; }
        public Positions Position { get; set; }
    }
}
