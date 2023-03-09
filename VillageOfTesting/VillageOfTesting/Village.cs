using System.Reflection.Metadata;
using VillageOfTesting.Buildings;

namespace VillageOfTesting;

public class Village
{
    public int Food { get; set; }
    public int Wood { get; set; }
    public int Metal { get; set; }
    public int WoodWorkerAmount { get; set; } = 1;
    public int MetalWorkerAmount { get; set; } = 1;
    public int FoodWorkerAmount { get; set; } = 5;
    public int HouseCount { get; set; }
    public int DaysPassed { get; set; }
    public List<Worker> Workers { get; set; } = new();
    public List<Building> Buildings { get; set; } = new();
    public List<Building> Projects { get; set; } = new();
    public int NameNumber { get; set; } = 0;
    
    public RandomNumberGenerator _randomNumberGenerator { get; set; } = new();
    public DataBaseConnection DataBaseConnection { get; set; } = new();


    #region Constructors

    public Village(DataBaseConnection dataBaseConnection) //Constructor used for mocking tests
    {
        DataBaseConnection = dataBaseConnection;
        AddStartStats();
    }
    
    public Village(RandomNumberGenerator randomNumberGenerator) //Constructor used for mocking tests
    {
        _randomNumberGenerator = randomNumberGenerator;
        AddStartStats();
    }
    public Village(int wood, int metal) // Constructor used for unit test
    {
        Wood = wood;
        Metal = metal;
        AddStartStats();
    }

    public Village() //Constructor used in program
    {
        
    }

    #endregion

    
    //Adds a worker, lets user pick what work function the worker should have. Passes that function to the worker class through delegate.
    //can only be 2 workers per house built.
    public void AddWorker(Worker.OccupationDelegate occupationDelegate) 
    {
        if (Workers.Count >= HouseCount * 2)
        {
            Console.WriteLine($"Can not add another worker! there can only be 2 workers per house. Current:\nWorkers: {Workers.Count} \nHouses: {HouseCount}");
            return;
            //return false;
        }
        NameNumber++;
        Workers.Add(new Worker("Worker " + NameNumber, SetWorkerOccupation(occupationDelegate), occupationDelegate));
        Console.WriteLine($"Added worker. \nName: Worker {NameNumber}. \nOccupation: {SetWorkerOccupation(occupationDelegate)}\n");
        //return true;
    }

    
    //Adds a new building to the project list if player has enough materials.
    public void AddProject(Building building)
    {
        if (Wood >= building.WoodCost && Metal >= building.MetalCost)
        {
            Projects.Add(building);
            Wood -= building.WoodCost;
            Metal -= building.MetalCost;
            Console.WriteLine($"Successfully added {building.Type} to the project list");
            return;
        }

        Console.WriteLine($"Not enough materials to add a {building.Type.ToLower()} to the project list, collect more. \nVillage materials:" +
                          $"\n{Wood} wood \n{Metal} metal");
        Console.WriteLine($"Materials needed for building a {building.Type.ToLower()}:" +
                          $"\n{building.WoodCost} wood \n{building.MetalCost} metal");
        
    }
    
    
    //Makes a day pass in the game where all methods are called to play the game.
    public void Day()
    {
        Console.WriteLine($"DAY {DaysPassed + 1}\n-------------------------------------");
        DaysPassed++;
        if (Workers.Count > 0)
        {
            FeedWorkers();
            BuryDead();
            foreach (Worker worker in Workers)
            {
                worker.DoWork();
            }

            Console.WriteLine("-------------------------------------");
        }
    }

    
    //Can be passed in to delegate, adds food to village.
    public void AddFood()
    {
        Console.WriteLine($"Adding {FoodWorkerAmount} food to village");
        Food += FoodWorkerAmount;
    }
    
    //Can be passed in to delegate, adds metal to village.
    public void AddMetal()
    {
        Console.WriteLine($"Adding {MetalWorkerAmount} metal to village");
        Metal += MetalWorkerAmount;
    }
    
    //Can be passed in to delegate, adds wood to village.
    public void AddWood()
    {
        Console.WriteLine($"Adding {WoodWorkerAmount} wood to village");
        Wood += WoodWorkerAmount;
    }

    //Can be passed in to delegate, builds and completes projects.
    public void Build()
    {
        if (Projects.Count > 0)
        {
            Projects[0].DaysWorkedOn++;
            Console.WriteLine($"Building on {Projects[0].Type}");
            if (Projects[0].DaysWorkedOn == Projects[0].DaysToComplete)
            {
                Console.WriteLine($"{Projects[0].Type} complete!");
                BuildingIsComplete();
                return;
            }
            return;
        }
        Console.WriteLine("Builders can't build! You have no projects.");
    }

