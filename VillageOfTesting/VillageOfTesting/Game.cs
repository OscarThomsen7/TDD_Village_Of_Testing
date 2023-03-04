
using VillageOfTesting.Buildings;

namespace VillageOfTesting;
public class Game
{
    private RandomNumberGenerator RandomNumberGenerator = new RandomNumberGenerator();
    private static DataBaseConnection DataBaseConnection = new DataBaseConnection();
    private static Village _village = new Village();
    private static bool playBool = true;
    public void PlayGame()
    {
        _village.LoadProgress();
        GameLoop();
    }

    #region Methods

    private void AddWorkerToGame(int input)
    {
        Console.Clear();
        switch (input)
        {
            case 1:
                _village.AddWorker(_village.Build);
                break;
            case 2:
                _village.AddWorker(_village.AddFood);
                break;
            case 3:
                _village.AddWorker(_village.AddMetal);
                break;
            case 4:
                _village.AddWorker(_village.AddWood);
                break;
            case 5:
                _village.AddRandomWorker(RandomNumberGenerator);
                break;
            
            default:
                Console.WriteLine("Wrong input");
                break;
                
        }
    }
    
    private void PickWorkerToChange(Worker.OccupationDelegate occupationDelegate)
    {
        if (_village.Workers.Count > 0)
        {
            int count = 1;
            Console.Clear();
            Console.WriteLine("pick a worker to change\n");
            for (int i = 0; i < _village.Workers.Count; i++)
            {
                Console.WriteLine($"{count}. {_village.Workers[i].Name}, {_village.Workers[i].Occupation}\n");
                count++;
            }

            int input = int.Parse(Console.ReadLine());
            if (input >= 1 && input <= _village.Workers.Count)
            {
                _village.ChangeWorkerOccupation(_village.Workers[input - 1], occupationDelegate);
                Console.Clear();
                Console.WriteLine($"Changed {_village.Workers[input - 1].Name}'s occupation to {_village.Workers[input - 1].Occupation}");
            }
        }
    }
    private Worker.OccupationDelegate PickOccupation(int input)
    {
        Worker.OccupationDelegate occupationDelegate = null;
        if (_village.Workers.Count > 0)
        {
            switch (input)
            {
                case 1:
                    occupationDelegate = _village.Build;
                    break;
                case 2:
                    occupationDelegate = _village.AddFood;
                    break;
                case 3:
                    occupationDelegate = _village.AddMetal;
                    break;
                case 4:
                    occupationDelegate = _village.AddWood;
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        }
        return occupationDelegate;
    }
    
    private void AddProjectToGame()
    {
        Console.Clear();
        Console.WriteLine($"1. {new House().Type}, Woodcost: {new House().WoodCost}, Metalcost: {new House().MetalCost}," +
                          $" Days to complete: {new House().DaysToComplete}\n");
        
        Console.WriteLine($"2. {new Woodmill().Type}, Woodcost: {new Woodmill().WoodCost}, Metalcost: {new Woodmill().MetalCost}," +
                          $" Days to complete: {new Woodmill().DaysToComplete}\n");
        
        Console.WriteLine($"3. {new Quarry().Type}, Woodcost: {new Quarry().WoodCost}, Metalcost: {new Quarry().MetalCost}," +
                          $" Days to complete: {new Quarry().DaysToComplete}\n");
        
        Console.WriteLine($"4. {new Farm().Type}, Woodcost: {new Farm().WoodCost}, Metalcost: {new Farm().MetalCost}," +
                          $" Days to complete: {new Farm().DaysToComplete}\n");
        
        Console.WriteLine($"5. {new Castle().Type}, Woodcost: {new Castle().WoodCost}, Metalcost: {new Castle().MetalCost}," +
                          $" Days to complete: {new Castle().DaysToComplete}\n");
        int input = int.Parse(Console.ReadLine());


        switch (input)
        {
            case 1:
                _village.AddProject(new House());
                break;
            case 2:
                _village.AddProject(new Woodmill());
                break;
            case 3:
                _village.AddProject(new Quarry());
                break;
            case 4:
                _village.AddProject(new Farm());
                break;
            case 5:
                _village.AddProject(new Castle());
                break;
            default:
                Console.WriteLine("Wrong input");
                break;
        }
    }

    private void Play()
    {
        Console.WriteLine("Input how many days you want to pass");
        int days = int.Parse(Console.ReadLine());
        for (int i = 0; i < days; i++)
        {
            if (_village.Win() == true)
            {
                Console.Clear();
                Console.WriteLine($"The castle is built! You won! It took you {_village.DaysPassed} days!");
                DataBaseConnection.DeleteRows();
                playBool = false;
                break;
            }
            _village.Day();
        }
    }
    
    private void OutputRules()
    {
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------" +
                          "\nBuilding Rules\n\nOnly one Building can be built at a time, " +
                          "if you start building a house you have to finish building it before you can start building another.\n" +
                          "For every worker who has the profession 'Builder' you increment the days worked on the building.\n" +
                          "So if you have 3 workers building a house it will be finished in one day.\nBuilding effects:\n" +
                          "House: Lets player add 2 more workers.\nWoodmill: +2 wood per lumberjack\nQuarry: +2 metal per miner" +
                          "\nFarm: +10 food per food producer\nCastle: You win the game\n" +
                          "------------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine("\nWorker Rules\n\nYou can only have 2 workers per house, " +
                          "you will start the game with 3 houses so you can 6 workers at the start of the game.\nIf you want to add more you have to build more houses." +
                          "\nEvery worker has to be fed every day in order to work. If they go hungry for 40 days they die.\n\n" +
                          "Worker jobs:\nBuilder: Builds buildings\nLumberjack: Collects wood\nMiner: Collects metal\nFood producer: Collects food\n" +
                          "------------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine("\nVillage Rules\n\nCollect resources to build a castle. When you complete a castle you win." +
                          "\nWhen all your workers die you lose.\n" +
                          "------------------------------------------------------------------------------------------------------------------------");
    }

