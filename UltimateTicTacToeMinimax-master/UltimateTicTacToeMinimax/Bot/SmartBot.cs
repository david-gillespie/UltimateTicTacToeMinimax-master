using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace UltimateTicTacToeMinimax.Bot
{
    public class SmartBot
    {
        private const bool Debug = true;

        private Random rand = new Random();

        /// <summary>
        /// Returns the next move to make. Edit this method to make your bot smarter.
        /// Currently does only random moves.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>The column where the turn was made</returns>
        public Move GetMove(BotState state)
        {
            char player;
            if(state.Field.MyId == 0)
            {
                player = 'X';
            }
            else
            {
                player = 'O';
            }
            return Minimax(state, player, 0);

        }

        private Move Minimax(BotState state, char player, int level)
        {

            // Terminal states:
            //player has won: 10
            //other player has won: -10
            //tie = 0, etc...

            UltimateBoard.GameStatus gameState = state.UltimateBoard.GetGameStatus();

            if(gameState == UltimateBoard.GameStatus.OWon)
            {
                return new Move { Score = -100 };
            }else if (gameState == UltimateBoard.GameStatus.XWon)
            {
                return new Move { Score = 100 };
            }else if(gameState == UltimateBoard.GameStatus.Tie)
            {
                return new Move { Score = 0 };
            }

            // Check level == 5 then return score
            if(level == 4)
            {
                int score = state.UltimateBoard.GetScore();
                return new Move { Score = score };
            }

            // For eac available move
            // Score each move by calling Minimax with the opposite player
            // opposite player

            List<Move> moves = state.UltimateBoard.AvailableMoves;

            foreach(Move move in moves)
            {
                // Save state of board and macroboard
                char[,] board = (char[,])state.UltimateBoard.Board.Clone();
                char[,] macroboard = (char[,])state.UltimateBoard.Macroboard.Clone();

                // Make the move
                state.UltimateBoard.MakeMove(move, player);


                //StreamWriter sw = File.AppendText(@"C:\\Users\\dgillespie\\Desktop\\output.txt");
                //sw.WriteLine(level);

                //Score each move by calling Minimax with the
                // opposite player
                if (player == UltimateBoard.PlayerX)
                {
                    move.Score = Minimax(state, UltimateBoard.PlayerO, level + 1).Score;
                }
                else
                {
                    move.Score = Minimax(state, UltimateBoard.PlayerX, level + 1).Score;
                }



                // Basic tree pruning.
                /*if (player == UltimateBoard.PlayerX)
                {
                    if(move.Score == 100)
                    {
                        return move;
                    }
                }
                else
                {
                    if(move.Score == -100)
                    {
                        return move;
                    }
                }*/


                //Revert board to original state
                state.UltimateBoard.Board = board;
                state.UltimateBoard.Macroboard = macroboard;
            }

            if(player == UltimateBoard.PlayerX)
            {
                return moves.Max();
            }
            else
            {
                return moves.Min();
            }

        }
                
        static void Main(string[] args)
        {
            BotParser parser = new BotParser(new SmartBot());
            parser.Run();
        }
    }
}
