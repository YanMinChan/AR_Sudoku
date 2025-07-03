using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameCommandManager
{
    private Stack<IGameCommand> _history = new Stack<IGameCommand>();

    public void ExecuteCommand(IGameCommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void Undo()
    {
        if (_history.Count > 0) 
        { 
            IGameCommand last = _history.Pop();
            last.Undo();
            GameLog.Instance.WriteToLog("I am undoing");
        }
    }

    public void ResetHistory()
    {
        _history.Clear();
    }
}
