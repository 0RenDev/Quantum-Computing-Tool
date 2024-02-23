using System.Diagnostics;
using System.Numerics;
using Quantum_Computing;

public class speedTesting
{

    static void Main()
    {
        double thouOur = timeTestOurImplement(1000);
        double thouSys = timeTestSysImplement(1000);

        double tenthouOur = timeTestOurImplement(10000);
        double tenthouSys = timeTestSysImplement(10000);

        double hundthouSys = timeTestSysImplement(100000);
        double hundthouOur = timeTestOurImplement(100000);

        double milOur = timeTestOurImplement(1000000);
        double milSys = timeTestSysImplement(1000000);

        double tenMilSys = timeTestSysImplement(10000000);
        double tenMilOur = timeTestOurImplement(10000000);

        Console.WriteLine("For the results, the sums of the time taken for each operation ( +, *, /) was subtracted from each other giving the difference ");

        double thouDiff = thouSys - thouOur;
        Console.WriteLine($"Our implmentation was {thouDiff} milliseconds faster for 1 thousand numbers");
        double tenthouDiff = tenthouSys - tenthouOur;
        Console.WriteLine($"Our implmentation was {tenthouDiff} milliseconds faster 10 thousand numbers");
        double hundthouDiff = hundthouSys - hundthouOur;
        Console.WriteLine($"Our implmentation was {hundthouDiff} milliseconds faster 100 thousand numbers");
        double milDiff = milSys - milOur;
        Console.WriteLine($"Our implmentation was {milDiff} milliseconds faster 1 million numbers");
        double tentmilDiff = tenMilSys - tenMilOur;
        Console.WriteLine($"Our implmentation was {tentmilDiff} milliseconds faster 10 million numbers");

    }

    // generates a given given length array of complex numbers
    // uses system.numerics representation
    public static Complex[] generateComplexArrayM(int maxNum)
    {
        Complex[] elements = new Complex[maxNum];
        Random rand = new Random();

        for (int i = 0; i < elements.Length; i++)
        {
            // generate random double complex between -10 and 10 
            elements[i] = new Complex(((rand.NextDouble() * 20) - 10), ((rand.NextDouble() * 20) - 10));
        }
        return elements;
    }

    // generates a given given length array of complex numbers
    // uses ComplexNumbers representation made by Griffith
    public static ComplexNumber[] generateComplexArrayO(int maxNum)
    {
        ComplexNumber[] elements = new ComplexNumber[maxNum];
        Random rand = new Random();

        for (int i = 0; i < elements.Length; i++)
        {
            // generate random double complex between -10 and 10 
            elements[i] = new ComplexNumber(((rand.NextDouble() * 20) - 10), ((rand.NextDouble() * 20) - 10));
        }
        return elements;
    }

    // tests the operation speed
    static double timeTestSysImplement(int m)
    {
        double[] results = new double[3];

        Stopwatch stopwatch = new Stopwatch();

        Complex[] sysImp1 = generateComplexArrayM(m);
        Complex[] sysImp2 = generateComplexArrayM(m);

        Console.WriteLine("Starting test of system.numerics implementation of complex numbers with array size: " + m); ;
        Console.WriteLine("Adding:");
        stopwatch.Start();
        for(int i = 0;i < sysImp1.Length;i++)
        {
            sysImp1[i] = sysImp1[i] + sysImp2[i];
        }
        stopwatch.Stop();
        TimeSpan timeadd = stopwatch.Elapsed;
        results[0] = timeadd.TotalMilliseconds;
        Console.WriteLine($"result: {results[0]} milliseconds");

        Console.WriteLine("Multiplying:");
        stopwatch.Start();
        for (int i = 0; i < sysImp1.Length; i++)
        {
            sysImp1[i] = sysImp1[i] * sysImp2[i];
        }
        stopwatch.Stop();
        TimeSpan timemult = stopwatch.Elapsed;
        results[1] = timemult.TotalMilliseconds;
        Console.WriteLine($"result: {results[1]} milliseconds");

        Console.WriteLine("Dividing:");
        stopwatch.Start();
        for (int i = 0; i < sysImp1.Length; i++)
        {
            sysImp2[i] = sysImp1[i] / sysImp2[i];
        }
        stopwatch.Stop();
        TimeSpan timediv = stopwatch.Elapsed;
        results[2] = timediv.TotalMilliseconds;
        Console.WriteLine($"result: {results[2]} milliseconds");
        Console.WriteLine();

        return results.Sum();
    }
    static double timeTestOurImplement(int m)
    {
        double[] results = new double[3];

        Stopwatch stopwatch = new Stopwatch();

        ComplexNumber[] ourImp1 = generateComplexArrayO(m);
        ComplexNumber[] ourImp2 = generateComplexArrayO(m);

        Console.WriteLine("Starting test of Griffith's implementation of complex numbers with array size: " + m); ;
        Console.WriteLine("Adding:");
        stopwatch.Start();
        for (int i = 0; i < ourImp1.Length; i++)
        {
            ourImp1[i] = ourImp1[i] + ourImp2[i];
        }
        stopwatch.Stop();
        TimeSpan timeadd = stopwatch.Elapsed;
        results[0] = timeadd.TotalMilliseconds;
        Console.WriteLine($"result: {results[0]} milliseconds");

        Console.WriteLine("Multiplying:");
        stopwatch.Start();
        for (int i = 0; i < ourImp1.Length; i++)
        {
            ourImp1[i] = ourImp1[i] * ourImp2[i];
        }
        stopwatch.Stop();
        TimeSpan timemult = stopwatch.Elapsed;
        results[1] = timemult.TotalMilliseconds;
        Console.WriteLine($"result: {results[1]} milliseconds");

        Console.WriteLine("Dividing:");
        stopwatch.Start();
        for (int i = 0; i < ourImp1.Length; i++)
        {
            ourImp2[i] = ourImp1[i] / ourImp2[i];
        }
        stopwatch.Stop();
        TimeSpan timediv = stopwatch.Elapsed;
        results[2] = timediv.TotalMilliseconds;
        Console.WriteLine($"result: {results[2]} milliseconds");
        Console.WriteLine();

        return results.Sum();
    }
}
