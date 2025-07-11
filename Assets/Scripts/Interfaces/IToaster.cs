using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IToaster
{
    ToastUI Show(string msg, float timer = 5);
}
