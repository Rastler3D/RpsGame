using Spectre.Console;
using Spectre.Console.Rendering;

namespace RpsGame
{

    internal class HelpTable : IRenderable, IPrompt<bool>
    {
        const int PAGE_SIZE = 4;

        readonly List<IEnumerable<IRenderable>> rows;
        readonly Table table;

        public HelpTable(GameRules.GameOutcome[,] outcomes, string[] moves)
        {
            if (outcomes.GetLength(0) != moves.Length || outcomes.GetLength(1) != moves.Length) throw new ArgumentException("Invalid size", nameof(outcomes));

            table = new Table();
            table.Title = new TableTitle("Game outcomes", new Style(Color.Blue));

            table.AddColumn(new TableColumn(new Text("v PC\\User >", new Style(foreground: Color.Yellow))));
            foreach (var move in moves)
            {
                table.AddColumn(new TableColumn(new Text(move, new Style(foreground: Color.Yellow))));
            }
            
            rows = new List<IEnumerable<IRenderable>>(moves.Length);

            for (var i = 0; i < outcomes.GetLength(0); i++)
            {
                var row = new List<IRenderable>(moves.Length)
                {
                   new Text(moves[i])
                };
                for (var j = 0; j < outcomes.GetLength(1); j++)
                {
                    row.Add(outcomes[i, j].ToText());
                }
                rows.Add(row);
            }
        }

        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            return Page(0, rows.Count).Measure(options, maxWidth);
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            return Page(0, rows.Count).Render(options, maxWidth);
        }

        public IRenderable Page(int page, int pageSize = PAGE_SIZE)
        {
            table.Rows.Clear();
            foreach (var row in rows.Skip(page * pageSize).Take(pageSize))
            {
                table.Rows.Add(row);
            }

            return table;
        }

        public enum TableOption
        {
            NextPage = 0,
            PreviousPage = 1,
            Back = 2
        }

        public bool Show(IAnsiConsole console)
        {
            var page = 0;
            var maxPage = rows.Count / PAGE_SIZE;
            var choices = new List<TableOption>(2);

            while (true)
            {
                console.Write(Page(page));
                choices.Clear();
                if (page < maxPage) choices.Add(TableOption.NextPage);
                if (page > 0) choices.Add(TableOption.PreviousPage);
                var select = new SelectionPrompt<TableOption>()
                    .Title("")
                    .AddChoices(choices)
                    .AddChoices(TableOption.Back)
                    .UseConverter(x => new[] { "Next page", "Previous page", "Back" }[(int)x]);

                switch (console.Prompt(select))
                {
                    case TableOption.Back: return true;
                    case TableOption.NextPage: page++; break;
                    case TableOption.PreviousPage: page--; break;
                };

            }
        }

        async Task<bool> IPrompt<bool>.ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
        {
            return await Task.Run(() => Show(console), cancellationToken);
        }
    }

    static class GameResultExt
    {
        internal static Text ToText(this GameRules.GameOutcome result)
        {
            var color = result switch
            {
                GameRules.GameOutcome.Win => Color.Green,
                GameRules.GameOutcome.Lose => Color.Red,
                GameRules.GameOutcome.Draw => Color.Blue,
            };

            var text = result.ToString();

            return new Text(text, new Style(color));
        }
    }
}
