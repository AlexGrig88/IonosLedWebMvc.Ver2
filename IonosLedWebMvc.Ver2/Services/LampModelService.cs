using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IonosLedWebMvc.Ver2.Services
{
    public class LampModelService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationContext _context;
        private string _wwwRootPath;
        private string storageFilesPath;

        public LampModelService(IWebHostEnvironment hostEnvironment, ApplicationContext context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _wwwRootPath = _hostEnvironment.WebRootPath;
            storageFilesPath = System.IO.Path.Combine(_wwwRootPath, "storage_lamp_model_files");
        }

        public async Task<string> SaveOrChangeImageFile(IFormFile imageFile, uint id)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = System.IO.Path.GetExtension(imageFile.FileName);
            fileName = $"Model{id}" + extension;
            string path = System.IO.Path.Combine(_wwwRootPath + "/images/lamp_models/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create)) {
                await imageFile.CopyToAsync(fileStream);
            }
            var lampModelDetail = _context.LampModelDetails.FirstOrDefault(m => m.LampModelId == (int)id);
            if (lampModelDetail == null) {
                _context.LampModelDetails.Add(new LampModelDetails(modelId: (int)id, imgName: fileName));
                await _context.SaveChangesAsync();
            }
            else {
                lampModelDetail.ImageName = fileName;
                _context.LampModelDetails.Update(lampModelDetail);
                await _context.SaveChangesAsync();
            }
            return "Файл изображения загружен успешно";
        }

        public async Task<string> AddNoteToDb(string noteArea, int id)
        {
            string newNote = $"{DateTime.Now:d}: {noteArea}\n";
            var lampModelDetail = _context.LampModelDetails.FirstOrDefault(m => m.LampModelId == id);
            if (lampModelDetail == null) {
                _context.LampModelDetails.Add(new LampModelDetails(modelId: id, note: newNote));
                await _context.SaveChangesAsync();
            }
            else {
                lampModelDetail.Note = newNote + lampModelDetail.Note;
                _context.LampModelDetails.Update(lampModelDetail);
                await _context.SaveChangesAsync();
            }
            return "Ok";
        }

        public async Task<string> SaveFiles(List<IFormFile> files, uint id)
        {
  
            string directoryName = System.IO.Path.Combine(storageFilesPath, $"Model{id}");
            if (!Directory.Exists(directoryName)) {
                Directory.CreateDirectory(directoryName);
            }
            foreach (var formFile in files) {
                if (formFile.Length > 0) {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(formFile.FileName);
                    string extension = System.IO.Path.GetExtension(formFile.FileName);
                    string fullFileName = fileName + extension;
                    string filePath = System.IO.Path.Combine(directoryName, fullFileName);

                    // работа с бд.
                    var lampModelDetail = _context.LampModelDetails.FirstOrDefault(m => m.LampModelId == id);
                    // Файл в бд записывается как название файла и через запятую его размер
                    string fileNameRecordToDB = $"{fullFileName},{formFile.Length};"; 

                    if (lampModelDetail == null) {
                        _context.LampModelDetails.Add(new LampModelDetails(modelId: (int)id, fileNames: fileNameRecordToDB));
                        await _context.SaveChangesAsync();
                    }
                    else {
                        if (string.IsNullOrEmpty(lampModelDetail.FileNames)) {
                            lampModelDetail.FileNames = fileNameRecordToDB;
                        }
                        else if (!lampModelDetail.FileNames.Contains(fileNameRecordToDB)) {
                            lampModelDetail.FileNames += fileNameRecordToDB;
                        }
                        _context.LampModelDetails.Update(lampModelDetail);
                        await _context.SaveChangesAsync();
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return "Файлы загружены успешно";
        }

        public async Task<string?> GetAllFilesByModelId(uint? id)
        {
            var lampModelDetail = await _context.LampModelDetails.FirstOrDefaultAsync(m => m.LampModelId == id);
            return lampModelDetail?.FileNames; 
        }

        public List<FileRecordForView> ParseFileRecordDB(string? filesWithSizesAsStr)
        {
            List<FileRecordForView> fileRecords = new List<FileRecordForView>();
            if (filesWithSizesAsStr != null) {
                string[] files = filesWithSizesAsStr.Split(';');
                foreach (var f in files) {
                    if (!string.IsNullOrWhiteSpace(f)) {
                        var fNameAndSize = f.Split(',');
                        var ext = fNameAndSize[0].Split('.')[1];
                        fileRecords.Add(new FileRecordForView(fNameAndSize[0], GetIconViewExtension($".{ext}"), GetSizeViewFile(long.Parse(fNameAndSize[1]))));
                    }
                }
            }
            return fileRecords;
        }

        private string GetIconViewExtension(string ext) =>
            ext switch
            {
                ".txt" => "bi bi-filetype-txt icon-big",
                ".pdf" => "bi bi-file-pdf icon-big",
                ".png" => "bi bi-file-image icon-big",
                ".jpg" => "bi bi-file-image icon-big",
                ".jpeg" => "bi bi-file-image icon-big",
                ".doc" => "bi bi-filetype-doc icon-big",
                ".docx" => "bi bi-filetype-docx icon-big",
                ".csv" => "bi bi-filetype-csv icon-big",
                ".html" => "bi bi-filetype-html icon-big",
                ".xls" => "bi bi-filetype-xls icon-big",
                ".xlsx" => "bi bi-filetype-xlsx icon-big",
                _ => "bi bi-file icon-big"
            };

        private string GetSizeViewFile(long size)
        {
            string resultSize = "";
            if (size > 0 && size <= 1024) {
                resultSize = size.ToString() + "Б";
            }
            else if (size > 1024 && size <= (1024 * 1024)) {
                resultSize = Math.Round((size / 1024.0), 1).ToString() + " КБ";
            }
            else {
                resultSize = Math.Round(((double)size / (1024 * 1024)), 1).ToString() + " MБ";
            }
            return resultSize;
        }

        public async Task<string> DeleteFileFromDetailsById(uint? id, string targetFileName)
        {
            StringBuilder sb = new StringBuilder("");
            var foundModelDetail = await _context.LampModelDetails.FirstOrDefaultAsync(m => m.LampModelId == id);
            if (foundModelDetail != null && !string.IsNullOrEmpty(foundModelDetail.FileNames)) {
                foreach (string file in foundModelDetail.FileNames.Split(';'))
                {
                    if (!file.Contains(targetFileName) && !string.IsNullOrEmpty(file)) sb.Append(file).Append(';');
                }
                foundModelDetail.FileNames = sb.ToString();
                _context.LampModelDetails.Update(foundModelDetail);
                await _context.SaveChangesAsync();
                DeleteFromStaticFiles(id, targetFileName);
            }
            return "Ok";
        }

        private void DeleteFromStaticFiles(uint? id, string fileName)
        {
            string directoryName = System.IO.Path.Combine(storageFilesPath, $"Model{id}");
            if (Directory.Exists(directoryName)) {
                string filePath = System.IO.Path.Combine(directoryName, fileName);
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists) {
                    fileInfo.Delete();
                }
            }
        }
    }
}
