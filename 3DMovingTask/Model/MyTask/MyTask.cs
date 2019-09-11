using System;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using _3DMovingTask.Model.FileDialog;
using HelixToolkit.Wpf;

namespace _3DMovingTask.Model.MyTask
{
    public class MyTask:Observable, IContentCommand
    {
        private readonly IFileDialog _dialog;
        private readonly IHelixViewport3D _viewport;
        private string _currentModelPath;
        private Dispatcher _dispatcher;
        private const string Filter = "Objects files (*.obj)|*.obj";


        private Model3D _currentModel;

        public string Name { get; set; }
        public int Number { get; set; }

        public Model3D TaskContent
        {
            get => _currentModel;
            set
            {
                _currentModel = value;
                OnPropertyChanged(nameof(TaskContent));
            }
        }

        public MyTask(string name, int number, IFileDialog dialog, HelixViewport3D viewport)
        {
            _viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
            Name = name;
            Number = number;
            _dispatcher = Dispatcher.CurrentDispatcher;
            _dialog = dialog;
            Load = new DelegateCommand(LoadFile);
            Clear = new DelegateCommand(ClearContent);
        }

        private void ClearContent()
        {
            TaskContent = null;
        }

        public ICommand Load { get; set; }

        public void Loading()
        {
            LoadFile();
        }
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

        private void LoadFile()
        {
            CurrentModelPath = _dialog.LoadFile("models", null, Filter);
            TaskContent = LoadModel(CurrentModelPath);
            _viewport.ZoomExtents(0);
        }
        private Model3D LoadModel(string model3DPath)
        {
            var mi = new ModelImporter();
            var model = mi.Load(model3DPath, _dispatcher);
            return model;
        }
    }
}
