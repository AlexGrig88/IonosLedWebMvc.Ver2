using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models;

namespace IonosLedWebMvc.Ver2.Repos
{
    public interface ILampRepo
    {
        IQueryable<LedLamp> GetAllAsync();
    }
}
