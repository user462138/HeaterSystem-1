using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaterSystem;

public class TemperatureSensor : ITemperatureSensor
{
    public string Url
    {
        get { throw new NotImplementedException(); }
        set { throw new NotImplementedException(); }
    }
    public double GetTemperature()
    {
        throw new NotImplementedException();
    }
}