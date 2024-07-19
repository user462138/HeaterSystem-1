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

    public Thermostat(ITemperatureSensor temperatureSensor, IHeatingElement heatingElement)
    {
        this.temperatureSensor = temperatureSensor;
        this.heatingElement = heatingElement;
    }

    public void Work()
    {
        double temperature = temperatureSensor.GetTemperature();

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
        }        //// Alternative
        //if (temperature < Setpoint - Offset)
        //{
        //    heatingElement.Enable();
        //}
        //else if (temperature > Setpoint + Offset)
        //{
        //    heatingElement.Disable();
        //}
        //else
        //{
        //  //Do Nothing
        //} 
    }
}
