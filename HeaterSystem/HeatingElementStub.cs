using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaterSystem;

public class HeatingElementStub : IHeatingElement
{
    private bool isEnabled = false;

    // public bool IsEnabled => isEnabled;
    public bool IsEnabled 
    {  
        get { return isEnabled; } 
    }

    public void Disable()
    {
        isEnabled = false;
    }

    public void Enable()
    {
        isEnabled = true;
    }
}