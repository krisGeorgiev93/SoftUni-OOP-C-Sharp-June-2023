using NUnit.Framework;
using System;

namespace SmartphoneShop.Tests
{
    [TestFixture]
    public class SmartphoneShopTests
    {
        [Test]
        public void ShopCapacity_IsSetWithCorrectData()
        {
            var shop = new Shop(1);
            Assert.That(shop.Capacity, Is.EqualTo(1));
        }

        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void ShopCapacity_ThrowExceptionWhenSetToLessThanZero(int capacity)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var shop = new Shop(capacity);
            },
            "Invalid capacity.");
        }

        [Test]
        public void PhonesCountIsSetCorrect()
        {
            var shop = new Shop(3);
            Assert.That(shop.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddPhone_ThrowExceptionWhenTheCarAlreadyExist()
        {
            var shop = new Shop(3);
            var phone = new Smartphone("Samsung", 100);
            shop.Add(phone);
            var phone2 = new Smartphone("Samsung", 100);
            Assert.Throws<InvalidOperationException>(() =>
            {
                shop.Add(phone2);
            }, $"The phone model {phone2.ModelName} already exist.");
        }

        [Test]
        public void AddPhone_ThrowExceptionWhenTheCapacityIsReached()
        {
            var shop = new Shop(1);
            var phone = new Smartphone("Samsung", 55);
            shop.Add(phone);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var phone2 = new Smartphone("Apple", 100);
                shop.Add(phone2);
            }, "The shop is full.");
        }

        [Test]
        public void AddPhoneCorrect()
        {
            var shop = new Shop(1);
            var phone = new Smartphone("Samsung", 100);
            shop.Add(phone);
            Assert.That(phone.ModelName, Is.EqualTo("Samsung"));
            Assert.That(phone.CurrentBateryCharge, Is.EqualTo(100));
        }

        [TestCase(null)]
        [TestCase("")]
        public void RemovePhoneFromTheShop(string name)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var shop = new Shop(1);
                shop.Remove(name);
            }, $"The phone model {name} doesn't exist."
            );
        }

        [Test]
        public void RemovePhoneCorrect()
        {
            var shop = new Shop(1);
            var phone = new Smartphone("Apple", 100);
            shop.Add(phone);
            shop.Remove("Apple");
            Assert.That(shop.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestPhone_ThrowsExceptionWhenPhoneModelDoesNotExist()
        {
            Shop shop = new Shop(1);

            Assert.Throws<InvalidOperationException>(() =>
            {
                shop.TestPhone(null, 20);
            },
            $"The phone model {null} doesn't exist.");
        }

        [Test]
        public void TestPhone_ThrowsExceptionWhenWhenBatteryChargeIsLowerThanBatteryUsage()
        {
            var shop = new Shop(1);
            var phone = new Smartphone("apple",15);
            shop.Add(phone);
            Assert.Throws<InvalidOperationException>(() =>
            {
                shop.TestPhone("apple", 20);
            }, $"The phone model {phone} is low on batery.");
        }

        [Test]
        public void TestPhone_WorkingProperlyWhenBatterChargeIsHigherThanBatteryUsage()
        {
            Shop shop = new Shop(1);
            Smartphone testPhone = new Smartphone("Nokia", 50);
            shop.Add(testPhone);
            shop.TestPhone("Nokia", 20);

            Assert.That(testPhone.CurrentBateryCharge, Is.EqualTo(30));
        }

        [Test]
        public void ChargePhone_ThrowsExceptionWhenPhoneModelDoesNotExist()
        {
            Shop shop = new Shop(1);

            Assert.Throws<InvalidOperationException>(() =>
            {
                shop.ChargePhone(null);
            },
            $"The phone model {null} doesn't exist.");
        }

        [Test]
        public void ChargePhoneWithCorrectData()
        {
            Shop shop = new Shop(1);
            Smartphone testPhone = new Smartphone("Nokia", 50);
            testPhone.CurrentBateryCharge = 1;
            shop.Add(testPhone);
            shop.ChargePhone(testPhone.ModelName);
            Assert.That(testPhone.CurrentBateryCharge, Is.EqualTo(testPhone.MaximumBatteryCharge));
        }
    }
}