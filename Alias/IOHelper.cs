namespace Alias;

public static class IOHelper
{
    static public string? FileType(string path)
    {
        if (!File.Exists(path) && !Directory.Exists(path))
            return null;

        var attributes = File.GetAttributes(path);
        if (attributes.HasFlag(FileAttributes.Directory))
            return "dir";

        return "file";
    }

    static public string? VerifyPath(string path)
    {
        var type = FileType(path);
        if (type == null) {
            Console.WriteLine("Invalid path.");
            return null;
        }

        if (type == "file") {
            if (Path.GetExtension(path) != ".exe" || string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(path)))
                return null;
        }
        //maybe add more checks later?

        return type;
    }
}
