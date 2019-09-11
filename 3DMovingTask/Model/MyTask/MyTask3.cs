using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using _3DMovingTask.Model.FileDialog;
using HelixToolkit.Wpf;

namespace _3DMovingTask.Model.MyTask
{
    class MyTask3:Observable,IMyTask, IContentCommand
    {
        private readonly IFileDialog _dialog;
        private readonly IHelixViewport3D _viewport;
        private string _currentModelPath;
        private readonly Dispatcher _dispatcher;
        private const string Filter = "Objects files (*.obj)|*.obj";

        public string Name { get; set; }
        public int Number { get; set; }
        public Model3D TaskContent { get; set; }
        public ObservableCollection<UIElement> Elements { get; set; }

        public MyTask3(string name, int number, IFileDialog dialog, HelixViewport3D viewport)
        {
            _viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
            Name = name;
            Number = number;
            _dialog = dialog;
            _dispatcher = Dispatcher.CurrentDispatcher;
            Load = new DelegateCommand(LoadFile);
            Clear = new DelegateCommand(ClearContent);
        }

        private void ClearContent()
        {
            TaskContent = null;
        }

        public ICommand Load { get; set; }
        public ICommand Clear { get; set; }

        public string CurrentModelPath
        {
            get => _currentModelPath;
            set
            {
                _currentModelPath = value;
                OnPropertyChanged(nameof(CurrentModelPath));
            }
        }

        private async void LoadFile()
        {
            CurrentModelPath = _dialog.LoadFile("models", null, Filter);
            TaskContent = await LoadAsync(CurrentModelPath, false);
            _viewport.ZoomExtents(0);

        }

        private async Task<Model3DGroup> LoadAsync(string model3DPath, bool freeze)
        {
            return await Task.Factory.StartNew(() =>
            {
                var mi = new ModelImporter();
                return freeze ? mi.Load(model3DPath, null, true) : mi.Load(model3DPath, _dispatcher);
            });
        }
    }
}
