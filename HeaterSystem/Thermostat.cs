using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaterSystem;

public class Thermostat
{
    private readonly ITemperatureSensor temperatureSensor;
    private readonly IHeatingElement heatingElement;

    private int failures = 0;

    private double setpoint;
    public double Setpoint
    {
        get { return setpoint; }
        set { setpoint = value; }
    }

    private double offset;
    public double Offset
    {
        get { return offset; }
        set { offset = value; }
    }

    private int maxFailures;

    public int MaxFailures
    {
        get { return maxFailures; }
        set { maxFailures = value; }
    }

    public bool InSafeMode
    {
        get { return (failures < MaxFailures) ? false : true; } 
    }


    public Thermostat(ITemperatureSensor temperatureSensor, IHeatingElement heatingElement)
    {
        this.temperatureSensor = temperatureSensor;
        this.heatingElement = heatingElement;
    }

    public void Work()
    {
        try
        {
            double temperature = temperatureSensor.GetTemperature();
            // reset number of failures
            failures = 0;

            // temperature between boundaries 
            if (temperature > Setpoint - Offset && temperature < Setpoint + Offset)
            {
                // Do nothing
            }
            // temperature less than lower boundary 
            else if (temperature < Setpoint - Offset)
            {
                heatingElement.Enable();
            }
            // temperature eqauls lower boundary 
            else if (temperature == Setpoint - Offset)
            {
                // Do nothing
            }
            // temperature higher than upper boundary 
            else if (temperature > Setpoint + Offset)
            {
                heatingElement.Disable();
            }
            // temperature eqauls upper boundary 
            else if (temperature == Setpoint + Offset)
            {
                // Do nothing
            }
            else
            {
                // Do nothing
            }
        }
        catch
        {
            failures++;
            // maximum number of failures reached
            if (failures >= MaxFailures)
            {
                heatingElement.Disable();
            }
        }

    }
}
