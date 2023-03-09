namespace VillageOfTesting;

public class Worker
{
    public string Name { get; set; }
    public string Occupation { get; set; }
    public bool Hungry { get; set; } = false;
    public bool Alive { get; set; } = true;
    public int DaysHungry { get; set; }
    public delegate void OccupationDelegate(); 
    private OccupationDelegate _occupationDelegate;
    
    //Constructor sets name, occupation and a delegate that will contain 1 of the worker functions of the village
    public Worker(string name, string occupation, OccupationDelegate occupationDelegate)
    {
        Name = name;
        Occupation = occupation;
        _occupationDelegate = occupationDelegate;
    }
    
    
    
    //Function is called when a worker is supposed to work, this function then calls the delegate that contains the work function from the village class.
    //if the worker is hungry or dead no work will be done.
    public void DoWork()
    {
        if (Alive == false)
        {
            Console.WriteLine($"{Name} is dead, can't work.");
            return;
        }
        if (Hungry)
        {
            Console.WriteLine($"{Name} refuses to work because of hunger.");
            return;
        }
        _occupationDelegate();
    }

    
    //Resets workers hunger stats
    public void FeedWorker()
    {
        Hungry = false;
        DaysHungry = 0;
    }

    
    //Kills worker
    public void WorkerDied()
    {
        Alive = false;
        DaysHungry = 0;
    }

    //Method used to set hunger status of worker when fetching data from DB
    public void SetHunger(int num)
    {
        if (num == 0)
        {
            Hungry = false;
        }

        Hungry = true;
    }
    
    //Method used to set alive status of worker when fetching data from DB
    public void SetAlive(int num)
    {
        if (num == 0)
        {
            Alive = false;
        }
        Alive = true;
    }
    
    
    //Changes worker occupation
    public void ChangeOccupation(OccupationDelegate occupationDelegate)
    {
        _occupationDelegate = occupationDelegate;
    }
}