using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpsGame
{
    public class ValidationError : IRenderable
    {
        private string message;

        protected ValidationError(string message)
        {
            this.message = message;
        }

        public static ValidationError InvalidLength =
            new ValidationError("Please enter an odd move number, greater than 1");

        public static ValidationError InvalidArguments =
            new ValidationError("All moves must be distinct.");


        private string Markup => string.Join("\n",
               "[red]Argument error.",
               message,
               "[/]",
               "[yellow]Example: Rock Paper Scissors.[/]\n");

        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            return ((IRenderable)new Markup(Markup)).Measure(options, maxWidth);
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            return ((IRenderable)new Markup(Markup)).Render(options, maxWidth);
        }
    }
}
