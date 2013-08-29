using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace EmpireUpdate
{
    public interface IEmpireUpdate
    {
        string AppName { get; }
        string AppID { get; }
        Assembly AppAssembly { get; }
        Icon AppIcon { get; }
        Uri UpdateXmlLoc { get; }
        Form Context { get; }
    }
}
