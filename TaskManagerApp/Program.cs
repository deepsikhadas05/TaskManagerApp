// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TaskManagerApp
{
    class Program
    {
        static string filePath = "tasks.json";
        static List<TaskItem> tasks = new List<TaskItem>();

        static void Main()
        {
            LoadTasks();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Personal Task Manager ===");
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Add Task");
                Console.WriteLine("3. Mark Task Complete");
                Console.WriteLine("4. Delete Task");
                Console.WriteLine("5. View Stats");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1": ViewTasks(); break;
                    case "2": AddTask(); break;
                    case "3": MarkComplete(); break;
                    case "4": DeleteTask(); break;
                    case "5": ShowStats(); break;
                    case "6": SaveTasks(); return;
                    default: Console.WriteLine("Invalid option."); break;
                }

                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        static void AddTask()
        {
            Console.Write("Enter task title: ");
            string title = Console.ReadLine();

            tasks.Add(new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            });

            Console.WriteLine("Task added.");
        }

        static void ViewTasks()
        {
            if (!tasks.Any())
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            foreach (var task in tasks)
            {
                Console.WriteLine($"{task.Id} - [{(task.IsCompleted ? "X" : " ")}] {task.Title} (Added: {task.CreatedAt})");
            }
        }

        static void MarkComplete()
        {
            Console.Write("Enter Task ID to mark as complete: ");
            string input = Console.ReadLine();

            var task = tasks.FirstOrDefault(t => t.Id.ToString() == input);
            if (task != null)
            {
                task.IsCompleted = true;
                Console.WriteLine("Task marked as complete.");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }

        static void DeleteTask()
        {
            Console.Write("Enter Task ID to delete: ");
            string input = Console.ReadLine();

            var task = tasks.FirstOrDefault(t => t.Id.ToString() == input);
            if (task != null)
            {
                tasks.Remove(task);
                Console.WriteLine("Task deleted.");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }

        static void ShowStats()
        {
            int total = tasks.Count;
            int completed = tasks.Count(t => t.IsCompleted);
            int pending = total - completed;

            Console.WriteLine($"Total Tasks: {total}");
            Console.WriteLine($"Completed: {completed}");
            Console.WriteLine($"Pending: {pending}");
        }

        static void SaveTasks()
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine("Tasks saved.");
        }

        static void LoadTasks()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
            }
        }
    }

    class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
