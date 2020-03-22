using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Lab_1.Tools.Managers
{
    internal static class SerializationManager
    {
        internal static void Serialize<TObject>(TObject obj, string filePath)
        {
            try
            {
                var file = new FileInfo(filePath);
                if (file.CreateFolderAndCheckFileExistence())
                {
                    file.Delete();
                }

                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    formatter.Serialize(stream, obj);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to serialize data to file {filePath}", ex);
            }
        }

        internal static TObject Deserialize<TObject>(string filePath) where TObject : class
        {
            try
            {
                if (!FileFolderHandler.CreateFolderAndCheckFileExistence(filePath))
                    throw new FileNotFoundException("File doesn't exist.");
                if (new FileInfo(filePath).Length == 0) throw new ArgumentException($"File '{filePath}' is empty.");
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    return (TObject) formatter.Deserialize(stream);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException($"Failed to Deserialize PersonExtract From File {filePath}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Completely failed to Deserialize PersonExtract From File {filePath}", ex);
            }
        }
    }
}