using TTP.Entities;

namespace TTP
{
    public interface ILoader
    {
        TTPData LoadFromFile(string path);
    }
}