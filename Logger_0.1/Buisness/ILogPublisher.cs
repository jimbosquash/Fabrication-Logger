
namespace FabricationLogger.Buisness
{
    public interface ILogPublisher
    {
        void Publish();
        void SetLog(ILog log);
        string GetFilePath();

        string FileName { get; set; }
        string FilePath { get; set; }
    }
}
