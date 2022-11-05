using System.Data.Entity;
using VeterinaryPractice;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new VeterinaryPracticeContext())
        {
            Console.WriteLine("\n-- VETERINARY PRACTICE --");
            Console.WriteLine("1. List the names and contact details of all owners who are customers of the veterinary practice, sorted by surname");
            Console.Write("Choice: ");
            var choiceInput = Console.ReadLine();
            var choice = Convert.ToInt32(choiceInput);
            Console.WriteLine("Choice was: {0}", choice);

            switch (choice)
            {
                case 1:
                    ListOwnerNames(db);
                    return;
                case 2:
                    return;
                default:
                    Console.WriteLine("That choice is not available");
                    break;
            }
        }
    }

    static void ListOwnerNames(VeterinaryPracticeContext db)
    {
        var query =
            from owner in db.Owner
            orderby owner.Name
            select owner;
        Console.WriteLine("Owner names and details:");
        foreach (var owner in query)
        {
            Console.WriteLine(owner.Name);
        }
    }

    static void ListPetsForPractice(VeterinaryPracticeContext db, int practiceId)
    {
        var query =
            from pet in db.Pet
            where pet.Practice.PracticeID == practiceId
            select pet;
        Console.WriteLine("Pets assigned to practice {0}:", practiceId);
        foreach (var pet in query)
        {
            Console.WriteLine(pet.Name);
        }
    }

    static void DisplayPracticeInfo(VeterinaryPracticeContext db, int practiceId)
    {
        var query =
            from practice in db.Practice
            where practice.PracticeID == practiceId
            select practice;
        foreach (var practice in query)
        {
            Console.WriteLine("Practice {0} Info:", practice.Name);
            Console.WriteLine(": {0}", practice.Address);
        }
    }

    static void PetInfo(VeterinaryPracticeContext db, int petId)
    {
        var petName = "";
        var query =
            from pet in db.Pet
            where pet.PetID == petId
            select pet;
        foreach (var pet in query)
        {
            Console.WriteLine("Pet{0} Info and Visits", pet.Name);
            // TODO
            Console.WriteLine("Type: {0}", pet.Species);
            Console.WriteLine("Breed: {0}", pet.Breed);
            petName = pet.Name;
        }

        query =
            from visit in db.Visit
            where visit.PetID == petId
            orderby visit.Date
            select visit;
        foreach (var visit in query)
        {
            Console.WriteLine("Pet {0} attended visit ID {1} on date {2}", petName, visit.VisitID, visit.Date);
        }
    }

    static void VisitsToVetOnDate(VeterinaryPracticeContext db, int vetId, string date)
    {
        var query =
            from vet in db.Vet
            where vet.VetId == vetId && vet.Date == date
            select vet;

        var vetQuery =
            from vet in db.Vet
            where vet.VetId == vetId
            select vet;
        foreach (var vet in vetQuery)
        {
            Console.WriteLine("Visits that Vet {0} had on date {1}", vet.Name, date);
            var visitQuery =
                from visit in db.Visit
                where visit.VetID == vetId
                select visit;
            foreach (var visit in visitQuery)
            {
                var petQuery =
                    from pet in db.Pet
                    where pet.PetID == visit.PetID
                    select pet;
                foreach (var pet in petQuery)
                {
                    var ownerQuery =
                        from owner in db.Owner
                        where owner.OwnerID == pet.OwnerID
                        select owner;
                    foreach (var owner in ownerQuery)
                    {
                        Console.WriteLine("Pet {0} owned by {1} was treated by vet {2}", pet.Name, pet.Owner, vet.Name);
                    }
                }
            }

        }
    }

    static void CostOfPetsRecentAppointment(VeterinaryPracticeContext db, int petId)
    {
        var visitQuery =
            from visit in db.Visit
            where visit.PetID == petId
            orderby visit.Date
            select visit;

        var visitsPresent = false;

        var totalCharge = 40;
        foreach (var visit in visitQuery)
        {
            visitsPresent = true;
            foreach (var medication in visit.medication)
            {
                // The cost of each additional medication will be £20
                totalCharge += 20;
            }
            var petQuery =
                from pet in db.Pet
                where pet.PetID == petId
                select pet;
            foreach (var pet in petQuery)
            {
                Console.WriteLine("Pet {0} paid £{1} at their most recent visit on the date {2}", pet.Name, pet.Visit, visit.Date);
            }
            break;
        }
        if (!visitsPresent)
        {
            Console.WriteLine("Pet with ID {0} has had no visits to the practice", petId);
        }
    }
}
