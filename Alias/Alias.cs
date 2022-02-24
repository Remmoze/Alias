namespace Alias;
using System.Text;
using System.Text.RegularExpressions;

public struct AliasEntry
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Type { get; set; }
    public string Date { get; set; }
}

public static class AliasFactory
{
    private static List<AliasEntry> GetAliases() =>
        RegistryManipulation.GetAliases();

    public static void List()
    {
        var aliases = GetAliases();
        foreach (var alias in aliases) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(alias.Name);
            Console.ResetColor();
            Console.Write(" - ");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(alias.Date);
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine(alias.Path);
            Console.WriteLine();
        }
    }

    private static void AppDataFolder()
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Alias");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
    }

    private static string GetAliasPath(string alias)
    {
        AppDataFolder();
        var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(directory, "Alias", alias + ".bat");
    }


    public static void Add(string alias, string path, bool? force = false)
    {
        if (string.IsNullOrEmpty(alias) || string.IsNullOrEmpty(path)) {
            Console.WriteLine("Provided invalid arguments");
            return;
        }

        if (!Regex.IsMatch(alias, @"^[a-zA-Z0-9_]+$")) {
            Console.WriteLine($"Alias \"{alias}\" should only contain letters, numbers, and _");
            return;
        }

        var type = IOHelper.VerifyPath(Path.GetFullPath(path));
        if (type == null) {
            return;
        }

        var dirname = Path.GetDirectoryName(path);
        if (dirname == null) {
            Console.WriteLine("Unknown error.");
            return;
        }

        if (dirname.Equals("c:\\")) {
            Console.WriteLine($"Failed to add alias \"{alias}\". Can not add aliases to root directory.");
            return;
        }

        var aliases = GetAliases();

        if (aliases.Any(x => x.Name == alias)) {
            if (force.HasValue && (bool)force) {
                // if --force flag was provided, we remove existing alias and create a new one.
                Remove(alias);
            }
            else {

                Console.WriteLine($"Alias \"{alias}\" already exists.");
                return;
            }
        }
        if (type == "dir") {
            AddDirectory(alias, dirname);
        }
        else {
            RegistryManipulation.Add(alias, path);
        }

    }

    private static void AddDirectory(string alias, string path)
    {
        var batPath = GetAliasPath(alias);

        using FileStream fs = File.Create(batPath);

        var text = "start C:\\Windows\\explorer.exe " + path;
        var bytes = Encoding.UTF8.GetBytes(text);

        fs.Write(bytes, 0, bytes.Length);
        fs.Close();

        RegistryManipulation.Add(alias, batPath, "dir");
    }


    public static void Remove(string alias)
    {
        if (string.IsNullOrEmpty(alias)) {
            Console.WriteLine("Invalid Alias");
            return;
        }

        var aliases = GetAliases();

        if (!aliases.Any(x => x.Name == alias)) {
            Console.WriteLine($"Alias \"{alias}\" doesn't exist.");
            return;
        }

        var entry = aliases.FirstOrDefault(x => x.Name == alias);
        if(entry.Type == "dir") {
            RemoveDirectory(alias);
        } else {
            RegistryManipulation.Remove(alias);
        }
    }

    public static void RemoveDirectory(string alias)
    {
        var batPath = GetAliasPath(alias);
        if(File.Exists(batPath))
            File.Delete(batPath);

        RegistryManipulation.Remove(alias);
    }
}
