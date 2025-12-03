using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoHelper.Models;
using ToDoHelper.ViewModels;

namespace ToDoHelper;

public partial class MainWindow : Window
{
    private Point _dragStartPoint;
    private Border _currentDropIndicator = null;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void TaskList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(null);
    }

    private void TaskList_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        var diff = e.GetPosition(null) - _dragStartPoint;

        if (e.LeftButton == MouseButtonState.Pressed &&
            (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
             Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem != null)
                {
                    DragDrop.DoDragDrop(listBox, listBox.SelectedItem, DragDropEffects.Move);
                }
            }
        }
    }

    private void TaskList_Drop(object sender, DragEventArgs e)
    {
        if (sender is not ListBox listBox) return;

        var droppedData = e.Data.GetData(typeof(TaskItem)) as TaskItem;
        var target = (e.OriginalSource as FrameworkElement)?.DataContext as TaskItem;

        if (droppedData == null || target == null || droppedData == target)
            return;

        var viewModel = DataContext as MainViewModel;

        int oldIndex = viewModel.Tasks.IndexOf(droppedData);
        int newIndex = viewModel.Tasks.IndexOf(target);

        if (oldIndex != newIndex)
        {
            viewModel.Tasks.Move(oldIndex, newIndex);
            viewModel.SaveTasks(null);
        }

        if (_currentDropIndicator != null)
        {
            AnimateBorderColor(_currentDropIndicator, Colors.Transparent);
            _currentDropIndicator = null;
        }
    }

    private void TaskList_PreviewDragOver(object sender, DragEventArgs e)
    {
        var pos = e.GetPosition(TaskList);
        var result = VisualTreeHelper.HitTest(TaskList, pos);

        if (result == null) return;

        var element = result.VisualHit as DependencyObject;

        while (element != null && element is not Border)
            element = VisualTreeHelper.GetParent(element);

        var border = element as Border;

        if (_currentDropIndicator == border)
            return;

        if (_currentDropIndicator != null)
        {
            AnimateBorderColor(_currentDropIndicator, Colors.Transparent);
        }

        if (border != null)
        {
            AnimateBorderColor(border, Colors.DeepSkyBlue);
        }

        _currentDropIndicator = border;
    }

    private void AnimateBorderColor(Border border, Color toColor)
    {
        var animation = new ColorAnimation
        {
            To = toColor,
            Duration = TimeSpan.FromMilliseconds(200)
        };

        border.BorderBrush = new SolidColorBrush(Colors.Transparent);
        border.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
    }
}