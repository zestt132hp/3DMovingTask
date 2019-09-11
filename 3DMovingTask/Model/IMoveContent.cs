using System.Windows.Input;

namespace _3DMovingTask.Model
{
    interface IMoveContent
    {
        ICommand Start { get; set; }
        ICommand Stop { get; set; }
    }

}
