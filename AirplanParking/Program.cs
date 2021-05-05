using System;
using System.Collections.Generic;
using System.Linq;

namespace AirplanParking
{
    class Program
    {
        static void Main(string[] args)
        {
            ParkingAssistant pa = new ParkingAssistant();
            string slotName = pa.Book("E195");

            if (!string.IsNullOrEmpty(slotName))
                Console.WriteLine(slotName);
            else Console.WriteLine("Wrong Plane Type or no space available");

            Console.ReadKey();
        }
    }

    public class ParkingAssistant
    {
        public string Book(string planeToPark)
        {
            ParkingSlot[] parkingSlotTypes = { new Props(), new Jets(), new Jumbo() };
            string slotName = null;

            foreach (var parkingSlot in parkingSlotTypes)
            {
                var slot = parkingSlot.ValidSlotAvailable(planeToPark);
                if (slot != null)
                {
                    parkingSlot.Book(slot);
                    slotName = slot.SlotName;
                    break;
                }
            }

            return slotName;
        }

    }

    public abstract class ParkingSlot
    {
        private string[] _planesAllowed;
        private int _slotsAvailable;
       // private static readonly List<ParkingSlot> parkingSlots = new List<ParkingSlot>();
        public string SlotName { get; set; }
        public DateTime From { get; set; }
        public Guid Id { get; set; }
        public bool IsAvailable { get; set; }

        protected ParkingSlot(string[] planesAllowed, int slotsAvailable, string slotName)
        {
            _planesAllowed = planesAllowed;
            _slotsAvailable = slotsAvailable;
            SlotName = slotName;
        }

        internal virtual ParkingSlot ValidSlotAvailable(string plane)
        {
            var slot = MockData.Slots.Where(slot => slot.SlotName == SlotName && slot.IsAvailable == true).FirstOrDefault();

            if (_planesAllowed.Contains(plane) && slot != null)
                return slot;

            return null;
        }

        internal virtual void Book(ParkingSlot slot)
        {
            slot.From = DateTime.Now;
            slot.IsAvailable = false;
        }

    }

    internal class Jumbo : ParkingSlot
    {

        private static readonly string[] planesAllowed = { "A380", "B747", "A330", "B777", "E195" };
        private const int SLOTSAVAILABLE = 25;
        private const string SLOTNAME = "Jumbo";

        internal Jumbo() : base(planesAllowed, SLOTSAVAILABLE, SLOTNAME)
        {

        }
    }

    internal class Jets : ParkingSlot
    {
        private static readonly string[] planesAllowed = { "A330", "B777", "E195" };
        private const int SLOTSAVAILABLE = 50;
        private const string SLOTNAME = "Jets";

        internal Jets() : base(planesAllowed, SLOTSAVAILABLE, SLOTNAME)
        {

        }
    }

    internal class Props : ParkingSlot
    {
        private static readonly string[] planesAllowed = { "E195" };
        private const int SLOTSAVAILABLE = 25;
        private const string SLOTNAME = "Props";

        internal Props() : base(planesAllowed, SLOTSAVAILABLE, SLOTNAME)
        {

        }
    }

    static class MockData
    {
        public static List<ParkingSlot> Slots { get; set; }

        static MockData()
        {
            Slots = new List<ParkingSlot>();
            for (int i = 0; i < 25; i++)
            {
                Slots.Add(new Props { Id = Guid.NewGuid(), IsAvailable = true, SlotName = "Props" });
            }

            for (int i = 0; i < 25; i++)
            {
                Slots.Add(new Jumbo { Id = Guid.NewGuid(), IsAvailable = true, SlotName = "Jumbo" });
            }

            for (int i = 0; i < 50; i++)
            {
                Slots.Add(new Props { Id = Guid.NewGuid(), IsAvailable = true, SlotName = "Jets" });
            }
        }
    }
}
