using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using _3DMovingTask.Model.FileDialog;
using _3DMovingTask.Model.MyTask;
using HelixToolkit.Wpf;

namespace _3DMovingTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _maxSize;
        public ObservableCollection<MyTask> MyTaskList { get; set; }
        public HelixViewport3D LoadScene { get; set; }
        public Grid Elements { get; set; }

        public MyTask SelectedTask { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MyTaskList = new ObservableCollection<MyTask>()
            {
                new MyTask("Task 3", 3, new Dialog(), View1),
                new MyTask4("Task 4", 4, new Dialog(), View1),
                new MyTask5("Task 5", 5, new Dialog(), View1)
            };
            Controls.ItemsSource = MyTaskList;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaxButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_maxSize)
            {
                WindowState = WindowState.Maximized;
                _maxSize = true;
            }
            else
            {
                _maxSize = false;
                WindowState = WindowState.Normal;
            }
        }

        private void MinButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Elements?.Children.Clear();
            Elements = new Grid();
            var boxItem = sender as ListBoxItem;
            var selectedTask = boxItem?.DataContext as MyTask;
            SelectedTask = selectedTask;
            switch (selectedTask?.Number)
            {
                case 3:
                case 4:
                {
                    Elements.ColumnDefinitions.Add(new ColumnDefinition());
                    Elements.ColumnDefinitions.Add(new ColumnDefinition());
                    Thickness thickness = new Thickness(20, 20, 20, 20);
                    Button buttonLoad = new Button
                    {
                        Command = selectedTask.Load,
                        Content = nameof(selectedTask.Load), Width = 100,
                        Margin = thickness, Height = 50
                    };
                    Button buttonClear = new Button
                    {
                        Command = selectedTask.Clear, Content = nameof(selectedTask.Clear), Width = 100,
                        Margin = thickness, Height = 50
                    };
                    Grid.SetColumn(buttonLoad, 0);
                    Grid.SetColumn(buttonClear, 1);
                    Elements.Children.Add(buttonClear);
                    Elements.Children.Add(buttonLoad);
                    if (selectedTask is MyTask4 task)
                    {
                        task.PropertyChanged += (o, args) =>  task.DrawBoundingLines(); 
                    }
                }
                    break;
                case 5:
                {
                    Elements = CreateElementsForTask5((MyTask5) selectedTask);
                }
                    break;
            }

            if (!(SelectedTask is MyTask4))
            {
                RemoveElements<BoundingBoxVisual3D>();
                RemoveElements<LinesVisual3D>();
            }
            DataContext = selectedTask;
            ToolsButton.Items.Add(Elements);
        }

        private void RemoveElements<T>()
        {
            for (int i = 0; i < View1.Children?.Count; i++)
            {
                if (View1.Children[i].GetType() == typeof(T))
                {
                    View1.Children.RemoveAt(i);
                }
            }
            if (View1.Children!=null && View1.Children.Any(x => x.GetType() == typeof(T)))
            {
                RemoveElements<T>();
            }

        }

        private Grid CreateElementsForTask5(MyTask5 task)
        {
            Thickness thickness = new Thickness(20, 20, 20, 20);
            Grid grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(), new ColumnDefinition(), new ColumnDefinition(), new ColumnDefinition(),
                    new ColumnDefinition(), new ColumnDefinition()
                },
                RowDefinitions = {new RowDefinition(), new RowDefinition()}
            };
            Button buttonLoad = new Button()
                {Command = task.Load, Content = nameof(task.Load), Width = 75, Margin = thickness, Height = 25};
            Button buttonClear = new Button()
                {Command = task.Clear, Content = nameof(task.Clear), Width = 75, Height = 25};
            Button buttonStart = new Button()
                {Command = task.Start, Content = nameof(task.Start), Width = 75, Height = 25};
            Button buttonStop = new Button()
                {Command = task.Stop, Content = nameof(task.Stop), Width = 75, Height = 25};
            Thickness thickness1 = new Thickness(0,25,0,0);
            Label minLabel = new Label() {Content = "MIN Z", Margin = thickness1};
            Label maxLabel = new Label() {Content = "MAX Z"};
            Slider minSlider = new Slider() {Minimum = -25, Maximum = -5, Margin = thickness1};
            Slider maxSlider = new Slider() {Minimum = 5, Maximum = 25};
            task.MaxPointOz = maxSlider.Value;
            task.MinPointOz = minSlider.Value;
            minSlider.ValueChanged += MinSlider_ValueChanged;
            maxSlider.ValueChanged += MaxSlider_ValueChanged;
            grid.Children.Add(SetGridProperty(buttonLoad, 0, 0, 2));
            grid.Children.Add(SetGridProperty(buttonClear, 1, 0, 2));
            grid.Children.Add(SetGridProperty(minLabel, 2));
            grid.Children.Add(SetGridProperty(minSlider, 3));
            grid.Children.Add(SetGridProperty(buttonStart, 4, 0, 2));
            grid.Children.Add(SetGridProperty(buttonStop, 5, 0, 2));
            grid.Children.Add(SetGridProperty(maxLabel, 2, 1));
            grid.Children.Add(SetGridProperty(maxSlider, 3, 1));
            return grid;
        }

        private void MaxSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SelectedTask is MyTask5 task)
            {
                task.MaxPointOz = ((Slider)sender).Value;
            }
        }

        private void MinSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SelectedTask is MyTask5 task)
            {
                task.MinPointOz = ((Slider)sender).Value;
            }
        }

        private UIElement SetGridProperty(UIElement element, int column, int row = 0, int rowSpan = 1)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            Grid.SetRowSpan(element, rowSpan);
            return element;
        }

    }
}
