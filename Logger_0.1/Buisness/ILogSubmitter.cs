
namespace FabricationLogger.Buisness
{
    public interface ILogSubmitter
    {
        void SubmitLogEntry();
        void SetLog(ILog log); 
    }
}
