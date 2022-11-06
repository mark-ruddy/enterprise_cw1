using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryPractice
{
    public class Model
    {
        public class Practice
        {
            public int PracticeID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public class Vet
        {
            public int VetID { get; set; }
            public string Name { get; set; }
            public virtual Practice Practice { get; set; }
        }

        public class Owner
        {
            public int OwnerID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNo { get; set; }
            public string Email { get; set; }
            public virtual Practice Practice { get; set; }
        }

        public class Pet
        {
            public int PetID { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Breed { get; set; }
            public virtual Owner Owner { get; set; }
            public virtual Practice Practice { get; set; }
        }

        public class Visit
        {
            public int VisitID { get; set; }
            public string Date { get; set; }
            public string ExaminationResultsSummary { get; set; }
            public string MedicationsPrescribed { get; set; }
            public virtual Pet Pet { get; set; }
            public virtual Vet Vet { get; set; }
        }
    }
}
