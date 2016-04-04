using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace FrameworkProfiles.FileSystem
{
    public static class ZipFileSystem
    {
        public static IFolder Open(string zipFilePath)
        {
            var archive = ZipFile.OpenRead(zipFilePath);
            return new ZipFileFolder(archive);
        }

        private abstract class ZipFileFolderItem : IFolderItem
        {
            protected ZipFileFolderItem(ZipArchive zipArchive)
            {
                ZipArchive = zipArchive;
            }

            protected ZipFileFolderItem(ZipArchiveEntry zipArchiveEntry)
            {
                ZipArchive = zipArchiveEntry.Archive;
                ZipArchiveEntry = zipArchiveEntry;
            }

            public string FullPath { get { return IsRoot ? string.Empty : ZipArchiveEntry.FullName; } }

            protected ZipArchive ZipArchive { get; private set; }

            protected ZipArchiveEntry ZipArchiveEntry { get; private set; }

            protected bool IsRoot { get { return ZipArchiveEntry == null; } }

            public override string ToString()
            {
                return FullPath;
            }
        }

        private sealed class ZipFileFile : ZipFileFolderItem, IFile
        {
            public ZipFileFile(ZipArchiveEntry zipArchiveEntry)
                : base(zipArchiveEntry)
            {
            }

            public Stream Open()
            {
                return ZipArchiveEntry.Open();
            }
        }

        private sealed class ZipFileFolder : ZipFileFolderItem, IFolder
        {
            public ZipFileFolder(ZipArchive zipArchive)
                : base(zipArchive)
            {
            }

            public ZipFileFolder(ZipArchiveEntry zipArchiveEntry)
                : base(zipArchiveEntry)
            {
            }

            public IEnumerable<IFolder> EnumerateFolders()
            {
                return ZipArchive.Entries.Where(x => x.FullName.EndsWith("/") && x.FullName.StartsWith(FullPath) && x.FullName.Skip(FullPath.Length).Count(c => c == '/') == 1)
                    .Select(x => new ZipFileFolder(x));
            }

            public IEnumerable<IFile> EnumerateFiles()
            {
                return ZipArchive.Entries.Where(x => !x.FullName.EndsWith("/") && x.FullName.StartsWith(FullPath) && x.FullName.IndexOf('/', FullPath.Length) == -1)
                    .Select(x => new ZipFileFile(x));
            }

            public IFolder Folder(string name)
            {
                var entry = ZipArchive.GetEntry(FullPath + name + "/");
                if (entry == null)
                    return null;
                return new ZipFileFolder(entry);
            }

            public IFile File(string name)
            {
                var entry = ZipArchive.GetEntry(FullPath + name);
                if (entry == null)
                    return null;
                return new ZipFileFile(entry);
            }
        }
    }
}
