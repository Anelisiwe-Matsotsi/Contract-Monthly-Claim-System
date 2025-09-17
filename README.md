# PROG6212_POE

---
---

## 📃Documentation:

### Introduction
The Contract Monthly Claim System (CMCS) is being developed to streamline the monthly process of submitting, reviewing, and approving claims for Independent Contractor (IC) lecturers. The system's primary goal is to reduce administrative overhead, minimize errors, and ensure the timely processing of claims.

With CMCS, lecturers will submit claims electronically, attach relevant documents, and monitor their approval status in real time. Programme Coordinators will be able to review claims quickly, and Academic Managers can approve them directly within the system. This creates a smooth, transparent workflow for all users while improving accountability and data accuracy.

Part 1 of this project focuses on planning and building a prototype, including the database UML class diagram, a project plan, and a visual user interface design without backend functionality.

### Design choices and GUI layout
#### Colour Scheme
#### Typography
#### Buttons
#### Interactions
#### Navigation
#### Consistency
#### User Experience

#### Lecturer View
#### Programme Coordinator View
#### Academic Manager View

### Database structure

#### 1.	Users Table
- Attributes: userId, username, password, role (Lecturer, Coordinator, Manager)
- Purpose: stores login credentials and role

#### 2.	Claims Table
- Attributes: claimId, lecturerId, month, totalHours, hourlyRate, totalAmount, submissionDate, status
- Purpose: records all lecturer claim submissions with their details and status

#### 3.	Approvals Table
- Attributes: approvalId, claimId
- Purpose: tracks the approval or rejection actions taken by coordinators and managers

#### 4.	Supporting Document Table
- Attributes: documentId, claimId, filename, date
- Purpose: stores links to uploaded files (evidence or supporting documents) for each claim

#### 5.	Lecturer Table
- Attributes: lecturerId, name, email, phone
- Purpose: stores personal and contact details for all lecturers

### Assumptions and Constraints
#### Assumptions
According to Indeed (2025), an assumption is something believed to be true based on prior experience or available data, and these assumptions often shape how a project is planned and executed.

The assumptions for this project are:
 - User Accounts Exist: All lecturers, programme coordinators, and academic managers already have valid accounts and login credentials.
 - Submission period: All claims must be submitted during the official monthly submission window set by the institution.
 - Predefined hourly rates: Hourly rates are predetermined and stored in the database.
 - Regular system usage: Coordinators and Academic Managers will consistently use the system to review and approve claims.

#### Constraints
Indeed (2025) states that a constraint is any factor that limits or restricts the project's execution.

The constraints for this project are as follows:
- Technology requirement: The project must be developed using C# and .NET Core.
- Limitation: Part 1 is limited to a front-end prototype, with no back-end functionality implemented at this stage.
- Version control: Using GitHub for version control is required with a minimum of five commits containing clear, descriptive messages.
- Report length: The report must be between 400 and 1000 words in length.
  
---

## 🖊️UML Class Diagram:
<img width="671" height="588" alt="Image" src="https://github.com/user-attachments/assets/30bf35e2-159e-4c23-ab85-1e39c300ad69" />
explain why uml is the way it is...

---

## 📖Project Plan:
<img width="796" height="662" alt="Image" src="https://github.com/user-attachments/assets/f2970761-23f9-4840-8044-7feffe54bf7e" />


---

## ✨GUI:

---

## 📚References:
Indeed. 2025. Assumption vs. Constraint: Definitions, Types and Differences. [online]. Available at: https://www.indeed.com/career-advice/career-development/assumption-vs-constraint. Accessed 12 September 2025.
