using RpsGame;
using Spectre.Console;

Game.Create(args)
    .Match(
        success => success.Run(), 
        failure => AnsiConsole.Write(failure)
    );



