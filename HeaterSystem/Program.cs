using HeaterSystem;

ITemperatureSensor temperatureSensor = new TemperatureSensorOpenWeather();
IHeatingElement heatingElement = new HeatingElementStub();

Thermostat thermostat = new Thermostat(temperatureSensor, heatingElement);

while (true)
{
    thermostat.Work();
    Thread.Sleep(5000);
}