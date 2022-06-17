using NUnit.Framework;
using System;
using System.Linq;

namespace Collections.UnitTests
{
    public class CollectionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_CollectionEmptyConstructor()
        {
            //Arrange
            var nums = new Collection<int>();

            //Act
            Assert.That(nums.Count == 0, "Count Property");
            //Assert.That(nums.Capacity == 0, "Capacity Property");
            Assert.AreEqual(nums.Capacity, 16, "Capacity Property");
            Assert.That(nums.ToString() == "[]");

            //Assert
        }

        [Test]
        public void Test_Collection_ConstructorSingleItem()
        {
            //Arrange
            var nums = new Collection<int>(5);

            //Act
            Assert.That(nums.Count == 1, "Count Property");
            Assert.AreEqual(nums.Capacity, 16, "Capacity Property");
            Assert.That(nums.ToString() == "[5]");
        }

        [Test]
        public void Test_Collection_ConstructorMultipleItem()
        {
            //Arrange
            var nums = new Collection<int>(5, 6);

            //Act
            Assert.That(nums.Count == 2, "Count Property");
            Assert.AreEqual(nums.Capacity, 16, "Capacity Property");
            Assert.That(nums.ToString() == "[5, 6]");
        }

        [Test]
        public void Test_Collection_Add()
        {
            //Arrange
            var nums = new Collection<int>();

            //Act
            nums.Add(7);

            //Assert
            Assert.That(nums.Count == 1, "Count Property");
            Assert.AreEqual(nums.Capacity, 16, "Capacity Property");
            Assert.That(nums.ToString() == "[7]");
        }

        [Test]
        public void Test_Collection_ConstructorMultipleItemString()
        {
            //Arrange
            var nums = new Collection<string>("QA");

            //Assert
            Assert.That(nums.Count == 1, "Count Property");
            Assert.AreEqual(nums.Capacity, 16, "Capacity Property");
            Assert.That(nums.ToString() == "[QA]");
        }

        [Test]
        public void Test_Collection_AddRange()
        {
            //Arrange
            var items = new[] { 6, 7, 8 };
            var nums = new Collection<int>();

            //Act
            nums.AddRange(items);

            //Assert
            Assert.That(nums.Count == 3, "Count Property");
            Assert.AreEqual(nums.Capacity, 16, "Capacity Property");
            Assert.That(nums.ToString() == "[6, 7, 8]");
        }

        [Test]
        [Timeout(5000)]
        public void Test_Collection_1MillionItems()
        {
            const int itemsCount = 1000000;
            var nums = new Collection<int>();

            nums.AddRange(Enumerable.Range(1, itemsCount).ToArray());
            Assert.That(nums.Count == itemsCount);
            Assert.That(nums.Capacity >= nums.Count);
            for (int i = itemsCount - 1; i >= 0; i--)
                nums.RemoveAt(i);
            Assert.That(nums.ToString() == "[]");
            Assert.That(nums.Capacity >= nums.Count);
        }

        [Test]
        public void Test_Collection_AddRangeWithGrow()
        {
            var nums = new Collection<int>();
            int oldCapacity = nums.Capacity;
            var newNums = Enumerable.Range(1000, 2000).ToArray();
            nums.AddRange(newNums);
            string expectedNums = "[" + string.Join(", ", newNums) + "]";
            Assert.That(nums.ToString(), Is.EqualTo(expectedNums));
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(oldCapacity));
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(nums.Count));
        }

        [Test]
        public void Test_Collection_GetByIndex()
        {
            //Arrange
            var names = new Collection<string>("Peter", "Maria");

            //Act
            var item0 = names[0];
            var item1 = names[1];

            //Assert
            Assert.That(item0, Is.EqualTo("Peter"));
            Assert.That(item1, Is.EqualTo("Maria"));
        }

        [Test]
        public void Test_Collection_SetByIndex()
        {
            var names = new Collection<string>("Peter", "Maria");
            names[0] = "Steve";
            names[1] = "Mike";
            Assert.That(names.ToString(), Is.EqualTo("[Steve, Mike]"));
        }

        [Test]
        public void Test_Collection_GetByInvalidIndex()
        {
            var names = new Collection<string>("Bob", "Joe");
            Assert.That(() => { var name = names[-1]; },
              Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => { var name = names[2]; },
              Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => { var name = names[500]; },
              Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(names.ToString(), Is.EqualTo("[Bob, Joe]"));

        }

        [Test]
        public void Test_Collection_ToStringNestedCollections()
        {
            var names = new Collection<string>("Teddy", "Gerry");
            var nums = new Collection<int>(10, 20);
            var dates = new Collection<DateTime>();
            var nested = new Collection<object>(names, nums, dates);
            string nestedToString = nested.ToString();
            Assert.That(nestedToString,
              Is.EqualTo("[[Teddy, Gerry], [10, 20], []]"));

        }
        [Test]
        public void Test_Collection_Clear()
        {
            var names = new Collection<string>("Peter", "Maria", "Steve", "Mia");
            names.Clear();
            Assert.That(names.Count, Is.EqualTo(0));
            Assert.That(names.ToString(), Is.EqualTo("[]"));
        }

        [Test]
        public void Test_Collection_ExchangeMiddle()
        {
            var names = new Collection<string>("Peter", "Maria", "Steve", "Mia");
            names.Exchange(1, 2);
            Assert.That(names.ToString(), Is.EqualTo("[Peter, Steve, Maria, Mia]"));
        }

        [Test]
        public void Test_Collection_InsertAtStart()
        {
            var names = new Collection<string>("Peter", "Maria");
            names.InsertAt(0, "Steve");
            Assert.That(names.ToString(), Is.EqualTo("[Steve, Peter, Maria]"));
            Assert.That(names.Capacity, Is.GreaterThanOrEqualTo(names.Count));
        }

        [Test]
        public void Test_Collection_RemoveAtEnd()
        {
            var names = new Collection<string>("Peter", "Maria", "Steve", "Mia");
            var removed = names.RemoveAt(3);
            Assert.That(removed, Is.EqualTo("Mia"));
            Assert.That(names.ToString(), Is.EqualTo("[Peter, Maria, Steve]"));
        }

        [Test]
        public void Test_Collection_RemoveAtMiddle()
        {
            var names = new Collection<string>("Peter", "Maria", "Steve", "Mia");
            var removed = names.RemoveAt(1);
            Assert.That(removed, Is.EqualTo("Maria"));
            Assert.That(names.ToString(), Is.EqualTo("[Peter, Steve, Mia]"));
        }

        [Test]
        public void Test_Collection_RemoveAtInvalidIndex()
        {
            var names = new Collection<string>("Peter", "Maria");
            Assert.That(() => names.RemoveAt(-1), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => names.RemoveAt(2), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => names.RemoveAt(500), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(names.ToString(), Is.EqualTo("[Peter, Maria]"));
        }




        /*[TestCase("Peter", 0, "Peter")]
        public void Collection_GetByValidIndex(
            string data, int index, string expectedValue)
        {

        }*/

    }
}