DROP DATABASE veterinary;
CREATE DATABASE veterinary;

CREATE TABLE Practices (
  PracticeRegNum int NOT NULL,
  Name varchar(255) NOT NULL,
  Address varchar(255) NOT NULL,
  PRIMARY KEY (PracticeRegNum)
);

--One practice to many vets
CREATE TABLE Vets (
  VetID int NOT NULL,
  Name varchar(255) NOT NULL,
  PRIMARY KEY (VetID),
  FOREIGN KEY (PracticeRegNum) REFERENCES Practices(PracticeRegNum)
);

--One practice to many owners
CREATE TABLE Owners (
  OwnerID int NOT NULL,
  Name varchar(255) NOT NULL,
  PhoneNo varchar(255) NOT NULL,
  Email varchar(255) NOT NULL,
  PRIMARY KEY (OwnerID),
  FOREIGN KEY (PracticeRegNum) REFERENCES Practices(PracticeRegNum)
);

--One owner to many pets
--One practice to many pets
CREATE TABLE Pets (
  PetID int NOT NULL,
  Name varchar(255) NOT NULL,
  Species varchar(255) NOT NULL,
  Breed varchar(255) NOT NULL,
  PRIMARY KEY (PetID),
  FOREIGN KEY (OwnerID) REFERENCES Owners(OwnerID),
  FOREIGN KEY (PracticeRegNum) REFERENCES Practices(PracticeRegNum)
);

--One pet to many visits
CREATE TABLE Visits (
  VisitID int NOT NULL,
  VisitDate date NOT NULL,
  Cost int NOT NULL,
  ExaminationResultSummary varchar(2000) DEFAULT NULL,
  MedicationsPrescribed varchar(2000) DEFAULT NULL,
  FOREIGN KEY (PetID) REFERENCES Pets(PetID),
  PRIMARY KEY (VisitID)
);
