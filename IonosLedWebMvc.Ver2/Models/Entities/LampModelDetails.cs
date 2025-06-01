namespace IonosLedWebMvc.Ver2.Models.Entities
{
    public class LampModelDetails
    {
        public int Id { get; set; }
        public int LampModelId { get; set; }
        public string? Note { get; set; } = null!;
        public string? ImageName { get; set; } = null!;
        public string? FileNames { get; set; } = null!; // paths to files with delimetr ';'

        public LampModelDetails()
        {
            
        }

        public LampModelDetails(int modelId, string note = "", string imgName = "", string fileNames = "")
        {
            LampModelId = modelId;
            ImageName = imgName;
            FileNames = fileNames;
            Note = note;
        }
    }
}
