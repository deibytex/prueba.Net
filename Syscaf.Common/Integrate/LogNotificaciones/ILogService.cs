using Syscaf.Data.Models.Portal;

namespace Syscaf.Common.Integrate.LogNotificaciones
{
    public interface ILogService
    {
        void SetLog(LogDTO log);
        void SetLogError(int OptionId, string Method, string Description);
    }
}
