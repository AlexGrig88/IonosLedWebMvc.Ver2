namespace IonosLedWebMvc.Ver2.Models
{
    public class FileRecordForView
    {
        public FileRecordForView(string? fileName, string? extension, string? size, string? fullPath)
        {
            FileName = fileName;
            Extension = extension;
            Size = size;
            FullPath = fullPath;
        }

        public string? FileName { get; set; }
        public string? Extension { get; set; }
        public string? Size { get; set; }
        public string? FullPath { get; set; }
    }
}
