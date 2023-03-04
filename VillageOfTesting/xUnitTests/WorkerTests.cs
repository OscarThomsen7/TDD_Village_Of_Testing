using VillageOfTesting;

namespace xUnitTests;

public class WorkerTests
{
    public Village _village = new Village();
    
    [Fact]
    public void WorkerDied_WorkerDies_IsAliveShouldBeFalseAndDaysHungryShouldBe0()
    {
        Worker worker = new Worker("name", "worker",_village.AddFood);
        worker.WorkerDied();
        
        int expectedHungerDays = 0;
        bool expectedAliveStatus = false;

        Assert.Equal(expectedHungerDays, worker.DaysHungry);
        Assert.Equal(expectedAliveStatus, worker.Alive);
        
    }    

    [Fact]
    public void FeedWorker_WorkerGotFed_ShouldNotBeHungryAndDaysHungryShouldBe0()
    {
        Worker worker = new Worker("name", "Woodworker",_village.AddFood);
        worker.FeedWorker();
        
        bool expectedHunger = false;
        int expectedDaysHungry = 0;
        
        Assert.Equal(expectedHunger, worker.Hungry);
        Assert.Equal(expectedDaysHungry, worker.DaysHungry);
    }

    [Fact]
    public void DoWork_WorkerIsHungry_Return1Wood()
    {
        Worker worker = new Worker("name", "worker",_village.AddWood);
        
        worker.DoWork();
        int didWork = _village.Wood;
        worker.Hungry = true;
        
        worker.DoWork();
        int refusedWork = _village.Wood;
        
        Assert.Equal(didWork, refusedWork);
    }

}