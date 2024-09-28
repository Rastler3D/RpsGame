using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RpsGame
{

    public class GameRules
    {
        readonly int availableMoves;

        public GameRules(int availableMoves)
        {
            this.availableMoves = availableMoves;
        }

        public enum GameOutcome
        {
            Win = 1,
            Lose = -1,
            Draw = 0
        }

        public GameOutcome DetermineGameOutcome(int move, int opponentMove)
        {
            var result = Math.Sign((move - opponentMove + (availableMoves >> 1) + availableMoves) % availableMoves - (availableMoves >> 1));

            return (GameOutcome)result;
        }

        public GameOutcome[,] GameOutcomes()
        {
            var outcomes = new GameOutcome[availableMoves, availableMoves];


            for (int i = 0; i < availableMoves; i++)
            {
                for (int j = 0; j < availableMoves; j++)
                {
                    outcomes[i, j] = DetermineGameOutcome(i, j);
                }

            }

            return outcomes;

        }
    }
}
