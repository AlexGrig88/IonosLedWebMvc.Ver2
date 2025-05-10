using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models.Entities;
using System.Linq;

namespace IonosLedWebMvc.Ver2.Repos
{
    public interface ILampRepo
    {
        IQueryable<LedLamp> GetAllAsQueryable();
        IQueryable<LedLamp> GetAllReleased();
        IQueryable<LedLamp> GetDateFiltering(DateTime startDt, DateTime endDt);

        public IQueryable<LedLamp> GetLabelPrintAsQueryable();
        public IQueryable<LedLamp> GetAssemblyAsQueryable();
        public IQueryable<LedLamp> GetCutAsQueryable();
        public IQueryable<LedLamp> GetDrillAsQueryable();
        public IQueryable<LedLamp> GetMountAsQueryable();
        public IQueryable<LedLamp> GetSolderAsQueryable();
        public IQueryable<LedLamp> GetPackegAsQueryable();
    }
}
