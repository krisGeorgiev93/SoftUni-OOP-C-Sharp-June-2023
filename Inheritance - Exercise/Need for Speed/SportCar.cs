﻿
namespace NeedForSpeed
{
    public class SportCar : Car
    {
        private const double DefaultFuelConsumtpion = 10;

        public SportCar(int horsePower, double fuel) : base(horsePower, fuel)
        {
        }

        public override double FuelConsumption => DefaultFuelConsumtpion;
    }
}
