using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ToDoHelper.Models;

namespace ToDoHelper.Managers
{
    public static class TaskManager
    {
        private static readonly string filePath = "Data/tasks.json";

        public static List<TaskItem> LoadTasks()
        {
            if (!File.Exists(filePath)) return new List<TaskItem>();
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        public static void SaveTasks(List<TaskItem> tasks)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