    private int Workers(string occupation)
    {
        int amount = 0;
        foreach (var worker in _village.Workers)
        {
            if (worker.Occupation == occupation)
            {
                amount++;
            }
        }
        return amount;
    }
    
    private int Buildings(string type)
    {
        int amount = 0;
        foreach (var building in _village.Buildings)
        {
            if (building.Type == type)
            {
                amount++;
            }
        }
        return amount;
    }

    private void HungryWorkers()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var worker in _village.Workers)
        {
            if (worker.DaysHungry > 0)
            {
                Console.Write($"{worker.Name} refuses to work due to hunger! worker has been hungry for {worker.DaysHungry} days, at 40 days the worker will die!\n");
            }
        }
        Console.ResetColor();
    }


    private void GameInfo()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"                    DAYS PASSED: {_village.DaysPassed}\n");
        Console.ResetColor();
        
        Console.Write($"Workers                 Buildings               Village\nBuilders: {Workers("Builder")}             " +
                      $"Houses: {Buildings("House")}            " +
                      $"   Food: {_village.Food}" +
                      $"\nFood producers: {Workers("Food producer")}       Farms: {Buildings("Farm")}                " +
                      $"Wood: {_village.Wood}" +
                      $"\nLumberjacks: {Workers("Lumberjack")}          Woodmills: {Buildings("Woodmill")}            " +
                      $"Metal: {_village.Metal}" +
                      $"\nMiners: {Workers("Miner")}               Quarries: {Buildings("Quarry")}             Wood collected per lumberjack: {_village.WoodWorkerAmount}\n" +
                      $"                        Castles: {Buildings("Castle")}" +
                      $"              Metal collected per miner: {_village.MetalWorkerAmount}" +
                      $"\n                                                Food collected per food producer: {_village.FoodWorkerAmount}\n");
        
        HungryWorkers();
    }

    private  void GameLoop()
    {
        //DataBaseConnection dataBaseConnection = new DataBaseConnection();
        while (playBool)
        {
            if (_village.Workers.Count == 0 && _village.Food == 0)
            {
                Console.WriteLine($"All workers died. You lose! You lasted {_village.DaysPassed} days....");
                break;
            }
                
            if (_village.Win() == true)
            {
                Console.Clear();
                Console.WriteLine($"The castle is built! You won! It took you {_village.DaysPassed} days!");
                DataBaseConnection.DeleteRows();
                playBool = false;
                break;
            }
            Console.Clear();
            GameInfo();
            Console.WriteLine("1. Add worker\n2. Change worker occupation\n3. Add project\n4. Make 1 day pass\n5. Make multiple days pass " +
                              "\n6. Read rules" +
                              "\n7. Save game and quit");
            int input = int.Parse(Console.ReadLine());

            switch (input)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("What occupation do you want your worker to have?");
                    Console.WriteLine($"1. Builder \n2. Food Producer \n3. Miner \n4. Lumberjack \n5. Random occupation");
                    AddWorkerToGame(int.Parse(Console.ReadLine()));
                    Thread.Sleep(1000);
                    break;
                    
                case 2:
                    if (_village.Workers.Count > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("What occupation do you want your worker to have?");
                        Console.WriteLine($"1. Builder \n2. Food Producer \n3. Miner \n4. Lumberjack");
                        PickWorkerToChange(PickOccupation(int.Parse(Console.ReadLine())));
                        Thread.Sleep(1000);    
                    }
                    
                    break;
                    
                case 3:
                    Console.Clear();
                    AddProjectToGame();
                    Thread.Sleep(1000);
                    break;
                    
                case 4:
                    Console.Clear();
                    _village.Day();
                    Thread.Sleep(3000);
                    break;
                
                case 5:
                    Console.Clear();
                    Play();
                    Thread.Sleep(3000);
                    break;
                    
                case 6:
                    Console.Clear();
                    OutputRules();
                    Console.WriteLine("\nPress any key to go back");
                    Console.ReadLine();
                    break;
                    
                case 7:
                    _village.SaveProgress();
                    Console.Clear();
                    Console.WriteLine("Saved game. Exiting...");
                    playBool = false;
                    break;
                    
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        }
    }
    #endregion
    
    
}