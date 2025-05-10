namespace IonosLedWebMvc.Ver2.Models.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int LampModelId { get; set; }
    }
}
