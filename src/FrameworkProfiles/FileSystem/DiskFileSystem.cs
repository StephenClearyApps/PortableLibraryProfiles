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
                return new DiskFolder(Path.Combine(FullPath, name));
            }

            public IFile File(string name)
            {
                return new DiskFile(Path.Combine(FullPath, name));
            }
        }
    }
}
