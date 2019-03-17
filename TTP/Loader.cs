using System;
using System.IO;
using TTP.Entities;

namespace TTP
{
    public class Loader : ILoader
    {
        private readonly IFileHelper _fileHelper;

        public Loader(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public TTPData LoadFromFile(string path)
        {
            var fileData =_fileHelper.ReadFile(path);
            if (!_fileHelper.IsFileDataValid(fileData))
            {
                throw new InvalidOperationException();
            }
            return _fileHelper.ParseFileData(fileData);
        }
    }
}