using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    // Generic repository for entity management
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // Patient class
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
        }
    }

    // Prescription class
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString()
        {
            return $"Prescription ID: {Id}, Medication: {MedicationName}, Date: {DateIssued:dd/MM/yyyy}";
        }
    }

    // HealthSystemApp
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new Repository<Patient>();
        private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

        public void Run()
        {
            Console.Title = "🏥 Healthcare Management System";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Welcome to the Healthcare Management System ===\n");
            Console.ResetColor();

            SeedData();
            BuildPrescriptionMap();

            PrintAllPatients();

            while (true)
            {
                Console.Write("\nEnter Patient ID to view prescriptions (or 0 to exit): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int patientId))
                {
                    if (patientId == 0) break;

                    PrintPrescriptionsForPatient(patientId);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                    Console.ResetColor();
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThank you for using the Healthcare Management System! 👋");
            Console.ResetColor();
        }

        public void SeedData()
        {
            // Adding patients
            _patientRepo.Add(new Patient(1, "Alice Johnson", 30, "Female"));
            _patientRepo.Add(new Patient(2, "Bob Smith", 45, "Male"));
            _patientRepo.Add(new Patient(3, "Clara Brown", 28, "Female"));

            // Adding prescriptions
            _prescriptionRepo.Add(new Prescription(101, 1, "Paracetamol", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(102, 1, "Amoxicillin", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(103, 2, "Ibuprofen", DateTime.Now.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(104, 3, "Vitamin C", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(105, 2, "Lisinopril", DateTime.Now.AddDays(-1)));
        }

        public void BuildPrescriptionMap()
        {
            var prescriptions = _prescriptionRepo.GetAll();
            foreach (var prescription in prescriptions)
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public void PrintAllPatients()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- List of Patients ---");
            Console.ResetColor();

            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine(patient);
            }
        }

        public void PrintPrescriptionsForPatient(int patientId)
        {
            var patient = _patientRepo.GetById(p => p.Id == patientId);
            if (patient == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Patient not found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nPrescriptions for {patient.Name}:");
            Console.ResetColor();

            if (_prescriptionMap.ContainsKey(patientId))
            {
                foreach (var prescription in _prescriptionMap[patientId])
                {
                    Console.WriteLine(prescription);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No prescriptions found for this patient.");
                Console.ResetColor();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new HealthSystemApp();
            app.Run();
        }
    }
}
