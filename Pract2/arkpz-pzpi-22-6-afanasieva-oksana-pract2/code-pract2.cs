//До рефакторингу
public class Account
{
    public int Withdraw(decimal amount)
    {
        if (amount > Balance)
        {
            return -1;
        }

        Balance -= amount;
        return 0;
    }

    public decimal Balance { get; private set; }
}

public class Program
{
    public static void Main()
    {
        Account account = new Account();
        int result = account.Withdraw(100);

        if (result == 1)
        {
            Console.WriteLine("Insufficient funds.");
        }
    }
}

//Після рефакторингу
public class Account
{
    public void Withdraw(decimal amount)
    {
        if (amount > Balance)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }

        Balance -= amount;
    }

    public decimal Balance { get; private set; }
}

public class Program
{
    public static void Main()
    {
        try
        {
            Account account = new Account();
            account.Withdraw(100);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

//До рефакторингу
public class Calculator
{
    public int Divide(int numerator, int denominator)
    {
        try
        {
            return numerator / denominator;
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Cannot divide by zero.");
            return 0;
        }
    }
}

public class Program
{
    public static void Main()
    {
        Calculator calculator = new Calculator();
        int result = calculator.Divide(10, 0);
        Console.WriteLine($"Result: {result}");
    }
}

//Після рефакторингу
public class Calculator
{
    public int Divide(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            Console.WriteLine("Cannot divide by zero.");
            return 0;
        }

        return numerator / denominator;
    }
}

public class Program
{
    public static void Main()
    {
        Calculator calculator = new Calculator();
        int result = calculator.Divide(10, 0);
        Console.WriteLine($"Result: {result}");
    }
}

//До рефакторингу
public class Manager
{
    private Worker worker = new Worker();

    public string GetWorkerName()
    {
        return worker.Name;
    }
}

public class Worker
{
    public string Name { get; set; } = "Harry";
}

public class Program
{
    public static void Main()
    {
        Manager manager = new Manager();
        Console.WriteLine(manager.GetWorkerName());
    }
}


//Після рефакторингу
public class Worker
{
    public string Name { get; set; } = "Harry";
}

public class Program
{
    public static void Main()
    {
        Worker worker = new Worker();
        Console.WriteLine(worker.Name);
    }
}
