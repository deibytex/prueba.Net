using Syscaf.Data.Models.Portal;

namespace Syscaf.Service
{
    public interface ILogService
    {
        void SetLog(LogDTO log);
        void SetLogError(int OptionId, string Method, string Description);
    }
}
