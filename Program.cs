using System.Data.Entity;
using VeterinaryPractice;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new VeterinaryPracticeContext())
        {
            while (true)
            {
                Console.WriteLine("\n-- VETERINARY PRACTICE --");
                Console.WriteLine("0. To Exit");
                Console.WriteLine("1. List the names and contact details of all owners for customers of the veterinary practice, sorted by surname");
                Console.WriteLine("2. Pets registered with a practice");
                Console.WriteLine("3. Display practice information and employed vets");
                Console.WriteLine("4. Pets general info and visit list");
                Console.WriteLine("5. Vets pet appointments on a specified date");
                Console.WriteLine("6. Cost of Pets most recent visit");
                Console.Write("Choice: ");
                var choiceInput = Console.ReadLine();
                var choice = Convert.ToInt32(choiceInput);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        ListOwnersForPractice(db, GetPracticeIdInput());
                        break;
                    case 2:
                        ListPetsForPractice(db, GetPracticeIdInput());
                        break;
                    case 3:
                        DisplayPracticeInfoAndVets(db, GetPracticeIdInput());
                        break;
                    case 4:
                        PetInfoAndVisits(db, GetPetIdInput());
                        break;
                    case 5:
                        VisitsToVetOnDate(db, GetVetIdInput(), GetDateInput());
                        break;
                    case 6:
                        CostOfPetsRecentAppointment(db, GetPetIdInput());
                        break;
                    default:
                        Console.WriteLine("That choice is not available");
                        break;
                }
            }
        }
    }

    static string GetDateInput()
    {
        Console.Write("Enter date in format YYYY-MM-DD: ");
        var dateInput = Console.ReadLine();
        if (dateInput != null)
        {
            return dateInput;
        }
        else
        {
            return "2000-01-01";
        }
    }

    static int GetVetIdInput()
    {
        Console.Write("Enter the Vet ID: ");
        var vetIdInput = Console.ReadLine();
        var vetId = Convert.ToInt32(vetIdInput);
        return vetId;
    }

    static int GetPetIdInput()
    {
        Console.Write("Enter the Pet ID: ");
        var petIdInput = Console.ReadLine();
        var petId = Convert.ToInt32(petIdInput);
        return petId;
    }

    static int GetPracticeIdInput()
    {
        Console.Write("Enter the Practice ID: ");
        var practiceIdInput = Console.ReadLine();
        var practiceId = Convert.ToInt32(practiceIdInput);
        return practiceId;
    }

    static void ListOwnersForPractice(VeterinaryPracticeContext db, int practiceId)
    {
        var practice =
            (from practiceItem in db.Practice
             where practiceItem.PracticeID == practiceId
             select practiceItem).FirstOrDefault();
        if (practice != null)
        {
            Console.WriteLine("\nOwner contact details for customers of practice {0}:", practice.Name);
            var ownerQuery =
                from owner in db.Owner
                where owner.Practice.PracticeID == practiceId
                orderby owner.LastName
                select owner;
            foreach (var owner in ownerQuery)
            {
                Console.WriteLine("---- Owner {0} {1} Contact Details ----", owner.FirstName, owner.LastName);
                Console.WriteLine("Email: {0}", owner.Email);
                Console.WriteLine("Phone No: {0}\n", owner.PhoneNo);
            }
        }
        else
        {
            Console.WriteLine("No practice with ID {0} found", practiceId);
        }
    }

    static void ListPetsForPractice(VeterinaryPracticeContext db, int practiceId)
    {
        var practice =
            (from practiceItem in db.Practice
             where practiceItem.PracticeID == practiceId
             select practiceItem).FirstOrDefault();
        if (practice != null)
        {
            Console.WriteLine("\nPets assigned to practice {0}:", practice.Name);
            var petQuery =
                from pet in db.Pet
                where pet.Practice.PracticeID == practiceId
                select pet;
            var petsPresent = false;
            foreach (var pet in petQuery)
            {
                petsPresent = true;
                Console.WriteLine("---- Pet {0} Info ----", pet.Name);
                Console.WriteLine("Type: {0}", pet.Type);
                Console.WriteLine("Breed: {0}\n", pet.Breed);
            }
            if (!petsPresent)
            {
                Console.WriteLine("No pets are assigned to this practice");
            }
        }
        else
        {
            Console.WriteLine("No practice with ID {0} found", practiceId);
        }
    }

    static void DisplayPracticeInfoAndVets(VeterinaryPracticeContext db, int practiceId)
    {
        var practice =
            (from practiceItem in db.Practice
             where practiceItem.PracticeID == practiceId
             select practiceItem).FirstOrDefault();
        if (practice != null)
        {
            Console.WriteLine("\n---- Practice {0} Info and Vets ----", practice.Name);
            Console.WriteLine("Address: {0}", practice.Address);
            var vetQuery =
                from vet in db.Vet
                where vet.Practice.PracticeID == practiceId
                select vet;
            foreach (var vet in vetQuery)
            {
                Console.WriteLine("Vet {0} works at practice {1}", vet.Name, practice.Name);
            }
        }
        else
        {
            Console.WriteLine("No practice with ID {0} found", practiceId);
        }
    }

    static void PetInfoAndVisits(VeterinaryPracticeContext db, int petId)
    {
        var pet =
            (from petItem in db.Pet
             where petItem.PetID == petId
             select petItem).FirstOrDefault();
        if (pet != null)
        {
            Console.WriteLine("\n---- Pet {0} Info and Visits ----", pet.Name);
            Console.WriteLine("Type: {0}", pet.Type);
            Console.WriteLine("Breed: {0}", pet.Breed);
            var visitsPresent = false;
            var visitQuery =
                from visit in db.Visit
                where visit.Pet.PetID == petId
                orderby visit.Date
                select visit;
            foreach (var visit in visitQuery)
            {
                visitsPresent = true;
                Console.WriteLine("Pet {0} attended visit ID {1} on date {2}. The result was: {3}.", pet.Name, visit.VisitID, visit.Date, visit.ExaminationResultsSummary);
            }
            if (!visitsPresent)
            {
                Console.WriteLine("No visits found for pet {0}", pet.Name);
            }
        }
        else
        {
            Console.WriteLine("No pet with ID {0} found", petId);
        }
    }

    static void VisitsToVetOnDate(VeterinaryPracticeContext db, int vetId, string date)
    {
        var vet =
            (from vetItem in db.Vet
             where vetItem.VetID == vetId
             select vetItem).FirstOrDefault();
        if (vet != null)
        {
            Console.WriteLine("\n---- Visits that Vet {0} had on date {1} ----", vet.Name, date);
            var visitQuery =
                from visit in db.Visit
                where visit.Vet.VetID == vetId && visit.Date == date
                select visit;
            foreach (var visit in visitQuery)
            {
                var owner =
                    (from ownerItem in db.Owner
                     where ownerItem.OwnerID == visit.Pet.Owner.OwnerID
                     select ownerItem).FirstOrDefault();
                if (owner != null)
                {
                    Console.WriteLine("Pet {0} owned by {1} {2} was treated", visit.Pet.Name, owner.FirstName, owner.LastName);
                }
                else
                {
                    Console.WriteLine("Pet {0} who has no designated owner was treated", visit.Pet.Name);
                }
            }
        }
    }

    static void CostOfPetsRecentAppointment(VeterinaryPracticeContext db, int petId)
    {
        var totalCharge = 40;
        var recentVisit =
            (from visit in db.Visit
             where visit.Pet.PetID == petId
             orderby visit.Date descending
             select visit).FirstOrDefault();
        if (recentVisit != null)
        {
            var pet =
                (from petItem in db.Pet
                 where petItem.PetID == petId
                 select petItem).FirstOrDefault();
            if (pet != null)
            {
                Console.WriteLine("\n---- Cost of Pet {0}'s Most Recent Visit ----", pet.Name);
                Console.WriteLine("The base payment for a visit is £40");
                var anyMedicationsPrescribed = false;
                var medications = recentVisit.MedicationsPrescribed.Split(',');
                foreach (var medication in medications)
                {
                    Console.WriteLine("Medication {0} was prescribed at an additional cost of £20", medication);
                    totalCharge += 20;
                    anyMedicationsPrescribed = true;
                }
                if (!anyMedicationsPrescribed)
                {
                    Console.WriteLine("No medications were prescribed at this visit");
                }
                Console.WriteLine("The visit for pet {0} cost £{1} on the date {2}", pet.Name, totalCharge, recentVisit.Date);
            }
            else
            {
                Console.WriteLine("No pet was found for visit on date {0}", recentVisit.Date);
            }
        }
        else
        {
            Console.WriteLine("Pet with ID {0} has had no visits to the practice", petId);
        }
    }
}
