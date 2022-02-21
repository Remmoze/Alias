using Cocona;
using Alias;

var app = CoconaApp.Create();

app.AddCommand("add", ([Argument] string alias, [Argument] string path, [Option('f')]bool? force) => AliasFactory.Add(alias, path, force))
.WithDescription("Add new alias to PATH");

app.AddCommand("remove", ([Argument] string alias) => AliasFactory.Remove(alias))
.WithDescription("Remove alias from the PATH");

app.AddCommand("list", () => {
    AliasFactory.List();

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
})
.WithDescription("List currently avaliable aliases");

app.AddCommand(() => Console.WriteLine("Unknown command."));

app.Run();

