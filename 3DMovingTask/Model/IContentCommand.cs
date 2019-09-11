using System.Windows.Input;

namespace _3DMovingTask.Model
{
    interface IContentCommand
    {
        ICommand Load { get; set; }
        ICommand Clear { get; set; }
    }
}
