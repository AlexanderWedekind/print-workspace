
namespace PrintWorkspace;

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.IO;

public class Program
{
    public class Folder
    {
        public string dirName = "";
        public string dirPath = "";
        public bool hasSubDirs = false;
        public bool hasFiles = false;
        public string[] subDirs = [];
        public string[] filesPaths = [];
        public int level = 0;
        public Folder(string path, int level)
        {
            this.dirName = GetFileOrFolderName(path);
            this.dirPath = path;
            this.hasSubDirs = CheckForSubDirs(path);
            this.hasFiles = CheckForFiles(path);
            this.subDirs = GetSubDirs(path);
            this.filesPaths = GetFilePaths(path);
            this.level = level + 1;
        }
    }

    public static string GetFileOrFolderName(string path)
    {
        string name;
        name = path.Substring(path.LastIndexOf('\\') + 1);
        return name;
    }

    public static bool CheckForSubDirs(string path)
    {
        bool hasSubDirs = false;
        if(GetSubDirs(path).Length > 0)
        {
            hasSubDirs = true;
        }
        return hasSubDirs;
    }

    public static bool CheckForFiles(string path)
    {
        bool hasFiles = false;
        if(GetFilePaths(path).Length > 0)
        {
            hasFiles = true;
        }
        return hasFiles;
    }

    public static string[] GetSubDirs(string path)
    {
        string[] subDirs;
        subDirs = Directory.GetDirectories(path);
        return subDirs;
    }

    public static string[] GetFilePaths(string path)
    {
        string[] files;
        files = Directory.GetFiles(path);
        return files;
    }

    public static int startingIndentLevel = -1;

    public static string GetIndent(int level)
    {
        string indent = "";
        string oneTab = "    ";
        for(int i = 1; i <= level; i++)
        {
            indent = indent + oneTab;
        }
        return indent;
    }

    public static List<string> ignoreDirs = new List<string>();

    public static bool Ignore(string dirName)
    {
        bool ignore = false;
        foreach(string ignoreThis in ignoreDirs)
        {
            if(GetFileOrFolderName(dirName) == ignoreThis)
            {
                ignore = true;
            }
        }
        return ignore;
    }

    public static List<string> workSpaceStructure = new List<string>();

    public static void AddFolderLine(string path, int level)
    {
        string line = "";
        line = GetIndent(level) + GetFileOrFolderName(path) + "\\";
        workSpaceStructure.Add(line);
    }

    public static void AddFileLine(string path, int level)
    {
        string line = "";
        line = GetIndent(level) + GetFileOrFolderName(path);
        workSpaceStructure.Add(line);
    }

    public static void AddInfoLine(string message, int level)
    {
        string line = "";
        line = GetIndent(level + 1) + message;
        workSpaceStructure.Add(line);
    }

    public static void WriteCurrentLevel(Folder folder)
    {
        AddFolderLine(folder.dirPath, folder.level);
        if(folder.hasSubDirs == true)
        {
            foreach(string subDir in folder.subDirs)
            {
                if(Ignore(subDir) == true)
                {
                    AddFolderLine(subDir, folder.level + 1);
                    AddInfoLine("~ignored contents~", folder.level + 1);
                }
                else
                {
                    Folder nextFolder = new Folder(subDir, folder.level);
                    WriteCurrentLevel(nextFolder);    
                }
                
            }
        }
        if(folder.hasFiles == true)
        {
            foreach(string filePath in folder.filesPaths)
            {
                AddFileLine(filePath, folder.level + 1);
            }
        }
    }
    static void Main(string[] args)
    {
        foreach(string name in args)
        {
            ignoreDirs.Add(name);
        }
        string workspacePath = Directory.GetCurrentDirectory();
        Folder workSpaceDir = new Folder(workspacePath, 0);
        WriteCurrentLevel(workSpaceDir);
        foreach(string line in workSpaceStructure)
        {
            Console.WriteLine(line);
        }
    }
}