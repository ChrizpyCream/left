using System;
using System.Threading;

class Version2
{
    // Create 5 forks as simple objects to lock on
    static object[] forks = new object[5];

    static void Main(string[] args)
    {
        // Set up the 5 forks
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new object();
        }

        Console.WriteLine("--- VERSION 2: Mixed Lock Order (No Deadlock) ---");

        // Create and start the 5 professor threads
        for (int i = 0; i < 5; i++)
        {
            int professorNumber = i;
            Thread t = new Thread(() => Professor(professorNumber));
            t.Start();
        }
    }

    static void Professor(int id)
    {
        int leftFork = id;
        int rightFork = (id + 1) % 5; // Wraps around to 0 for Professor 4

        while (true)
        {
            // Even professors (0, 2, 4) grab LEFT then RIGHT
            if (id % 2 == 0)
            {
                lock (forks[leftFork])
                {
                    Console.WriteLine("Professor " + id + " grabbed LEFT fork (" + leftFork + ")");
                    Thread.Sleep(100);

                    lock (forks[rightFork])
                    {
                        Console.WriteLine("Professor " + id + " grabbed RIGHT fork (" + rightFork + ") and takes a bite!");
                        Thread.Sleep(300); // Eating
                    }
                }
            }
            // Odd professors (1, 3) grab RIGHT then LEFT
            else
            {
                lock (forks[rightFork])
                {
                    Console.WriteLine("Professor " + id + " grabbed RIGHT fork (" + rightFork + ")");
                    Thread.Sleep(100);

                    lock (forks[leftFork])
                    {
                        Console.WriteLine("Professor " + id + " grabbed LEFT fork (" + leftFork + ") and takes a bite!");
                        Thread.Sleep(300); // Eating
                    }
                }
            }

            // Forks are put back down after leaving the lock blocks
            Console.WriteLine("Professor " + id + " put both forks down and is resting/talking.");
            Thread.Sleep(300); // Resting and talking
        }
    }
}