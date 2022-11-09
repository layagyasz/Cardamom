using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom.Ui.Controller
{
    public interface IControlled
    {
        IController Controller { get; }
    }
}
