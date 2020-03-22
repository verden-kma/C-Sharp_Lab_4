using System;
using System.IO;

namespace Lab_1.Tools
{
    internal static class FileFolderHandler
    {
        private static readonly string AppDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // ProgramData = CommonApplicationData

        private static readonly string AppFolderPath = Path.Combine(AppDataPath, "CSharpLab_04");

        internal static readonly string StorageFilePath = Path.Combine(AppFolderPath, "people.bin");

        internal static bool CreateFolderAndCheckFileExistence(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            return file.CreateFolderAndCheckFileExistence();
        }

        internal static bool CreateFolderAndCheckFileExistence(this FileInfo file)
        {
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            return file.Exists;
        }
    }
}