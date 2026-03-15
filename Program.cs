using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        List<Goal> goals = new List<Goal>();
        int score = 0;
        bool running = true;

        while (running)
        {
            Console.WriteLine("\nEternal Quest Program");
            Console.WriteLine($"Current Score: {score}");
            DisplayLevel(score);

            Console.WriteLine("\nMenu Options:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Record Event");
            Console.WriteLine("4. Save Goals");
            Console.WriteLine("5. Load Goals");
            Console.WriteLine("6. Quit");

            Console.Write("Select a choice: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
                CreateGoal(goals);

            else if (choice == 2)
                ListGoals(goals);

            else if (choice == 3)
                RecordEvent(goals, ref score);

            else if (choice == 4)
                SaveGoals(goals, score);

            else if (choice == 5)
                LoadGoals(goals, ref score);

            else if (choice == 6)
                running = false;
        }
    }

    static void DisplayLevel(int score)
    {
        int level = score / 1000 + 1;
        Console.WriteLine($"Level: {level}");
    }

    static void CreateGoal(List<Goal> goals)
    {
        Console.WriteLine("\nGoal Types:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");

        Console.Write("Which type? ");
        int type = int.Parse(Console.ReadLine());

        Console.Write("Goal Name: ");
        string name = Console.ReadLine();

        Console.Write("Description: ");
        string description = Console.ReadLine();

        Console.Write("Points: ");
        int points = int.Parse(Console.ReadLine());

        if (type == 1)
        {
            goals.Add(new SimpleGoal(name, description, points));
        }

        else if (type == 2)
        {
            goals.Add(new EternalGoal(name, description, points));
        }

        else if (type == 3)
        {
            Console.Write("Target Count: ");
            int target = int.Parse(Console.ReadLine());

            Console.Write("Bonus Points: ");
            int bonus = int.Parse(Console.ReadLine());

            goals.Add(new ChecklistGoal(name, description, points, target, bonus));
        }
    }

    static void ListGoals(List<Goal> goals)
    {
        Console.WriteLine("\nYour Goals:");

        for (int i = 0; i < goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {goals[i].GetDetailsString()}");
        }
    }

    static void RecordEvent(List<Goal> goals, ref int score)
    {
        Console.WriteLine("\nWhich goal did you accomplish?");

        for (int i = 0; i < goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {goals[i].GetDetailsString()}");
        }

        int index = int.Parse(Console.ReadLine()) - 1;

        int points = goals[index].RecordEvent();

        score += points;

        Console.WriteLine($"You earned {points} points!");
    }

    static void SaveGoals(List<Goal> goals, int score)
    {
        Console.Write("Enter filename: ");
        string file = Console.ReadLine();

        using (StreamWriter output = new StreamWriter(file))
        {
            output.WriteLine(score);

            foreach (Goal goal in goals)
            {
                output.WriteLine(goal.GetStringRepresentation());
            }
        }
    }

    static void LoadGoals(List<Goal> goals, ref int score)
    {
        Console.Write("Enter filename: ");
        string file = Console.ReadLine();

        string[] lines = File.ReadAllLines(file);

        goals.Clear();

        score = int.Parse(lines[0]);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split("|");

            if (parts[0] == "SimpleGoal")
            {
                goals.Add(new SimpleGoal(parts[1], parts[2], int.Parse(parts[3]), bool.Parse(parts[4])));
            }

            else if (parts[0] == "EternalGoal")
            {
                goals.Add(new EternalGoal(parts[1], parts[2], int.Parse(parts[3])));
            }

            else if (parts[0] == "ChecklistGoal")
            {
                goals.Add(new ChecklistGoal(
                    parts[1],
                    parts[2],
                    int.Parse(parts[3]),
                    int.Parse(parts[4]),
                    int.Parse(parts[5]),
                    int.Parse(parts[6])
                ));
            }
        }
    }
}