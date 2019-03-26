using System.IO;
using TTP.Entities;

namespace TTP
{
    public interface IBootstrapper
    {
        (TTPData data, string statisticsOutputPath, string routeOutputPath) ParseArguments(string[] args);
    }
}