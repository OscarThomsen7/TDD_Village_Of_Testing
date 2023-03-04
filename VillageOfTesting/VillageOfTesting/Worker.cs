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
    public Worker(string name, string occupation, OccupationDelegate occupationDelegate)
    {
        Name = name;
        Occupation = occupation;
        _occupationDelegate = occupationDelegate;
    }
    
    public void DoWork() //Works
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

    public void FeedWorker()
    {
        Hungry = false;
        DaysHungry = 0;
    }

    public void WorkerDied()
    {
        Alive = false;
        DaysHungry = 0;
    }

    public void SetHunger(int num)
    {
        if (num == 0)
        {
            Hungry = false;
        }

        Hungry = true;
    }
    
    public void SetAlive(int num)
    {
        if (num == 0)
        {
            Alive = false;
        }
        Alive = true;
    }

    public void WorkerInfo()
    {
        Console.WriteLine($"Name: {Name} \nOccupation: {Occupation} \nIs hungry: {Hungry}" +
                          $"\nIs alive: {Alive} \nBeen hungry for: {DaysHungry} days\n");
    }

    public void ChangeOccupation(OccupationDelegate occupationDelegate)
    {
        _occupationDelegate = occupationDelegate;
    }
}