using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppWPFClient
{
    public interface IWindow
    {
        Action Close { get; set; }
    }
}
