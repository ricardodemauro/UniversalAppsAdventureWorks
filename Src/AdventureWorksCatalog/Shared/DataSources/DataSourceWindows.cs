using AdventureWorksCatalog.Portable.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Windows.ApplicationModel;

namespace AdventureWorksCatalog.DataSources
{
    public class DataSourceWindows : DataSource
    {
        protected override async Task<List<string>> ListFiles(string directoryPath, FilePathKind filePathKind)
        {
            var folder = await GetFolderAsync(directoryPath, filePathKind);
            if (folder == null)
            {
                return new List<string>();
            }
            var files = await folder.GetFilesAsync();
            return files.Select((file) => file.Path).ToList();
        }

        protected override async Task<Stream> OpenFileReadAsync(string filePath, FilePathKind filePathKind)
        {
            var file = await GetFileAsync(filePath, filePathKind);
            if (file == null)
            {
                return null;
            }
            var randomStream = await file.OpenReadAsync();
            return randomStream.AsStream();
        }

        private async Task<StorageFile> GetFileAsync(string filePath, FilePathKind filePathKind)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);
            var storageFolder = await GetFolderAsync(directoryPath, filePathKind);
            if (storageFolder == null)
            {
                return null;
            }

            try 
            {
                return await storageFolder.GetFileAsync(fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<StorageFolder> GetFolderAsync(string directoryPath, FilePathKind filePathKind)
        {
            try
            {
                switch (filePathKind)
                {
                    case FilePathKind.InstalledFolder:
                        return await Package.Current.InstalledLocation.GetFolderAsync(directoryPath);
                    case FilePathKind.DataFolder:
                        return await ApplicationData.Current.LocalFolder.GetFolderAsync(directoryPath);
                    case FilePathKind.AbsolutePath:
                        return await StorageFolder.GetFolderFromPathAsync(directoryPath);
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (FileNotFoundException) 
            {
                return null;
            }
        }


        private async Task<StorageFile> CreateFileAsync(string filePath, FilePathKind filePathKind)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);
            var storageFolder = await CreateFolderAsync(directoryPath, filePathKind);
            return await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }

        private async Task<StorageFolder> CreateFolderAsync(string directoryPath, FilePathKind filePathKind)
        {
            StorageFolder storageFolder = null;
            switch (filePathKind)
            {
                case FilePathKind.InstalledFolder:
                    Package package = Package.Current;
                    storageFolder = package.InstalledLocation;
                    break;
                case FilePathKind.DataFolder:
                    storageFolder = ApplicationData.Current.LocalFolder;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var folders = directoryPath.Split('\\', '/');
            foreach (var folder in folders)
            {
                storageFolder = await storageFolder.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists);
            }
            return storageFolder;
        }

        protected override async Task<Stream> OpenFileWriteAsync(string filePath, DataSource.FilePathKind filePathKind)
        {
            var file = await CreateFileAsync(filePath, filePathKind);
            var randomStream = await file.OpenAsync(FileAccessMode.ReadWrite);
            return randomStream.AsStream();
        }

        protected override async Task DeleteFileAsync(string filePath, DataSource.FilePathKind filePathKind)
        {
            var file = await GetFileAsync(filePath, filePathKind);
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

        public override async Task<DateTimeOffset?> GetFileModifiedDateAsync(string filePath, DataSource.FilePathKind filePathKind)
        {
            var file = await GetFileAsync(filePath, filePathKind);
            if (file != null)
            {
                var properties = await file.GetBasicPropertiesAsync();
                return properties.DateModified;
            }
            return null;
        }
    }
}
