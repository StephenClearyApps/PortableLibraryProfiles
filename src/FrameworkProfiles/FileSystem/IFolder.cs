using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkProfiles.FileSystem
{
    public interface IFolder : IFolderItem
    {
        IEnumerable<IFolder> EnumerateFolders();

        IEnumerable<IFile> EnumerateFiles();
            
        IFolder Folder(string name);

        IFile File(string name);
    }
}
