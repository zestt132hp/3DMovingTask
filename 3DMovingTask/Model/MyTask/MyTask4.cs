using System.Windows.Media.Media3D;
using _3DMovingTask.Model.FileDialog;
using HelixToolkit.Wpf;

namespace _3DMovingTask.Model.MyTask
{
    class MyTask4:MyTask
    {
        private readonly IHelixViewport3D _viewport;
        public MyTask4(string name, int number, IFileDialog dialog, HelixViewport3D viewport) : base(name, number, dialog, viewport)
        {
            _viewport = viewport;
        }

        public void DrawBoundingLines()
        {
            if (TaskContent == null)
                return;
            Rect3D rect = TaskContent.Bounds;
            LinesVisual3D line1 = new LinesVisual3D();
            LinesVisual3D line2 = new LinesVisual3D();
            LinesVisual3D line3 = new LinesVisual3D();
            LinesVisual3D line4 = new LinesVisual3D();
            Point3D maxPosition1 = new Point3D(rect.SizeX, 0, 0);
            Point3D maxPosition2 = new Point3D(-rect.SizeX, 0, 0);
            Point3D maxPosition3 = new Point3D(0, rect.SizeY, 0);
            Point3D maxPosition4 = new Point3D(0, -rect.SizeY, 0);
            Point3D nullPoint = new Point3D(0, 0, 0);
            line1.Points = new Point3DCollection() { nullPoint, maxPosition1 };
            line2.Points = new Point3DCollection() { nullPoint, maxPosition2 };
            line3.Points = new Point3DCollection() { nullPoint, maxPosition3 };
            line4.Points = new Point3DCollection() { nullPoint, maxPosition4 };
            _viewport.Viewport.Children.Add(line1);
            _viewport.Viewport.Children.Add(new BoundingBoxVisual3D() { BoundingBox = rect });
            _viewport.Viewport.Children.Add(line2);
            _viewport.Viewport.Children.Add(line3);
            _viewport.Viewport.Children.Add(line4);
        }
    }
}
