namespace VillageOfTesting;

public class RandomNumberGenerator
{
    public virtual int ReturnRandomNumber()
    {
        Random random = new Random();
        int randomNumber = random.Next(0, 4);
        
        return randomNumber;
    }
}