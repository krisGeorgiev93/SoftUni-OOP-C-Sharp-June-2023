using Formula1.Core.Contracts;
using Formula1.Models;
using Formula1.Models.Contracts;
using Formula1.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Formula1.Core
{
    public class Controller : IController
    {
        private PilotRepository pilotRepository;
        private RaceRepository raceRepository;
        private FormulaOneCarRepository carsRepository;

        public Controller()
        {
            this.pilotRepository = new PilotRepository();
            this.raceRepository = new RaceRepository();
            this.carsRepository = new FormulaOneCarRepository();
        }
        public string AddCarToPilot(string pilotName, string carModel)
        {
            IFormulaOneCar car = carsRepository.FindByName(carModel);
            IPilot pilot = pilotRepository.FindByName(pilotName);
            if (pilot == null || pilot.Car != null) // If the pilot does not exist, or the pilot already has a car,
            {
                throw new InvalidOperationException($"Pilot {pilotName} does not exist or has a car.");
            }
            if (car == null)
            {
                throw new NullReferenceException($"Car {carModel} does not exist.");
            }
            pilot.AddCar(car);
            carsRepository.Remove(car);
            return $"Pilot {pilot.FullName} will drive a {car.GetType().Name} {car.Model} car.";
        }

        public string AddPilotToRace(string raceName, string pilotFullName)
        {
            IPilot pilot = pilotRepository.FindByName(pilotFullName);
            IRace race = raceRepository.FindByName(raceName);
            if (race == null)
            {
                throw new NullReferenceException($"Race {raceName} does not exist.");
            }
            if (pilot == null || pilot.CanRace == false || race.Pilots.Contains(pilot)) // If the pilot does not exist, or the pilot can not race, or the pilot is already in the race,
            {
                throw new InvalidOperationException($"Can not add pilot {pilotFullName} to the race.");
            }
            race.AddPilot(pilot);
            return $"Pilot {pilot.FullName} is added to the {race.RaceName} race.";
        }

        public string CreateCar(string type, string model, int horsepower, double engineDisplacement)
        {
            IFormulaOneCar car;
            if (carsRepository.FindByName(model) != null)
            {
                throw new InvalidOperationException($"Formula one car {model} is already created.");
            }

            if (type != nameof(Ferrari) && type != nameof(Williams))
            {
                throw new InvalidOperationException($"Formula one car type {type} is not valid.");
            }

            if (type == nameof(Ferrari))
            {
                car = new Ferrari(model, horsepower, engineDisplacement);
            }

            else
            {
                car = new Williams(model, horsepower, engineDisplacement);
            }
            carsRepository.Add(car);
            return $"Car {car.GetType().Name}, model {car.Model} is created.";
        }

        public string CreatePilot(string fullName)
        {
            IPilot pilot;
            if (pilotRepository.FindByName(fullName) != null)
            {
                throw new InvalidOperationException($"Pilot {fullName} is already created.");
            }
            pilot = new Pilot(fullName);
            pilotRepository.Add(pilot);
            return $"Pilot {pilot.FullName} is created.";
        }

        public string CreateRace(string raceName, int numberOfLaps)
        {
            IRace targetRace = raceRepository.FindByName(raceName);

            if (targetRace != null)
            {
                throw new InvalidOperationException($"Race {raceName} is already created.");
            }

            IRace race = new Race(raceName, numberOfLaps);
            this.raceRepository.Add(race);

            return $"Race {race.RaceName} is created.";
        }

        public string PilotReport()
        {
            StringBuilder result = new StringBuilder();

            foreach (IPilot pilot in pilotRepository.Models.OrderByDescending(pilot => pilot.NumberOfWins))
            {
                result.AppendLine($"Pilot {pilot.FullName} has {pilot.NumberOfWins} wins.");
            }

            return result.ToString().TrimEnd();
        }

        public string RaceReport()
        {
            StringBuilder result = new StringBuilder();

            foreach (IRace race in raceRepository.Models.Where(x=> x.TookPlace == true))
            {
                result.AppendLine(race.RaceInfo());  
            }
            return result.ToString().TrimEnd();
        }

        public string StartRace(string raceName)
        {
            if (raceRepository.FindByName(raceName) == null)
            {
                throw new NullReferenceException($"Race {raceName} does not exist.");
            }
            if (raceRepository.FindByName(raceName).Pilots.Count < 3)
            {
                throw new InvalidOperationException($"Race {raceName} cannot start with less than three participants.");
            }
            if (raceRepository.FindByName(raceName).TookPlace == true)
            {
                throw new InvalidOperationException($"Can not execute race {raceName}.");
            }
            IRace race = raceRepository.FindByName(raceName);
            race.TookPlace = true;
            IEnumerable<IPilot> winnersIEnumerable = race.Pilots.OrderByDescending
                (pilot => pilot.Car.RaceScoreCalculator(race.NumberOfLaps)).Take(3);
            winnersIEnumerable.ElementAt(0).WinRace();
            StringBuilder winners = new StringBuilder();

            winners.AppendLine($"Pilot {winnersIEnumerable.ElementAt(0).FullName} wins the {race.RaceName} race.");
            winners.AppendLine($"Pilot {winnersIEnumerable.ElementAt(1).FullName} is second in the {race.RaceName} race.");
            winners.AppendLine($"Pilot {winnersIEnumerable.ElementAt(2).FullName} is third in the {race.RaceName} race.");

            return winners.ToString().TrimEnd();
        }
    }
}
