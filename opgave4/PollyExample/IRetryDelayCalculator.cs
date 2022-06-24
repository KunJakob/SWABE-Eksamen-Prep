namespace PollyExample;

public interface IRetryDelayCalculator
{
    public TimeSpan Calculate(int attemptNumber);
}

public class ExponentialBackoffWithJitterCalculator : IRetryDelayCalculator
{
    private readonly Random random;
    private readonly object randomLock;

    public ExponentialBackoffWithJitterCalculator()
    {
        random = new Random();
        randomLock = new object();
    }
    public TimeSpan Calculate(int attemptNumber)
    {
        var jitter = 0;
        lock (randomLock) //because Random is global and we don't want multiple threads to use it at the same time
            jitter = random.Next(10, 200);

        return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber - 1)) + TimeSpan.FromMilliseconds(jitter); 
        // Attempt #	Min delay	Max delay
        //      1	    1.01 s	    1.2 s
        //      2	    2.01 s	    2.2 s
        //      3	    4.01 s	    4.2 s
    }
}