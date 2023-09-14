using EasterRaces.Core.Contracts;
using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Models.Cars.Entities;
using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Drivers.Entities;
using EasterRaces.Models.Races.Contracts;
using EasterRaces.Models.Races.Entities;
using EasterRaces.Repositories.Contracts;
using EasterRaces.Repositories.Entities;
using EasterRaces.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasterRaces.Core.Entities
{
    public class ChampionshipController : IChampionshipController
    {
        private IRepository<IDriver> drivers;
        private IRepository<IRace> races;
        private IRepository<ICar> cars;

        public ChampionshipController()
        {
            cars = new CarRepository();
            races = new RaceRepository();
            drivers = new DriverRepository();
        }

        public string AddCarToDriver(string driverName, string carModel)
        {
            IDriver driver;
            ICar car;
            if (!drivers.GetAll().Any(x => x.Name == driverName))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.DriverNotFound, driverName));
            }
            if (!cars.GetAll().Any(x => x.Model == carModel))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.CarNotFound, carModel));
            }
            driver = drivers.GetAll().FirstOrDefault(x => x.Name == driverName);
            car = cars.GetAll().FirstOrDefault(x => x.Model == carModel);
            driver.AddCar(car);
            return string.Format(OutputMessages.CarAdded, driverName, carModel);
        }

        public string AddDriverToRace(string raceName, string driverName)
        {
            IDriver driver;
            IRace race;
            if (!races.GetAll().Any(x => x.Name == raceName))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.RaceNotFound, raceName));
            }
            if (!drivers.GetAll().Any(x => x.Name == driverName))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.DriverNotFound, driverName));
            }
            race = races.GetAll().FirstOrDefault(x=> x.Name == raceName);
            driver = drivers.GetAll().FirstOrDefault(x => x.Name == driverName);
            race.AddDriver(driver);
            return string.Format(OutputMessages.DriverAdded, driverName, raceName);
        }

        public string CreateCar(string type, string model, int horsePower)
        {
            ICar car;           
            if (cars.GetAll().Any(x => x.Model == model))
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CarExists, model));
            }
            if (type == "Muscle")
            {
                car = new MuscleCar(model, horsePower);
            }
            else
            {
                car = new SportsCar(model, horsePower);
            }
            cars.Add(car);
            return string.Format(OutputMessages.CarCreated, car.GetType().Name, model);
        }

        public string CreateDriver(string driverName)
        {
            IDriver driver;
            if (drivers.GetAll().Any(x => x.Name == driverName))
            {
                throw new ArgumentException(string.Format(ExceptionMessages.DriversExists, driverName));
            }
            driver = new Driver(driverName);
            drivers.Add(driver);
            return string.Format(OutputMessages.DriverCreated, driverName);
        }

        public string CreateRace(string name, int laps)
        {
            IRace race;
            if (races.GetAll().Any(x => x.Name == name))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.RaceExists, name));
            }
            race = new Race(name, laps);
            races.Add(race);
            return string.Format(OutputMessages.RaceCreated, name);
        }

        public string StartRace(string raceName)
        {
            IRace race;
            if (!races.GetAll().Any(x => x.Name == raceName))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.RaceNotFound, raceName));
            }
            race = races.GetAll().FirstOrDefault(x => x.Name == raceName);
            if (race.Drivers.Count < 3)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.RaceInvalid, raceName, 3));
            }

            List<IDriver> topDrivers = new List<IDriver>();
            foreach (var driver in race.Drivers.OrderByDescending(x => x.Car.CalculateRacePoints(race.Laps)))
            {
                topDrivers.Add(driver);
            }

            topDrivers.Take(3);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < topDrivers.Count; i++)
            {
                if (i == 0)
                {
                    stringBuilder.AppendLine($"Driver {topDrivers[0].Name} wins {raceName} race.");
                }
                else if (i == 1)
                {
                    stringBuilder.AppendLine($"Driver {topDrivers[1].Name} is second in {raceName} race.");
                }
                else
                {
                    stringBuilder.AppendLine($"Driver {topDrivers[2].Name} is third in {raceName} race.");
                }
            }
          
            return stringBuilder.ToString().TrimEnd();
        }
    }
}