    //Feeds all workers as long as player has food, if there is no food all workers go hungry, +1 day per day passed.
    //If a worker is hungry for 40 days it dies.
    public void FeedWorkers()
    {
        for (int i = 0; i < Workers.Count; i++)
        {
            if (Food <= 0)
            {
                Workers[i].Hungry = true;
                Workers[i].DaysHungry++;
                
                if (Workers[i].DaysHungry >= 40)
                {
                    Workers[i].WorkerDied();
                    Console.WriteLine($"{Workers[i].Name} died due to hunger.");
                    Thread.Sleep(1000);
                }
                
            }

            if (Workers[i].DaysHungry >= 40)
            {
                Workers[i].WorkerDied();
                Console.WriteLine($"{Workers[i].Name} died due to hunger.");
                Thread.Sleep(1000);
            }
            
            else if (Food > 0)
            {
                Workers[i].FeedWorker();
                Food -= 1;
            }
        }
    }

    //Adds 3 houses and 10 food in the beginning of the game to make game easier to play
    public void AddStartStats()
    {
            Food += 10;
            for (int i = 0; i < 3; i++)
            {
                Buildings.Add(new House());
                HouseCount++;
            }
            foreach (Building house in Buildings)
            {
                house.Completed = true;
            }
    }

    
    //Checks if a building is complete. If so, it is moved from the project list to the buildings list.
    //Each building built affects how much resources the village collects per worker.
    public void BuildingIsComplete()
    {
        if (Projects[0].GetType() == typeof(House))
            HouseCount++;
        
        if (Projects[0].GetType() == typeof(Woodmill))
            WoodWorkerAmount += 2;
        
        if (Projects[0].GetType() == typeof(Quarry))
            MetalWorkerAmount += 2;
        
        if (Projects[0].GetType() == typeof(Farm))
            FoodWorkerAmount += 10;
        
        Projects[0].Completed = true;
        Projects[0].DaysToComplete = 0;
        Buildings.Add(Projects[0]);
        Projects.RemoveAt(0);
        Win();
    }
    
    
    //picks the occupation to represent the worker based on what method the worker delegate gets passed.
    public string SetWorkerOccupation(Worker.OccupationDelegate occupationDelegate)
    {
        if (occupationDelegate == AddMetal)
            return "Miner";
        if (occupationDelegate == AddFood)
            return "Food producer";
        if (occupationDelegate == AddWood)
            return "Lumberjack";
        if (occupationDelegate == Build)
            return "Builder";
        return "Unemployed";
    }
    
    //Buries every worker who dies. Removes them from the worker list. if all workers die the game is lost.
    public void BuryDead()
    {
        int beforeBuriedCount = Workers.Count;
        
        Workers.RemoveAll(worker => worker.Alive == false);
        if (Workers.Count < beforeBuriedCount)
        {
            Console.WriteLine("Buried the dead");
        }
        
    }
    
    //Checks if a castle is built. If so, the game is won
    public bool Win()
    {
        foreach (var building in Buildings)
        {
            if (building.GetType() == typeof(Castle))
            {
                return true;
            }   
        }
        return false;
    }

    //Adds a worker with a random mwork method passed tot the delegate
    public void AddRandomWorker(RandomNumberGenerator randomNumberGenerator)
    {
        switch (randomNumberGenerator.ReturnRandomNumber())
        {
            case 0 :
                AddWorker(AddWood);
                break;
            case 1 :
                AddWorker(AddMetal);
                break;
            case 2 :
                AddWorker(AddFood);
                break;
            case 3 :
                AddWorker(Build);
                break;
        }
    }
    
    //Saves data to DB
    public void SaveProgress()
    {
        DataBaseConnection.Save(Workers, Buildings, Projects, this);
    }
    
    //Loads data from DB
    public virtual Village LoadProgress()
    {
        DataBaseConnection.Load(this);
        return this;
    }

    
    //Changes the worker delegate method
    public void ChangeWorkerOccupation(Worker worker, Worker.OccupationDelegate occupationDelegate)
    {
        worker.ChangeOccupation(occupationDelegate);
        worker.Occupation = SetWorkerOccupation(occupationDelegate);
    }

    
    public Village LoadProgressForMockTest()//Method used for mock test in assignment, has no real use
    {
        DataBaseConnection.LoadForMockTest(this);
        return this;
    }
}