using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FrameworkProfiles.FileSystem
{
    public static class DiskFileSystem
    {
        public static IFolder Folder(string fullPath)
        {
            return new DiskFolder(fullPath);
        }

        private abstract class DiskFolderItem : IFolderItem
        {
            protected DiskFolderItem(string fullPath)
            {
                FullPath = fullPath;
            }

            public string FullPath { get; private set; }

            public override string ToString()
            {
                return FullPath;
            }
        }

        private sealed class DiskFile : DiskFolderItem, IFile
        {
            public DiskFile(string fullPath)
                : base(fullPath)
            {
            }

            public Stream Open()
            {
                return File.OpenRead(FullPath);
            }
        }

        private sealed class DiskFolder : DiskFolderItem, IFolder
        {
            public DiskFolder(string fullPath)
                : base(fullPath)
            {
            }

            public IEnumerable<IFolder> EnumerateFolders()
            {
                return Directory.EnumerateDirectories(FullPath).Select(x => new DiskFolder(x));
            }

            public IEnumerable<IFile> EnumerateFiles()
            {
                return Directory.EnumerateFiles(FullPath).Select(x => new DiskFile(x));
            }

            public IFolder Folder(string name)
            {
                var path = Path.Combine(FullPath, name);
                if (!Directory.Exists(path))
                    return null;
                return new DiskFolder(path);
            }

            public IFile File(string name)
            {
                var path = Path.Combine(FullPath, name);
                if (!System.IO.File.Exists(path))
                    return null;
                return new DiskFile(path);
            }
        }
    }
}
