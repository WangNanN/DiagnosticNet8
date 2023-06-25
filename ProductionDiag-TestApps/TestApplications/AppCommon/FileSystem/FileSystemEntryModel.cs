using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AppSharedCore.FileSystem
{
    public enum FileEntryType
    {
        File,
        Directory
    }

    public class FileSystemEntryModel
    {
        public FileSystemEntryModel(string name, FileEntryType type)
        {
            Name = name;
            Type = type;
        }

        public bool IsRoot => Name == string.Empty;

        public string Name { get; }

        public FileEntryType Type { get; }

        public string DisplayName
        {
            get
            {
                string displayName = Path.GetFileName(Name);
                return !string.IsNullOrEmpty(displayName) ? displayName : Name;
            }
        }

        public string Parent
        {
            get
            {
                if (IsRoot)
                {
                    return Name;
                }
                string parent = Path.GetDirectoryName(Name);
                return !string.IsNullOrEmpty(parent) ? parent : string.Empty;
            }
        }

        public IList<FileSystemEntryModel> Children { get; } = new List<FileSystemEntryModel>();

        public static FileSystemEntryModel CreateFromFolder(string rootFolder)
        {
            var fileSystemEntry = new FileSystemEntryModel(rootFolder ?? string.Empty, FileEntryType.Directory);

            string logicalRoot = rootFolder;

            if (fileSystemEntry.IsRoot)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    foreach (var child in Directory.GetLogicalDrives().Select(drive => new FileSystemEntryModel(drive, FileEntryType.Directory)))
                    {
                        fileSystemEntry.Children.Add(child);
                    }
                    return fileSystemEntry;
                }
                else
                {
                    logicalRoot = Path.DirectorySeparatorChar.ToString();
                }
            }

            foreach(var directory in Directory.GetDirectories(logicalRoot).Select(dir => new FileSystemEntryModel(dir, FileEntryType.Directory)))
            {
                fileSystemEntry.Children.Add(directory);
            }

            foreach(var file in Directory.GetFiles(logicalRoot, "*", SearchOption.TopDirectoryOnly).Select(f => new FileSystemEntryModel(f, FileEntryType.File)))
            {
                fileSystemEntry.Children.Add(file);
            }


            return fileSystemEntry;
        }
    }
}
