using OnlineShop.Common.Constants;
using OnlineShop.Models.Products.Computers;
using OnlineShop.Models.Products.Peripherals;
using OnlineShop.Models.Products.Components;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;

namespace OnlineShop.Core
{
    public class Controller : IController
    {
        private ICollection<IComputer> computers;
        private ICollection<IPeripheral> peripherals;
        private ICollection<IComponent> components;

        public Controller()
        {
            this.computers = new List<IComputer>();
            this.peripherals = new List<IPeripheral>();
            this.components = new List<IComponent>();
        }


        public string AddComponent(int computerId, int id, string componentType, string manufacturer, string model, decimal price, double overallPerformance, int generation)
        {
            if (!computers.Any(x=> x.Id == computerId))
            {
                throw new ArgumentException(ExceptionMessages.NotExistingComputerId);
            }
            if (components.Any(x => x.Id == id))
            {
                throw new ArgumentException(ExceptionMessages.ExistingComponentId);
            }
            if (componentType != nameof(CentralProcessingUnit) && componentType != nameof(Motherboard) && componentType != nameof(PowerSupply)
                && componentType != nameof(RandomAccessMemory) && componentType != nameof(SolidStateDrive) && componentType != nameof(VideoCard))
            {
                throw new ArgumentException(ExceptionMessages.InvalidComponentType);
            }
            IComponent component;
            if (componentType == nameof(CentralProcessingUnit))
            {
                component = new CentralProcessingUnit(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(Motherboard))
            {
                component = new Motherboard(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(PowerSupply))
            {
                component = new PowerSupply(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(RandomAccessMemory))
            {
                component = new RandomAccessMemory(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(SolidStateDrive))
            {
                component = new SolidStateDrive(id, manufacturer, model, price, overallPerformance, generation);
            }
            else
            {
                component = new VideoCard(id, manufacturer, model, price, overallPerformance, generation);
            }
            var targetComputer = computers.FirstOrDefault(x => x.Id == computerId);
            targetComputer.AddComponent(component);
            components.Add(component);
            return string.Format(SuccessMessages.AddedComponent, component.GetType().Name, id, computerId);
        }

        public string AddComputer(string computerType, int id, string manufacturer, string model, decimal price)
        {
            
            if (computerType != nameof(Laptop) && computerType != nameof(DesktopComputer))
            {
                throw new ArgumentException(ExceptionMessages.InvalidComputerType);
            }
            IComputer computer;
            if (computerType == nameof(Laptop))
            {
                computer = new Laptop(id, manufacturer, model, price);
            }
            else
            {
                computer = new DesktopComputer(id, manufacturer, model, price);
            }
            computers.Add(computer);
            return string.Format(SuccessMessages.AddedComputer, id);
        }

        public string AddPeripheral(int computerId, int id, string peripheralType, string manufacturer, string model, decimal price, double overallPerformance, string connectionType)
        {
            if (!computers.Any(x => x.Id == computerId))
            {
                throw new ArgumentException(ExceptionMessages.NotExistingComputerId);
            }
            if (peripherals.Any(x=> x.Id == id))
            {
                throw new ArgumentException(ExceptionMessages.ExistingPeripheral);
            }
            if (peripheralType!=nameof(Headset) && peripheralType != nameof(Monitor) && peripheralType != nameof(Keyboard) && peripheralType != nameof(Mouse))
            {
                throw new ArgumentException(ExceptionMessages.InvalidPeripheralType);
            }

            IPeripheral peripheral;
            if (peripheralType == nameof(Headset))
            {
                peripheral = new Headset(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == nameof(Keyboard))
            {
                peripheral = new Keyboard(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == nameof(Mouse))
            {
                peripheral = new Mouse(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else
            {
                peripheral = new Monitor(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            var targetComputer = computers.FirstOrDefault(x => x.Id == computerId);
            targetComputer.AddPeripheral(peripheral);
            peripherals.Add(peripheral);
            return string.Format(SuccessMessages.AddedPeripheral, peripheral.GetType().Name, peripheral.Id,computerId);
        }

        public string BuyBest(decimal budget)
        {
            IComputer computer = computers.OrderByDescending(c => c.OverallPerformance).Where(c => c.Price <= budget).FirstOrDefault();
            if (computer == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CanNotBuyComputer, budget));
            }

            string outPut = computer.ToString();
            computers.Remove(computer);
            return outPut;
        }

        public string BuyComputer(int id)
        {
            if (!computers.Any(x => x.Id == id))
            {
                throw new ArgumentException(ExceptionMessages.NotExistingComputerId);
            }
            var targetComputer = computers.FirstOrDefault(x=> x.Id == id);
            computers.Remove(targetComputer);
            string output = targetComputer.ToString();
            return output;            
        }

        public string GetComputerData(int id)
        {
            if (!computers.Any(x => x.Id == id))
            {
                throw new ArgumentException(ExceptionMessages.NotExistingComputerId);
            }
            var targetComputer = computers.FirstOrDefault(x => x.Id == id);
            string output = targetComputer.ToString();
            return output;
        }

        public string RemoveComponent(string componentType, int computerId)
        {
            if (!computers.Any(x => x.Id == computerId))
            {
                throw new ArgumentException(ExceptionMessages.NotExistingComputerId);
            }
            var targetComputer = computers.FirstOrDefault(x => x.Id == computerId);
            var targetComponent = components.FirstOrDefault(x => x.GetType().Name == componentType);
            targetComputer.RemoveComponent(componentType);
            components.Remove(targetComponent);
            return string.Format(SuccessMessages.RemovedComponent, componentType, targetComponent.Id);
        }

        public string RemovePeripheral(string peripheralType, int computerId)
        {
            if (!computers.Any(x => x.Id == computerId))
            {
                throw new ArgumentException(ExceptionMessages.NotExistingComputerId);
            }
            var targetPeripheral = peripherals.FirstOrDefault(x=> x.GetType().Name == peripheralType);
            var targetComputer = computers.FirstOrDefault(x=> x.Id == computerId);
            targetComputer.RemovePeripheral(peripheralType);
            peripherals.Remove(targetPeripheral);
            return string.Format(SuccessMessages.RemovedPeripheral, peripheralType, targetPeripheral.Id);
        }
    }
}
