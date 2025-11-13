using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoHelper.Managers;
using ToDoHelper.Models;
using ToDoHelper.Commands;

namespace ToDoHelper.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public ICommand AddTaskCommand { get; }
        public ICommand SaveCommand { get; }

        public MainViewModel()
        {
            Tasks = new ObservableCollection<TaskItem>(TaskManager.LoadTasks());
            AddTaskCommand = new RelayCommand(AddTask);
            SaveCommand = new RelayCommand(SaveTasks);
        }

        private void AddTask(object parameter)
        {
            Tasks.Add(new TaskItem { Title = "Új feladat", IsDone = false });
        }

        private void SaveTasks(object parameter)
        {
            TaskManager.SaveTasks(new List<TaskItem>(Tasks));
        }
    }
}
