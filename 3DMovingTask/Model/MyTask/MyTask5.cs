using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using _3DMovingTask.Model.FileDialog;
using HelixToolkit.Wpf;

namespace _3DMovingTask.Model.MyTask
{
    class MyTask5:MyTask, IMoveContent
    {
        private Thread _myThread;
        private bool _isCancel;

        public MyTask5(string name, int number, IFileDialog dialog, HelixViewport3D viewport) : base(name, number, dialog, viewport)
        {
            MaxPointOz = 25;
            MinPointOz = -25;

            Start = new DelegateCommand(StartTransformation);
            Stop = new DelegateCommand(StopTask);
            _myThread = new Thread(Move);
        }

        private void Move()
        {
            while (!_isCancel)
            {
                Model3DGroup group = new ModelImporter().Load(CurrentModelPath);
                Point3D p = group.Bounds.Location;
                Vector3D vector = new Vector3D(p.X, p.Y, p.Z); 
                Vector3D maxVector = new Vector3D(p.X, p.Y, MaxPointOz);
                Vector3D minVector = new Vector3D(p.X,p.Y,MinPointOz);
                if (MoveElementToMax(vector, maxVector))
                {
                    MoveElementToMin(maxVector, vector);
                }

                if (MoveElementToMin(vector, minVector))
                {
                    MoveElementToMax(minVector, vector);
                }
            }
        }

        private bool MoveElementToMax(Vector3D zeroVector3D, Vector3D toPosition)
        {
            for (; zeroVector3D.Z < toPosition.Z; zeroVector3D.Z++)
            {
                var vector3 = new Vector3D(zeroVector3D.X, zeroVector3D.Y, zeroVector3D.Z);
                TaskContent?.Dispatcher?.Invoke(() =>
                {
                    if (TaskContent == null)
                    {
                        return;
                    }
                    TaskContent.Transform = new TranslateTransform3D(vector3);
                });
                Thread.Sleep(50);
            }
            return true;
        }

        private bool MoveElementToMin(Vector3D zeroVector3D, Vector3D toPosition)
        {
            for (; zeroVector3D.Z > toPosition.Z; zeroVector3D.Z--)
            {
                var vector3 = new Vector3D(zeroVector3D.X, zeroVector3D.Y, zeroVector3D.Z);
                TaskContent?.Dispatcher?.Invoke(() =>
                {
                    if (TaskContent == null)
                    {
                        _myThread.Abort();
                        return;
                    }
                    TaskContent.Transform = new TranslateTransform3D(vector3);
                });
                Thread.Sleep(50);
            }
            return true;
        }
        public void StartTransformation()
        {
            if (_isCancel)
            {
                _isCancel = false;
            }

            if (TaskContent == null)
            {
                MessageBox.Show("Модель не загружена", "Attention!", MessageBoxButton.OK, MessageBoxImage.Exclamation,
                    MessageBoxResult.OK);
                return;
            }

            if (_myThread.ThreadState == ThreadState.Aborted)
            {
                _myThread = new Thread(Move);
            }

            if (_myThread.ThreadState == ThreadState.Running||_myThread.ThreadState==ThreadState.WaitSleepJoin)
            {
                return;
            }
            _myThread.Start();
        }

        public void StopTask()
        {
            if (!_isCancel)
            {
                _isCancel = true;
            }
            _myThread.Abort();
        }

        public double MaxPointOz { get; set; }

        public double MinPointOz { get; set; }

        public ICommand Start { get; set; }
        public ICommand Stop { get; set; }
    }
}
