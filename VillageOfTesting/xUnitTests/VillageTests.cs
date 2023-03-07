using VillageOfTesting;

using Moq;
using VillageOfTesting;
using VillageOfTesting.Buildings;

namespace xUnitTests;

public class VillageTests
{
    private Village _villageUnitTests = new Village(100, 100);

    #region AddWorker Tests
    
    [Fact]
    public void Addworker_Add3Workers_Return3()
    {
        int expected = 6;

        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddWood);
        AddWorkers(_villageUnitTests, 2, _villageUnitTests.AddWood);
        AddWorkers(_villageUnitTests, 3, _villageUnitTests.AddWood);

        Assert.Equal(expected, _villageUnitTests.Workers.Count);
    }
    
    [Fact]
    public void Addworker_Add7thWorkerWith3Houses_Return3Houses6Workers()
    {
        int expectedWorkers = 6;
        int expectedHouses = 3;
        
        AddWorkers(_villageUnitTests, 7, _villageUnitTests.AddWood); //adding 7 workers with only 3 houses, should add 6 workers and reject the 7th

        Assert.Equal(expectedHouses, _villageUnitTests.HouseCount);
        Assert.Equal(expectedWorkers, _villageUnitTests.Workers.Count);
    }

    [Fact]
    public void Addworker_AddWoodWorkerRunDayMethod_Return1Wood()
    {
        _villageUnitTests.Wood = 0;
        int expected = 1;
        
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddWood);
        _villageUnitTests.Day();

        Assert.Equal(expected, _villageUnitTests.Wood);
    }

    #endregion

    #region Day Tests

    [Fact]
    public void Day_RunDayWithoutWorkers_Return10FoodAnd0DaysPassed()
    {
        int startFood = _villageUnitTests.Food; // food from start is equal to 10
        int expectedDaysPassed = 1;

        _villageUnitTests.Day(); // run day method, without any workers nobody is fed so the food amount stays the same and one day passes. 
        
        Assert.Equal(startFood, _villageUnitTests.Food);
        Assert.Equal(expectedDaysPassed, _villageUnitTests.DaysPassed);
    }

    
    [Fact]
    public void Day_RunDayWithWorkers_ShouldCollect1MetalAndReduceFoodBy1()
    {
        _villageUnitTests.Metal = 0;
        int expectedMetalCount = 1;
        int expectedFoodCount = 9;
        
        int food = _villageUnitTests.Food;
        AddWorkers(_villageUnitTests,1, _villageUnitTests.AddMetal);
        _villageUnitTests.Day();

        Assert.Equal(10, food); //food starting amount
        Assert.Equal(expectedMetalCount, _villageUnitTests.Metal); //metal amount after 1 day
        Assert.Equal(expectedFoodCount, _villageUnitTests.Food); //food amount after 1 day
    }
    
    [Fact]
    public void Day_RunDayWithoutfood_ShouldNotCollectMetalWithoutFood()
    {
        _villageUnitTests.Food = 0;
        int expected = _villageUnitTests.Metal;
        
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddMetal);
        RunDayMethod(_villageUnitTests, 1);
        int metalCount = _villageUnitTests.Metal;

        Assert.Equal(expected, metalCount);
    }
    
    [Fact]
    public void Day_RunDayWithoutfood_ShouldKillWorker()
    {
        _villageUnitTests.Food = 0;
        int expectedAliveStatus = 0;
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddMetal);

        RunDayMethod(_villageUnitTests, 40);// no food left on last iteration, kills and buries the worker
        
        Assert.Equal(expectedAliveStatus, _villageUnitTests.Workers.Count); // worker is dead and buried, Amount of workers is now 0
    }
    #endregion
    
    #region AddProject Tests
    
    [Fact]
    public void AddProject_SuccessfullyAddProject_Return1ItemInList()
    {
        _villageUnitTests.Wood = 50;
        _villageUnitTests.Metal = 50;
        
        int expected = 1;
        
        _villageUnitTests.AddProject(new House());
        
        Assert.Equal(expected, _villageUnitTests.Projects.Count);
    }
    
    [Fact]
    public void AddProject_FailToAddProjectDueToLowResources_ReturnEmptyList()
    {
        _villageUnitTests.Wood = 0;
        _villageUnitTests.Metal = 0;
        int expected = 0;
        
        _villageUnitTests.AddProject(new Quarry());
        
        Assert.Equal(expected, _villageUnitTests.Projects.Count);
    }
    
    [Fact]
    public void Build_BuildHouseEffects_ShouldLetUserAdd1MoreWorker()
    {
        int expected = 7;
        
        _villageUnitTests.Wood = 10;
        
        _villageUnitTests.AddProject(new House());
        _villageUnitTests.AddWorker(_villageUnitTests.Build);
        RunDayMethod(_villageUnitTests, 5);
        
        AddWorkers(_villageUnitTests, 6, _villageUnitTests.AddWood);
        
        Assert.Equal(expected, _villageUnitTests.Workers.Count);
    }
    
    [Fact]
    public void Build_BuildWoodmillEffects_ShouldCollect2MoreWood()
    {
        _villageUnitTests.Wood = 5;
        _villageUnitTests.Metal = 1;

        int woodCollectionBeforeFarm = _villageUnitTests.WoodWorkerAmount;

        AddWorkers(_villageUnitTests, 1, _villageUnitTests.Build);
        _villageUnitTests.AddProject(new Woodmill());
        RunDayMethod(_villageUnitTests, 5);

        int woodCollectionAfterFarm = _villageUnitTests.WoodWorkerAmount;
        
        Assert.Equal(1, woodCollectionBeforeFarm);
        Assert.Equal(3, woodCollectionAfterFarm);
    }
    
    [Fact]
    public void Build_BuildQuarryEffects_ShouldCollect2MoreMetal()
    {
        _villageUnitTests.Wood = 3;
        _villageUnitTests.Metal = 5;
        
        int woodCollectionBeforeFarm = _villageUnitTests.MetalWorkerAmount;

        AddWorkers(_villageUnitTests, 1, _villageUnitTests.Build);
        _villageUnitTests.AddProject(new Quarry());
        RunDayMethod(_villageUnitTests, 7);

        int woodCollectionAfterFarm = _villageUnitTests.MetalWorkerAmount;
        
        Assert.Equal(1, woodCollectionBeforeFarm);
        Assert.Equal(3, woodCollectionAfterFarm);
    }
    
    [Fact]
    public void Build_BuildFarmEffects_ShouldCollect10MoreFood()
    {
        _villageUnitTests.Wood = 5;
        _villageUnitTests.Metal = 2;

        int foodCollectionBeforeFarm = _villageUnitTests.FoodWorkerAmount;

        AddWorkers(_villageUnitTests, 1, _villageUnitTests.Build);
        _villageUnitTests.AddProject(new Farm());
        RunDayMethod(_villageUnitTests, 5);

        int foodCollectionAfterFarm = _villageUnitTests.FoodWorkerAmount;
        
        Assert.Equal(5, foodCollectionBeforeFarm);
        Assert.Equal(15, foodCollectionAfterFarm);
    }
    
    [Fact]
    public void Build_BuildCastleEffects_ShouldWinGame()
    {
        _villageUnitTests.Wood = 50;
        _villageUnitTests.Metal = 50;

        bool expected = true;

        AddWorkers(_villageUnitTests, 1, _villageUnitTests.Build);
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddFood);
        _villageUnitTests.AddProject(new VillageOfTesting.Buildings.Castle());
        RunDayMethod(_villageUnitTests, 50);
        bool actual = _villageUnitTests.Win();
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Build_StartBuildingAddWorkers_ShouldFinishFaster()
    {
        _villageUnitTests.Wood = 50;
        _villageUnitTests.Metal = 50;

        int expectedDayCount = 2;

        AddWorkers(_villageUnitTests, 1, _villageUnitTests.Build);
        
        _villageUnitTests.AddProject(new House());
        RunDayMethod(_villageUnitTests, 1);
        
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.Build);
        RunDayMethod(_villageUnitTests, 1);
        
        int actual = _villageUnitTests.DaysPassed;

        Assert.Equal(4, _villageUnitTests.HouseCount);
        Assert.Equal(expectedDayCount, actual);
    }
    
    #endregion

    #region FeedWorker Tests
    [Fact]
    public void DaysHungry_DaysPassed40_ReturnAliveFalse()
    {
        _villageUnitTests.Food = 0;
        _villageUnitTests.AddWorker(_villageUnitTests.AddWood);

        bool expected = false;
        
        RunFeedMethod(_villageUnitTests, 40);
        bool actual = _villageUnitTests.Workers[0].Alive;
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void FeedWorker_FeedDeadWorker_ReturnAliveFalse()
    {
        _villageUnitTests.Food = 0;
        _villageUnitTests.AddWorker(_villageUnitTests.AddWood);

        bool expectedWhenWorkerDies = false;
        bool expectedAfterFeedingDeadWorker = false;
        
        RunFeedMethod(_villageUnitTests, 40);
        bool actualWhenWorkerDies = _villageUnitTests.Workers[0].Alive;
        
        RunFeedMethod(_villageUnitTests, 1);
        bool actualAfterFeedingDeadWorker = _villageUnitTests.Workers[0].Alive;
        
        Assert.Equal(expectedWhenWorkerDies, actualWhenWorkerDies);
        Assert.Equal(expectedAfterFeedingDeadWorker, actualAfterFeedingDeadWorker);
    }
    
    [Fact]
    public void BuryDead_BuryTheDead_Return0Workers()
    {
        _villageUnitTests.Food = 0;
        int expected = _villageUnitTests.Workers.Count;
        _villageUnitTests.AddWorker(_villageUnitTests.AddWood);

        RunFeedMethod(_villageUnitTests, 40);
        _villageUnitTests.BuryDead();
        int actual = _villageUnitTests.Workers.Count;
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PlayFromStartToFinish_BuildCastle()
    {
        _villageUnitTests.Wood = 0;
        _villageUnitTests.Metal = 0;
        _villageUnitTests.Food = 10;
        int expectedDaysPassed = 100;
        int expectedWoodAfter50Days = 50;
        int expectedMetalAfter50Days = 50;
        
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddFood);
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddMetal);
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.AddWood);
        AddWorkers(_villageUnitTests, 1, _villageUnitTests.Build);
        
        RunDayMethod(_villageUnitTests, 50);
        int actualWoodAfter50Days = _villageUnitTests.Wood;
        int actualMetalAfter50Days = _villageUnitTests.Metal;
        _villageUnitTests.AddProject(new VillageOfTesting.Buildings.Castle());
        
        RunDayMethod(_villageUnitTests, 50);
        int actualDaysPassed = _villageUnitTests.DaysPassed;
        
        
        Assert.Equal(expectedWoodAfter50Days,actualWoodAfter50Days);
        Assert.Equal(expectedMetalAfter50Days,actualMetalAfter50Days);
        Assert.Equal(expectedDaysPassed, actualDaysPassed);
        Assert.True(_villageUnitTests.Win());
        
    }

    #endregion

    #region MockTests

    [Fact]
    public void Load_LoadProgressFromDbMock()
    {
        MockRepository mockRepository = new MockRepository(MockBehavior.Loose);
        var mockDatabaseConnection = mockRepository.Create<DataBaseConnection>();
        DataBaseConnection dbMock = mockDatabaseConnection.Object;
        Village village = new Village(dbMock);
        mockDatabaseConnection.Setup(mock => mock.GetWood()).Returns(10);

        Village actual = village.LoadProgressForMockTest();
        
        Assert.True(actual != null);
        Assert.Equal(10, actual.Wood);
    }
    
    
    [Fact]
    public void AddRandomWorker_MockResult()
    {
        Mock<RandomNumberGenerator> objectMock = new Mock<RandomNumberGenerator>();
        RandomNumberGenerator random = objectMock.Object;
        Village village = new Village(random);
        
        objectMock.Setup(mock => mock.ReturnRandomNumber()).Returns(0);
        
        village.AddRandomWorker(village._randomNumberGenerator);
            
        village.Day();

        int expectedWood = 1;

        int actualWood = village.Wood;
        
        Assert.Single(village.Workers);
        Assert.Equal(expectedWood, actualWood);
    }
    #endregion
    
    #region MethodsForTests
    private void RunDayMethod(Village village, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            village.Day();
        }
    }
    
    private void AddWorkers(Village village, int amount, Worker.OccupationDelegate occupationDelegate)
    {
        for (int i = 0; i < amount; i++)
        {
            village.AddWorker(occupationDelegate);
        }
    }


    private void RunFeedMethod(Village village, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            village.FeedWorkers();
        }
    }

    #endregion
    
}
