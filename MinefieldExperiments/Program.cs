using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MinefieldExperiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RandomTest1();
            //RandomTest2();

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IRandomIntegerGenerator, RandomIntegerGenerator>();
                    //services.AddSingleton<IRandomIntegerGenerator>(provider =>  new RandomIntegerGenerator(13,1)); 
                    services.AddSingleton<Game>();
                })
                .Build();

            var game = host.Services.GetService<Game>();
            PlayGame(game!);
            
            //host.Run();
        }

        static void RandomTest1()
        {
            Console.WriteLine("Random numbers between 0 and 64)");

            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);

            for (int i = 0; i < 30; i++)
            {
                Console.Write("{0} ", r.NextValue(0, 64));
            }
            Console.WriteLine();
        }

        static void RandomTest2()
        {
            Console.WriteLine("Random numbers but sequential");

            RandomIntegerGenerator r = new RandomIntegerGenerator(1, 1);

            for (int i = 0; i < 30; i++)
            {
                Console.Write("{0} ", r.NextValue(1, 63));
            }
            Console.WriteLine();
        }

        static void PlayGame(Game game)
        {
            if (game == null)
            {
                Console.WriteLine("Something went wrong!");
                return;
            }

            Console.WriteLine("Welcome!");
            try
            {
                int mines = 16;
                int size = 8;
                int lives = 6;

                while (true)
                {
                    game.ReadyGame(size, lives, mines);
                    game.Help();

                    GameState result = game.Play();

                    if (!result.gameOver)  // exited game in progress
                    {
                        Console.WriteLine("Thanks for playing!");
                    }
                    else if (result.lives == 0) // lost all lives
                    {
                        Console.WriteLine("Unfortunately you run out of lives!");

                    }
                    else if (lives == result.lives)
                    {
                        Console.WriteLine("Amazing, you completed the game in {0} moves without losing any lives!", result.moves);
                    }
                    else
                    {
                        Console.WriteLine("Congratulations! You completed the game in {0} moves.", result.moves);
                    }

                    Console.WriteLine();
                    Console.Write("Play Again (y/n)? ");
                    char command = ' ';
                    while (command != 'y' && command != 'n')
                    {
                        command = Console.ReadKey().KeyChar;
                    }
                    Console.WriteLine();

                    if (command == 'n')
                        break;

                    Console.WriteLine();
                    Console.WriteLine("New game stated!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
