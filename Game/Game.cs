using Spectre.Console;


namespace RpsGame
{
    class Game
    {
        readonly string[] moves;
        readonly GameRules rules;

        protected Game(string[] moves, GameRules rules)
        {
            this.moves = moves;
            this.rules = rules;
        }

        public static Result<Game, ValidationError> Create(string[] moves)
        {
            if (!ValidateArguments(moves, out var error))
            {
                return error;
            }

            var rules = new GameRules(moves.Length);

            return new Game(moves, rules);

        }

        public void Run()
        {
            var computerMove = Random.Shared.Next(0, moves.Length);
            var privateKey = new SecretKeyGenerator().GenerateKey();
            var hmac = new HMACGenerator().GenerateHMAC(privateKey, moves[computerMove]);

            AnsiConsole.MarkupLine($"HMAC [yellow]: {hmac}[/]");

            while (true)
            {
                var userSelection = PromptUserMove();

                if (userSelection == moves.Length)
                {
                    ShowHelp();
                    continue;
                }
                DetermineWinner(computerMove, userSelection);

                AnsiConsole.MarkupLine($"HMAC Key: [yellow]{privateKey}[/]");
                break;
            }

        }

        private void DetermineWinner(int computerMove, int userMove)
        {
            AnsiConsole.MarkupLine($"[teal]Your move: {moves[userMove]}[/]");
            AnsiConsole.MarkupLine($"[teal]Computer move: {moves[computerMove]}[/]");
            var outcome = rules.DetermineGameOutcome(userMove, computerMove);

            var result = outcome switch
            {
                GameRules.GameOutcome.Win => "[green]You win[/]",
                GameRules.GameOutcome.Lose => "[red]You lose[/]",
                GameRules.GameOutcome.Draw => "[blue]Draw[/]",
            };

            AnsiConsole.MarkupLine(result);
        }

        private void ShowHelp()
        {
            var help = new HelpTable(rules.GameOutcomes(), moves);

            AnsiConsole.Prompt(help);
        }

        private int PromptUserMove()
        {
            var select = new SelectionPrompt<int>()
                .Title("Available moves:")
                .AddChoices(Enumerable.Range(0, moves.Length + 1))
                .UseConverter(i => i == moves.Length ? "? - help" : $"{i + 1} - {moves[i]}");

            return AnsiConsole.Prompt(select);
        }

        private static bool ValidateArguments(string[] args, out ValidationError error)
        {
            if (args.Length <= 1 || args.Length % 2 == 0)
            {
                error = ValidationError.InvalidLength;
                return false;
            }

            if (args.Distinct().Count() != args.Count())
            {
                error = ValidationError.InvalidArguments;
                return false;
            }
            error = default;
            return true;
        }
    }
}