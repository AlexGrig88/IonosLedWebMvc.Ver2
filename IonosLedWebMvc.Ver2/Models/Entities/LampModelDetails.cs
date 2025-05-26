namespace IonosLedWebMvc.Ver2.Models.Entities
{
    public class LampModelDetails
    {
        public int Id { get; set; }
        public int LampModelId { get; set; }
        public string? Note { get; set; } = null!;
        public string? ImageName { get; set; } = null!;
        public string? FileName { get; set; } = null!;

        public LampModelDetails()
        {
            
        }

        public LampModelDetails(int modelId, string note = "", string imgName = "", string filename = "")
        {
            LampModelId = modelId;
            ImageName = imgName;
            FileName = filename;
            Note = note;
        }
    }
}
