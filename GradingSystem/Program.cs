using System;
using System.Collections.Generic;
using System.IO;

namespace GradingSystem
{
    // Custom exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // Student class
    public class Student
    {
        public string Id { get; }
        public string FullName { get; }
        public double Score { get; }

        public Student(string id, string fullName, double score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80) return "A";
            if (Score >= 70) return "B";
            if (Score >= 60) return "C";
            if (Score >= 50) return "D";
            return "F";
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {FullName}, Score: {Score}, Grade: {GetGrade()}";
        }
    }

    // Processor class
    public class StudentResultProcessor
    {
        private List<Student> students = new List<Student>();

        public List<Student> ReadStudentsFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Input file not found.");

            var lines = File.ReadAllLines(filePath);
            int lineNumber = 0;

            foreach (var line in lines)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');

                if (parts.Length < 3)
                    throw new MissingFieldException($"Line {lineNumber}: Missing fields.");

                string id = parts[0].Trim();
                string name = parts[1].Trim();
                string scoreText = parts[2].Trim();

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(scoreText))
                    throw new MissingFieldException($"Line {lineNumber}: One or more fields are empty.");

                if (!double.TryParse(scoreText, out double score))
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format.");

                if (score < 0 || score > 100)
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Score must be between 0 and 100.");

                students.Add(new Student(id, name, score));
            }

            return students;
        }

        public void WriteReportToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("=== Student Grade Report ===");
                foreach (var student in students)
                {
                    writer.WriteLine(student.ToString());
                }
            }
        }

        public void AddStudent(Student student)
        {
            students.Add(student);
        }

        public void PrintAllStudents()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Student Records ---");
            Console.ResetColor();

            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "students.txt";
            string outputFile = "report.txt";

            var processor = new StudentResultProcessor();

            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== Grading System - Processing File ===\n");
                Console.ResetColor();
                if (!File.Exists(inputFile))
{
    File.WriteAllLines(inputFile, new string[]
    {
        "S001,John Doe,85",
        "S002,Jane Smith,72",
        "S003,Bob Brown,65",
        "S004,Alice White,48"
    });
    Console.WriteLine("Input file not found. Default file created.");
}
                processor.ReadStudentsFromFile(inputFile);
                processor.WriteReportToFile(outputFile);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Initial report generated successfully: {outputFile}");
                Console.ResetColor();

                processor.PrintAllStudents();
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return;
            }
            catch (MissingFieldException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return;
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.ResetColor();
                return;
            }

            // Interactive menu for additional enhancements
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nChoose an action:");
                Console.ResetColor();
                Console.WriteLine("1. View All Students");
                Console.WriteLine("2. Add New Student");
                Console.WriteLine("3. Save Report");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        processor.PrintAllStudents();
                        break;

                    case "2":
                        Console.Write("Enter Student ID: ");
                        string id = Console.ReadLine();
                        Console.Write("Enter Full Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter Score: ");
                        if (double.TryParse(Console.ReadLine(), out double score) && score >= 0 && score <= 100)
                        {
                            processor.AddStudent(new Student(id, name, score));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Student added successfully!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid score. Must be between 0 and 100.");
                            Console.ResetColor();
                        }
                        break;

                    case "3":
                        processor.WriteReportToFile(outputFile);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Report updated successfully: {outputFile}");
                        Console.ResetColor();
                        break;

                    case "0":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting... Thank you for using the Grading System! 👋");
                        Console.ResetColor();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}
