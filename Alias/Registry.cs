namespace Alias;
using Microsoft.Win32;

public static class RegistryManipulation
{
    public static List<AliasEntry> GetAliases()
    {
        var aliases = new List<AliasEntry>();
        var reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths", true);
        if (reg == null) {
            Console.WriteLine("Could not access the registry.");
            return aliases;
        }

        foreach (var key in reg.GetSubKeyNames()) {
            var regAlias = reg.OpenSubKey(key);
            if (regAlias?.GetValue("Alias") == null)
                continue;

            var cleanKey = key;
            if (key.EndsWith(".exe"))
                cleanKey = key[..key.IndexOf(".exe")];

            var creationDate = (string?)regAlias.GetValue("CreationDate") ?? "";
            var path = (string?)regAlias.GetValue("");
            var type = (string?)regAlias.GetValue("Type") ?? "exe";

            if (path == null) {
                Console.WriteLine($"Warning: found key {cleanKey} without a path!");
                continue;
            }

            aliases.Add(new AliasEntry() {
                Name = cleanKey,
                Date = creationDate,
                Path = path,
                Type = type
            }) ;
        }
        return aliases;
    }

    public static void Add(string alias, string path, string type = "exe")
    {
        var dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir)) {
            Console.WriteLine("Error occured while reading Path");
            return;
        }

        try {
            var reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths", true);
            if(reg == null) {
                Console.WriteLine("Error occured while reading Registry.");
                return;
            }

            reg = reg.CreateSubKey(alias + ".exe");

            reg.SetValue("", path);
            reg.SetValue("Path", dir);
            reg.SetValue("Type", type);
            reg.SetValue("Alias", true);
            reg.SetValue("CreationDate", DateTime.Now);

            Console.WriteLine($"Alias {alias} has been added.");
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }

    public static void Remove(string alias)
    {
        try {
            var reg = Registry.LocalMachine.OpenSubKey($"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\", true);
            if (reg == null) {
                Console.WriteLine("Error occured while reading Registry.");
                return;
            }

            if (!reg.GetSubKeyNames().Contains($"{alias}.exe")) {
                Console.WriteLine($"Alias \"{alias}\" was not found in registry");
                return;
            }

            reg.DeleteSubKey(alias + ".exe");

            Console.WriteLine($"Alias {alias} has been removed.");
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }
}
