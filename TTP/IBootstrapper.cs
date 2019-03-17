using System.IO;
using TTP.Entities;

namespace TTP
{
    public interface IBootstrapper
    {
        (TTPData data, string csvOutputPath) ParseArguments(string[] args);
    }
}