using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UIEvents
{
    public static event Action OnUndoPressed;

    public static void UndoPressed()
    {
        OnUndoPressed?.Invoke();
    }
}
