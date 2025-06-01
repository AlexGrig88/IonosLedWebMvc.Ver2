namespace IonosLedWebMvc.Ver2.Models
{
    public class FileRecordForView
    {
        public FileRecordForView(string? fileName, string? extension, string? size)
        {
            FileName = fileName;
            Extension = extension;
            Size = size;
        }

        public string? FileName { get; set; }
        public string? Extension { get; set; }
        public string? Size { get; set; }
    }
}
