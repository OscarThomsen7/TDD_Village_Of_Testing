using Microsoft.Data.Sqlite;

namespace VillageOfTesting;

public class DataBaseConnection
{   
    private static string _dataBaseString = "Data Source=DataBase.sqlite";
    
	public void Save(List<Worker> workers, List<Building> buildings, List<Building> projects, Village village)
	{
		CreateDatabase();
		SaveVillage(village);
		SaveWorker(workers);
		SaveBuilding(buildings);
		SaveProject(projects);
	}
	public void CreateDatabase()
    {
	    if (!File.Exists("./DataBase.sqlite"))
	    {
			SqliteConnection connection = new SqliteConnection(_dataBaseString);
			connection.Open();
			Console.WriteLine("Created database file.");
		    CreateVillageTable();
		    CreateWorkersTable();
		    CreateBuildingsTable();
		    CreateProjectsTable();
		    connection.Close();
	    }
    }
    public void CreateVillageTable()
    {
		SqliteConnection connection = new SqliteConnection(_dataBaseString);
		connection.Open();

	    string tableString =
		    " CREATE TABLE Village (ID INTEGER NOT NULL UNIQUE, Food INTEGER, Wood INTEGER, Metal INTEGER," +
		    "WoodWorkerAmount INTEGER," +
		    "MetalWorkerAmount INTEGER," +
		    "FoodWorkerAmount INTEGER," +
		    "DaysPassed INTEGER," +
		    "NameNumber INTEGER," +
		    "HouseNumber INTEGER," +
		    "PRIMARY KEY(ID AUTOINCREMENT))";

	    SqliteCommand command = new SqliteCommand(tableString, connection);
	    command.ExecuteNonQuery();
	    connection.Close();
    }
    private void CreateWorkersTable()
    {
		SqliteConnection connection = new SqliteConnection(_dataBaseString);
		connection.Open();
		string tableString =
		    " CREATE TABLE Workers (ID INTEGER NOT NULL UNIQUE, Name TEXT, Occupation TEXT, Hungry INTEGER," +
		    "Alive INTEGER," +
		    "DaysHungry INTEGER," +
		    "PRIMARY KEY(ID AUTOINCREMENT))";

	    SqliteCommand command = new SqliteCommand(tableString, connection);
	    command.ExecuteNonQuery();
	    connection.Close();
    }
    private void CreateBuildingsTable()
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);
	    connection.Open();

	    string tableString =
		    " CREATE TABLE Buildings (ID INTEGER NOT NULL UNIQUE, Type TEXT, WoodCost INTEGER, MetalCost INTEGER," +
		    "DaysWorkedOn INTEGER," +
		    "DaysToComplete INTEGER," +
		    "Complete INTEGER," +
		    "PRIMARY KEY(ID AUTOINCREMENT))";

	    SqliteCommand command = new SqliteCommand(tableString, connection);
	    command.ExecuteNonQuery();
	    connection.Close();
    }
    private void CreateProjectsTable()
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);
	    connection.Open();

	    string tableString =
		    " CREATE TABLE Projects (ID INTEGER NOT NULL UNIQUE, Type TEXT, WoodCost INTEGER, MetalCost INTEGER," +
		    "DaysWorkedOn INTEGER," +
		    "DaysToComplete INTEGER," +
		    "Complete INTEGER," +
		    "PRIMARY KEY(ID AUTOINCREMENT))";

	    SqliteCommand command = new SqliteCommand(tableString, connection);
	    command.ExecuteNonQuery();
	    connection.Close();
    }
    public void DeleteRows()
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);
	    connection.Open();
	    string query1 = "DELETE FROM Village";
	    string query2 = "DELETE FROM Workers";
	    string query3 = "DELETE FROM Buildings";
	    string query4 = "DELETE FROM Projects";
	    
	    SqliteCommand command1 = new SqliteCommand(query1, connection);
	    command1.ExecuteNonQuery();
	    SqliteCommand command2 = new SqliteCommand(query2, connection);
	    command2.ExecuteNonQuery();
	    SqliteCommand command3 = new SqliteCommand(query3, connection);
	    command3.ExecuteNonQuery();
	    SqliteCommand command4 = new SqliteCommand(query4, connection);
	    command4.ExecuteNonQuery();

	    connection.Close();
    }
    public void SaveWorker(List<Worker> workers)
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);

	    foreach (var worker in workers)
	    {
		    string query =
			    $"INSERT INTO Workers (Name, Occupation, Hungry, Alive, DaysHungry) VALUES ( '{worker.Name}'," +
			    $"'{worker.Occupation}'," +
			    $"{worker.Hungry},{worker.Alive},{worker.DaysHungry})";
	    
		    SqliteCommand command = new SqliteCommand(query, connection);
		    connection.Open();
		    command.ExecuteNonQuery();
		    connection.Close();
	    }
    }
    public void SaveProject(List<Building> buildings)
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);

	    foreach (var building in buildings)
	    {
		    string query =
			    $"INSERT INTO Projects (Type, WoodCost, MetalCost, DaysWorkedOn, DaysToComplete, Complete) " +
			    $"VALUES ( '{building.Type}'," +
			    $"'{building.WoodCost}'," +
			    $"{building.MetalCost},{building.DaysWorkedOn}," +
			    $"{building.DaysToComplete},{building.Completed})";

		    SqliteCommand command = new SqliteCommand(query, connection);
		    connection.Open();
		    command.ExecuteNonQuery();
		    connection.Close();   
	    }
    }
    public void SaveBuilding(List<Building> buildings)
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);
	    
	    foreach (var building in buildings)
	    {
		    string query =
			    $"INSERT INTO Buildings (Type, WoodCost, MetalCost, DaysWorkedOn, DaysToComplete, Complete) " +
			    $"VALUES ( '{building.Type}'," +
			    $"'{building.WoodCost}'," +
			    $"{building.MetalCost},{building.DaysWorkedOn}," +
			    $"{building.DaysToComplete},{building.Completed})";

		    SqliteCommand command = new SqliteCommand(query, connection);
		    connection.Open();
		    command.ExecuteNonQuery();
		    connection.Close();
	    }
    }
    public void SaveVillage(Village village)
    {
	    int count = 0;
	    foreach (var building in village.Buildings)
	    {
		    if (building.Type == "House")
			    count++;
	    }
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);
	    string query =
		    $"INSERT INTO Village (Food, Wood, Metal, WoodWorkerAmount, MetalWorkerAmount, FoodWorkerAmount, DaysPassed" +
		    $", NameNumber, HouseNumber) " +
		    $"VALUES ( '{village.Food}'," + $"'{village.Wood}'," + $"{village.Metal},{village.WoodWorkerAmount}," +
		    $"{village.MetalWorkerAmount},{village.FoodWorkerAmount}," +
		    $"{village.DaysPassed}," +
		    $"{village.NameNumber}," +
		    $"{count})";

	    SqliteCommand command = new SqliteCommand(query, connection);
	    connection.Open();
	    command.ExecuteNonQuery();
	    connection.Close();
    }
    public void LoadWorker(Village village) //Works
    {
	    Worker.OccupationDelegate occupationDelegate = null;
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);

	    string data = "SELECT * FROM Workers";
	    SqliteCommand command = new SqliteCommand(data, connection);
	    connection.Open();
	    SqliteDataReader sqliteDataReader = command.ExecuteReader();

	    while (sqliteDataReader.Read())
	    {
		    if (sqliteDataReader["occupation"].ToString() == "Builder")
			    occupationDelegate = village.Build;
		    
		    if (sqliteDataReader["occupation"].ToString() == "Miner")
			    occupationDelegate = village.AddMetal;
		    
		    if (sqliteDataReader["occupation"].ToString() == "Lumberjack")
			    occupationDelegate = village.AddWood;
		    
		    if (sqliteDataReader["occupation"].ToString() == "Food producer")
			    occupationDelegate = village.AddFood;

		    
		    Worker worker = new Worker(sqliteDataReader["name"].ToString(),
			    sqliteDataReader["occupation"].ToString(), occupationDelegate);
		    
		    worker.DaysHungry = Int32.Parse(sqliteDataReader["dayshungry"].ToString());
		    int hunger = Int32.Parse(sqliteDataReader["hungry"].ToString());
		    worker.SetHunger(hunger);
		    int alive = Int32.Parse(sqliteDataReader["alive"].ToString());
		    worker.SetAlive(alive);
		    village.Workers.Add(worker);
		    
	    }
	    connection.Close();
    }
    public void LoadBuilding(Village village)
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);

	    string data = "SELECT * FROM Buildings";
	    SqliteCommand command = new SqliteCommand(data, connection);
	    connection.Open();
	    SqliteDataReader sqliteDataReader = command.ExecuteReader();

	    while (sqliteDataReader.Read())
	    {
		    string type = sqliteDataReader["type"].ToString();
		    int woodcost = Int32.Parse(sqliteDataReader["woodcost"].ToString());
		    int metalcost = Int32.Parse(sqliteDataReader["metalcost"].ToString());
		    int daystocomplete = Int32.Parse(sqliteDataReader["daystocomplete"].ToString());
		    int daysworkedon = Int32.Parse(sqliteDataReader["daysworkedon"].ToString());
		    Building building = new Building(type, woodcost, metalcost, daysworkedon, daystocomplete, true);
		    
		    village.Buildings.Add(building);
	    }
	    connection.Close();
    }
    public void LoadProject(Village village)
    {
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);

	    string data = "SELECT * FROM Projects";
	    SqliteCommand command = new SqliteCommand(data, connection);
	    connection.Open();
	    SqliteDataReader sqliteDataReader = command.ExecuteReader();

	    while (sqliteDataReader.Read())
	    {
		    string type = sqliteDataReader["type"].ToString();
		    int woodcost = Int32.Parse(sqliteDataReader["woodcost"].ToString());
		    int metalcost = Int32.Parse(sqliteDataReader["metalcost"].ToString());
		    int daystocomplete = Int32.Parse(sqliteDataReader["daystocomplete"].ToString());
		    int daysworkedon = Int32.Parse(sqliteDataReader["daysworkedon"].ToString());
		    Building building = new Building(type, woodcost, metalcost, daysworkedon, daystocomplete, false);
		    
		    village.Projects.Add(building);
	    }
	    connection.Close();
    }
    public void Load(Village village)
    {
	    CreateDatabase();
	    //Village village = new Village();
	    SqliteConnection connection = new SqliteConnection(_dataBaseString);

	    string data = "SELECT * FROM Village";
	    SqliteCommand command = new SqliteCommand(data, connection);
	    connection.Open();
	    SqliteDataReader sqliteDataReader = command.ExecuteReader();

	    while (sqliteDataReader.Read())
	    {
		    village.Food = Int32.Parse(sqliteDataReader["food"].ToString());
		    village.Wood = Int32.Parse(sqliteDataReader["wood"].ToString());
		    village.Metal = Int32.Parse(sqliteDataReader["metal"].ToString());
		    village.WoodWorkerAmount = Int32.Parse(sqliteDataReader["woodworkeramount"].ToString());
		    village.MetalWorkerAmount = Int32.Parse(sqliteDataReader["metalworkeramount"].ToString());
		    village.FoodWorkerAmount = Int32.Parse(sqliteDataReader["foodworkeramount"].ToString());
		    village.DaysPassed = Int32.Parse(sqliteDataReader["dayspassed"].ToString());
		    village.NameNumber = Int32.Parse(sqliteDataReader["namenumber"].ToString());
		    village.HouseCount = Int32.Parse(sqliteDataReader["housenumber"].ToString());
	    }
	    LoadWorker(village);
	    LoadBuilding(village);
	    LoadProject(village);
	    if (village.Buildings.Count == 0)
	    {
		    village.AddStartStats();
	    }
	    DeleteRows();
	    connection.Close();

	    //return village;
    }

    public void LoadForMockTest(Village village)//Method used for mock test in assignment, has no real use
    {
	    village.Wood = GetWood();
    }

    public virtual int GetWood()//Method used for mock test in assignment, has no real use
    {
	    return 10;
    }

}