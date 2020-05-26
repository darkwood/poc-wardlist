using System;
using System.Collections.ObjectModel;
using Bogus;

namespace pocwardlist
{
    public static class PatientListService
    {
        public static ObservableCollection<ColumnGroup> CreateColumns()
        {
            var groups = new ObservableCollection<ColumnGroup>();
            groups.Add(new ColumnGroup("TopLeft", "TL"));
            groups.Add(new ColumnGroup("TopRight", "TR"));
            groups.Add(new ColumnGroup("Left", "M"));
            groups.Add(new ColumnGroup("Right", "S"));
            foreach (var group in groups)
            {
                for (var i = 0; i < 5; i++)
                {
                    group.Add(new Column
                    {
                        Header = "Header " + i,
                        Value = "Value " + i,
                        Group = group.Title
                    });
                }
            }

            return groups;
        }

        public static ObservableCollection<Patient> GetPatients()
        {
            var patientSetup = new Faker<Patient>()

                .RuleFor(u => u.Gender, f => f.PickRandomParam("Male", "Female"))
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName((Bogus.DataSets.Name.Gender)Enum.Parse(typeof(Bogus.DataSets.Name.Gender), u.Gender)))
                .RuleFor(u => u.SSN, (f, u) => f.Random.Replace("######-####"))
                .RuleFor(u => u.Age, (f, u) => f.Random.Number(100))
                ;

            var fakePatients = patientSetup.Generate(10);
            var patients = new ObservableCollection<Patient>(fakePatients);
            return patients;
        }
    }
}
