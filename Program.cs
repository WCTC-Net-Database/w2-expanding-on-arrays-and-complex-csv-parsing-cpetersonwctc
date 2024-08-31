using System;
using System.IO;
using System.Reflection.Emit;
using System.Xml.Linq;

class Program
{
    static string[] lines;

    static void Main()
    {
        string filePath = "input.csv";

        while (true)
        {
            lines = File.ReadAllLines(filePath);
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Add Character");
            Console.WriteLine("3. Level Up Character");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllCharacters(lines);
                    break;
                case "2":
                    AddCharacter(ref lines);
                    break;
                case "3":
                    LevelUpCharacter(lines);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllCharacters(string[] lines)
    {
        // Skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            string name;
            int commaIndex;

            

            // Check if the name is quoted
            if (line.StartsWith("\""))
            {
                // TODO: Find the closing quote and the comma right after it
                // TODO: Remove quotes from the name if present and parse the name
                line = line.Substring(1, line.Length - 1);
                name = line.Substring(0, line.IndexOf('"'));
                line = line.Substring(line.IndexOf('"')+2,line.Length-line.IndexOf('"')-2);
            }
            else
            {
                // TODO: Name is not quoted, so store the name up to the first comma
                name = line.Substring(0, line.IndexOf(','));
                line = line.Substring(line.IndexOf(',')+1, line.Length- line.IndexOf(',')-1);
            }
            
            var charData = line.Split(",");
            string characterClass = charData[0];
            int level = Convert.ToInt32(charData[1]);
            int hitPoints = Convert.ToInt32(charData[2]);

            // TODO: Parse equipment noting that it contains multiple items separated by '|'
            string[] equipment = charData[3].Split('|');

            // Display character information
            Console.WriteLine($"Name: {name}, Class: {characterClass}, Level: {level}, HP: {hitPoints}, Equipment: {string.Join(", ", equipment)}");
        }
    }

    static void AddCharacter(ref string[] lines)
    {
        // TODO: Implement logic to add a new character
        // Prompt for character details (name, class, level, hit points, equipment)
        // DO NOT just ask the user to enter a new line of CSV data or enter the pipe-separated equipment string
        // Append the new character to the lines array

        Console.Write("Give the player a name (No Quotes): ");
        var name = Console.ReadLine();
        if (name != null && name.Contains(","))
        {
            name = '"' + name + '"';
        }
        Console.Write("Give the player a class: ");
        var charClass = Console.ReadLine();
        Console.Write("Give the player a level: ");
        int level = Convert.ToInt32(Console.ReadLine());
        Console.Write("Give the player a max HP: ");
        int hp = Convert.ToInt32(Console.ReadLine());

        string[] items = { };
        while (true)
        {
            Console.Write("Add item? (y/n): ");
            var addItem = Console.ReadLine()?.ToLower();
            if (addItem == "n" || addItem == "no")
            {
                break;
            }
            else if (addItem == "y" || addItem == "yes")
            {
                Console.WriteLine("Give the item a name: ");
                string? newItem = Console.ReadLine();
                if (newItem != null)
                {
                    items = items.Append(newItem).ToArray();
                }
            }
        }
        StreamWriter writer = new StreamWriter("input.csv", true);
        writer.WriteLine($"{name},{charClass},{level},{hp},{string.Join("|",items)}");
        writer.Close();
    }

    static void LevelUpCharacter(string[] lines)
    {
        Console.Write("Enter the name of the character to level up: ");
        string nameToLevelUp = Console.ReadLine();

        // Loop through characters to find the one to level up
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            // TODO: Check if the name matches the one to level up
            // Do not worry about case sensitivity at this point
            if (line.Contains(nameToLevelUp))
            {

                string name;
                if (line.StartsWith("\""))
                {
                    line = line.Substring(1, line.Length - 1);
                    name = line.Substring(0, line.IndexOf('"'));
                    line = line.Substring(line.IndexOf('"') + 2, line.Length - line.IndexOf('"') - 2);
                }
                else
                {
                    name = line.Substring(0, line.IndexOf(','));
                    line = line.Substring(line.IndexOf(',') + 1, line.Length - line.IndexOf(',') - 1);
                }
                var charData = line.Split(",");
                string characterClass = charData[0];
                int level = Convert.ToInt32(charData[1]);
                int hitPoints = Convert.ToInt32(charData[2]);
                string equipment = charData[3];

                level++;

                //lines[i] = $"{name},{characterClass},{level},{hitPoints},{equipment}";

                StreamWriter writer = new StreamWriter("input.csv", false);
                
                writer.WriteLine("Name,Class,Level,HP,Equipment");
                for (int b = 1; i < lines.Length; i++)
                {
                    if (b == i)
                    {
                        writer.WriteLine($"\"{name}\",{characterClass},{level},{hitPoints},{equipment}");
                    }
                    else
                    {
                        string lineAgain = lines[i];
                        string lineName;
                        if (lineAgain.StartsWith("\""))
                        {
                            lineAgain = lineAgain.Substring(1, lineAgain.Length - 1);
                            lineName = lineAgain.Substring(0, lineAgain.IndexOf('"'));
                            lineAgain = lineAgain.Substring(lineAgain.IndexOf('"') + 2, lineAgain.Length - lineAgain.IndexOf('"') - 2);
                        }
                        else
                        {
                            lineName = lineAgain.Substring(0, lineAgain.IndexOf(','));
                            lineAgain = lineAgain.Substring(lineAgain.IndexOf(',') + 1, lineAgain.Length - lineAgain.IndexOf(',') - 1);
                        }
                        var lineCharData = lineAgain.Split(",");
                        string lineCharacterClass = charData[0];
                        int lineLevel = Convert.ToInt32(charData[1]);
                        int lineHitPoints = Convert.ToInt32(charData[2]);
                        string lineEquipment = charData[3];
                        writer.WriteLine($"\"{lineName}\",{lineCharacterClass},{lineLevel},{lineHitPoints},{lineEquipment}");
                    }
                }
                writer.Close();
                Console.WriteLine($"Character {name} leveled up to level {level}!");
                break;
            }
        }
    }
}