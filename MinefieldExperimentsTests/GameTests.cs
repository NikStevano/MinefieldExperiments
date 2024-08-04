using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinefieldExperiments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldExperiments.Tests
{
    [TestClass()]
    public class GameTests
    {

        [TestMethod()]
        public void EdgeMovementSuccessTest()
        {
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);
            Game game = new Game(r);
            game.ReadyGame(8, 14, 62);

            // we will try to move too many times, hit 13 mines along the way

            // move right
            for (int i = 0; i < 18; i++)
            {
                game.ProcessCommand('r');
            }
            // then move up
            for (int i = 0; i < 18; i++)
            {
                game.ProcessCommand('u');
            }

            GameState result = game.GetGameState();

            Assert.IsTrue(result.gameOver);
            Assert.AreEqual(result.moves, 14);
            Assert.AreEqual(result.lives, 1);
        }

        [TestMethod()]
        public void DiagonalMovementSuccessTest()
        {
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);
            Game game = new Game(r);
            game.ReadyGame(8, 14, 62);

            for (int i = 0; i < 8; i++)
            {
                // move up
                game.ProcessCommand('u');
                // then move right
                game.ProcessCommand('r');
            }

            GameState result = game.GetGameState();

            Assert.IsTrue(result.gameOver);
            Assert.AreEqual(result.moves, 14);
            Assert.AreEqual(result.lives, 1);
        }

        [TestMethod()]
        public void NoMinesSuccessTest()
        {
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);
            Game game = new Game(r);
            game.ReadyGame(8, 1, 0);
            // no mines and only one life
            // go diagonal
            for (int i = 0; i < 8; i++)
            {
                // move up
                game.ProcessCommand('u');
                // then move right
                game.ProcessCommand('r');
            }

            GameState result = game.GetGameState();

            Assert.IsTrue(result.gameOver);
            Assert.AreEqual(result.moves, 14);
            Assert.AreEqual(result.lives, 1);
        }

        [TestMethod()]
        public void OutOfLivesTest()
        {
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);
            Game game = new Game(r);
            game.ReadyGame(8, 1, 62);
            // move up, it will hit a mine
            game.ProcessCommand('u');

            GameState result = game.GetGameState();

            Assert.IsTrue(result.gameOver);
            Assert.AreEqual(result.moves, 1);
            Assert.AreEqual(result.lives, 0);
        }

        [TestMethod()]
        public void PredictableMinesTest()
        {
            // this will return simple sequence with numbers increasing by 1
            RandomIntegerGenerator r = new RandomIntegerGenerator(1, 1);
            Game game = new Game(r);
            game.ReadyGame(8, 1, 10);

            // cells A8 and B1 should have mines as sequence will start at 7 then 8 ...
            Assert.AreEqual(game.GetBoardContent(0, 7), Game.CELL_MINE);
            Assert.AreEqual(game.GetBoardContent(1, 0), Game.CELL_MINE);
        }

        [TestMethod()]
        public void ArgumentExceptionTest()
        {
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);
            try
            {
                // too many mines
                Game game = new Game(r);
                game.ReadyGame(8, 1, 100);
                Assert.Fail("No exception thrown!");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

    }
}