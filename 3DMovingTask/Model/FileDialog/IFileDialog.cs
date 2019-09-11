namespace _3DMovingTask.Model.FileDialog
{
    public interface IFileDialog
    {
        string LoadFile(string initialDirectory, string defaultPath, string filter);
    }
}
