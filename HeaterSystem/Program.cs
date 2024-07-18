using HeaterSystem;

ITemperatureSensor temperatureSensor = new TemperatureSensor();
IHeatingElement heatingElement = new HeatingElement();

Thermostat thermostat = new Thermostat(temperatureSensor, heatingElement);

while (true)
{
    thermostat.Work();
    Thread.Sleep(5000);
}