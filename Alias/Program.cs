using Cocona;
using Alias;

var app = CoconaApp.Create();

app.AddCommand("add", ([Argument] string alias, [Argument] string path) => AliasFactory.Add(alias, path))
.WithDescription("Add new alias to PATH");

app.AddCommand("remove", ([Argument] string alias) => AliasFactory.Remove(alias))
.WithDescription("Remove alias from the PATH");

app.AddCommand("list", AliasFactory.List)
.WithDescription("List currently avaliable aliases");

app.AddCommand(() => Console.WriteLine("Unknown command."));

app.Run();

