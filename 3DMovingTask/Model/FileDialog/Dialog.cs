using Microsoft.Win32;

namespace _3DMovingTask.Model.FileDialog
{
    class Dialog:IFileDialog
    {
        public string LoadFile(string initialDirectory, string defaultPath, string filter)
        {
            var d = new OpenFileDialog {InitialDirectory = initialDirectory, FileName = defaultPath, Filter = filter};
            if (!d.ShowDialog().Value)
            {
                return null;
            }

            return d.FileName;
        }
    }
}
