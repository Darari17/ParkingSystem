using System;
using System.Collections.Generic;
using System.Linq;

class ParkingSystem
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Parking System");
        Dictionary<int, Vehicle> parkingLot = new Dictionary<int, Vehicle>();
        int totalSlots = 0;

        while (true)
        {
            Console.Write("$ ");
            var input = Console.ReadLine();
            var command = input.Split(' ');

            switch (command[0].ToLower())
            {
                case "create_parking_lot":
                    totalSlots = int.Parse(command[1]);
                    for (int i = 1; i <= totalSlots; i++)
                        parkingLot[i] = null;

                    Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                    break;

                case "park":
                    ParkVehicle(parkingLot, command.Skip(1).ToArray());
                    break;

                case "leave":
                    LeaveSlot(parkingLot, int.Parse(command[1]));
                    break;

                case "status":
                    ShowStatus(parkingLot);
                    break;

                case "type_of_vehicles":
                    CountVehicleType(parkingLot, command[1]);
                    break;

                case "registration_numbers_for_vehicles_with_ood_plate":
                    GetVehiclesByPlate(parkingLot, true);
                    break;

                case "registration_numbers_for_vehicles_with_event_plate":
                    GetVehiclesByPlate(parkingLot, false);
                    break;

                case "registration_numbers_for_vehicles_with_colour":
                    GetRegistrationNumbersByColour(parkingLot, command[1]);
                    break;

                case "slot_numbers_for_vehicles_with_colour":
                    GetSlotNumbersByColour(parkingLot, command[1]);
                    break;

                case "slot_number_for_registration_number":
                    GetSlotByRegistrationNumber(parkingLot, command[1]);
                    break;

                case "exit":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }

    static void ParkVehicle(Dictionary<int, Vehicle> parkingLot, string[] details)
    {
        var registrationNumber = details[0];
        var color = details[1];
        var type = details[2].ToLower();

        if (type != "mobil" && type != "motor")
        {
            Console.WriteLine("Only Mobil and Motor are allowed.");
            return;
        }

        var availableSlot = parkingLot.FirstOrDefault(s => s.Value == null).Key;

        if (availableSlot == 0)
        {
            Console.WriteLine("Sorry, parking lot is full");
        }
        else
        {
            parkingLot[availableSlot] = new Vehicle
            {
                RegistrationNumber = registrationNumber,
                Color = color,
                Type = type
            };
            Console.WriteLine($"Allocated slot number: {availableSlot}");
        }
    }

    static void LeaveSlot(Dictionary<int, Vehicle> parkingLot, int slotNumber)
    {
        if (parkingLot.ContainsKey(slotNumber) && parkingLot[slotNumber] != null)
        {
            parkingLot[slotNumber] = null;
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
        else
        {
            Console.WriteLine("Slot is already empty or does not exist.");
        }
    }

    static void ShowStatus(Dictionary<int, Vehicle> parkingLot)
    {
        Console.WriteLine("Slot\tNo.\t\tType\tRegistration No\tColour");

        foreach (var slot in parkingLot)
        {
            if (slot.Value != null)
            {
                Console.WriteLine($"{slot.Key}\t{slot.Value.RegistrationNumber}\t{slot.Value.Type}\t{slot.Value.Color}");
            }
        }
    }

    static void CountVehicleType(Dictionary<int, Vehicle> parkingLot, string type)
    {
        var count = parkingLot.Values.Count(v => v != null && v.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(count);
    }

    static void GetVehiclesByPlate(Dictionary<int, Vehicle> parkingLot, bool isOdd)
    {
        var vehicles = parkingLot.Values
            .Where(v => v != null)
            .Where(v => IsPlateOdd(v.RegistrationNumber) == isOdd)
            .Select(v => v.RegistrationNumber);

        Console.WriteLine(string.Join(", ", vehicles));
    }

    static void GetRegistrationNumbersByColour(Dictionary<int, Vehicle> parkingLot, string color)
    {
        var vehicles = parkingLot.Values
            .Where(v => v != null && v.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(v => v.RegistrationNumber);

        Console.WriteLine(string.Join(", ", vehicles));
    }

    static void GetSlotNumbersByColour(Dictionary<int, Vehicle> parkingLot, string color)
    {
        var slots = parkingLot
            .Where(s => s.Value != null && s.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Key);

        Console.WriteLine(string.Join(", ", slots));
    }

    static void GetSlotByRegistrationNumber(Dictionary<int, Vehicle> parkingLot, string registrationNumber)
    {
        var slot = parkingLot
            .FirstOrDefault(s => s.Value != null && s.Value.RegistrationNumber == registrationNumber)
            .Key;

        Console.WriteLine(slot == 0 ? "Not found" : slot.ToString());
    }

    static bool IsPlateOdd(string plate)
    {
        var numbers = new string(plate.Where(char.IsDigit).ToArray());
        return int.Parse(numbers) % 2 != 0;
    }

    class Vehicle
    {
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
    }
}
