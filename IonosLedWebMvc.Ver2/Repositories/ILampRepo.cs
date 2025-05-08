using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models;
using System.Linq;

namespace IonosLedWebMvc.Ver2.Repos
{
    public interface ILampRepo
    {
        IQueryable<LedLamp> GetAllAsQueryable();
        IQueryable<LedLamp> GetAllReleased();
        IQueryable<LedLamp> GetDateFiltering(DateTime startDt, DateTime endDt);
    }
}
