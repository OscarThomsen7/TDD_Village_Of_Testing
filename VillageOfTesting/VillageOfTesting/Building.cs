namespace VillageOfTesting;

public class Building
{
    public string Type { get; set; }
    public int WoodCost { get; set; }
    public int MetalCost { get; set; }
    public int DaysWorkedOn { get; set; }
    public int DaysToComplete { get; set; }
    public bool Completed { get; set; }
    
    public Building(string type, int woodCost, 
        int metalCost, int daysWorkedOn, int daysToComplete, bool complete)
    {
        Type = type;
        WoodCost = woodCost;
        MetalCost = metalCost;
        DaysWorkedOn = daysWorkedOn;
        DaysToComplete = daysToComplete;
        Completed = complete;
    }

    public void BuildingInfo()
    {
        Console.WriteLine($"Type: {Type} \nWoodcost: {WoodCost} \nMetalcost: {MetalCost}" +
                          $"\nDays worked on: {DaysWorkedOn} \nDays to complete: {DaysToComplete} days" +
                          $"\nCompleted: {Completed}\n");
    }
}