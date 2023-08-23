namespace SmartDevice.Tests
{
    using NUnit.Framework;
    using System;
    using System.Text;

    public class Tests
    {
        private Device device;

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Device_Constructor_ShouldInitializePropertiesCorrectly()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);

            Assert.AreEqual(memoryCapacity, device.MemoryCapacity);
            Assert.AreEqual(memoryCapacity, device.AvailableMemory);
            Assert.AreEqual(0, device.Photos);
            Assert.AreEqual(0, device.Applications.Count);
        }

        [Test]
        public void Device_TakePhoto_ShouldReduceAvailableMemoryAndIncrementPhotos()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);
            int photoSize = 100;

            bool photoTaken = device.TakePhoto(photoSize);

            Assert.IsTrue(photoTaken);
            Assert.AreEqual(memoryCapacity - photoSize, device.AvailableMemory);
            Assert.AreEqual(1, device.Photos);
        }

        [Test]
        public void Device_TakePhoto_ShouldreturnFalseIfNotEnoughMemory()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);
            int photoSize = 3000;

            bool photoTaken = device.TakePhoto(photoSize);

            Assert.IsFalse(photoTaken);
            Assert.AreEqual(memoryCapacity, device.AvailableMemory);
            Assert.AreEqual(0, device.Photos);
        }

        [Test]
        public void Device_InstallApp_ShouldReduceAvailableMemoryAndAddAppToList()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);
            int appSize = 500;
            string appName = "MyApp";

            string result = device.InstallApp(appName, appSize);

            Assert.AreEqual($"{appName} is installed successfully. Run application?", result);
            Assert.AreEqual(memoryCapacity - appSize, device.AvailableMemory);
            Assert.AreEqual(1, device.Applications.Count);
            Assert.IsTrue(device.Applications.Contains(appName));
        }

        [Test]
        public void Device_InstallApp_ShouldThrowExceptionIfNotEnoughMemory()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);
            int appSize = 3000;
            string appName = "MyApp";

            Assert.Throws<InvalidOperationException>(() => device.InstallApp(appName, appSize));
            Assert.AreEqual(memoryCapacity, device.AvailableMemory);
            Assert.AreEqual(0, device.Applications.Count);
        }

        [Test]
        public void Device_FormatDevice_ShouldResetProperties()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);
            int photoSize = 100;
            device.TakePhoto(photoSize);
            device.InstallApp("MyApp", 500);

            device.FormatDevice();

            Assert.AreEqual(memoryCapacity, device.AvailableMemory);
            Assert.AreEqual(0, device.Photos);
            Assert.AreEqual(0, device.Applications.Count);
        }

        [Test]
        public void Device_GetDeviceStatus_ShouldReturnStatusString()
        {
            int memoryCapacity = 2048;
            Device device = new Device(memoryCapacity);
            int photoSize = 100;
            device.TakePhoto(photoSize);
            device.InstallApp("MyFirstApp", 500);
            device.InstallApp("MySecondApp", 300);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Memory Capacity: {memoryCapacity} MB, Available Memory: {memoryCapacity - photoSize - 500 - 300} MB");
            stringBuilder.AppendLine($"Photos Count: 1");
            stringBuilder.AppendLine($"Applications Installed: MyFirstApp, MySecondApp");

            string result = stringBuilder.ToString().TrimEnd();
            string status = device.GetDeviceStatus();

            Assert.AreEqual(result, status);
        }

       
    }
}