using System.Collections;
using System.Collections.Generic;
using TTP.Entities;

namespace TTP
{
    public interface IFileHelper
    {
        List<string> ReadFile(string path);
        bool IsFileDataValid(IEnumerable<string> fileData);
        TTPData ParseFileData(List<string> fileData);
        void WriteToCsv<T>(IEnumerable<T> records, string path);
    }
}