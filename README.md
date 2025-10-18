# PROG6212_POE

---

## 📃Documentation:

### Introduction
The Contract Monthly Claim System (CMCS) is being developed to streamline the monthly process of submitting, reviewing, and approving claims for Independent Contractor (IC) lecturers. The system's primary goal is to reduce administrative overhead, minimize errors, and ensure the timely processing of claims.

With CMCS, lecturers will submit claims electronically, attach relevant documents, and monitor their approval status in real time. Programme Coordinators will be able to review claims quickly, and Academic Managers can approve them directly within the system. This creates a smooth, transparent workflow for all users while improving accountability and data accuracy.

Part 1 of this project focuses on planning and building a prototype, including the database UML class diagram, a project plan, and a visual user interface design without backend functionality.

### Design choices and GUI layout
Satzinger, Jackson and Burd (2016) 

#### Colour Scheme
The interface uses a clean blue to white gradient that gives the system a professional and calm look without overwhelming the user. Blue is the main colour because it represents trust, calmness, and reliability which are all essential qualities. The neutral greys and whites are applied to tables and cards to keep data easy to read and uncluttered. Status indicators use clear colours green is for approved claims, yellow for pending, and red for rejected claims. This colour coding helps users quickly understand claim statuses at a single glance.

#### Typography
The system uses a modern sans-serif font for readability.
- Headings (h1,h2) are bold and larger to create heirarchy so key areas like Lecturer Dashboard or Manage Claims can stand out.
- Body text is slightly muted, ensuring buttons and headings catch the user's eye first.
  
#### Buttons
Buttons follow a rounded-pill style for a polished, consistent look.
- Dark buttons are used for primary actions like New Claim to grab attention.
- Contextual colours guide decision making:
   - Green is for approve or confirm actions
   - Red for reject or delete actions
This colour mapping makes actions clear and intuitive.

#### Interactions
Buttons and table rows have hover effects to guide the user’s focus.
- Statuses use badges (e.g., badge bg-warning, badge bg-success) so they are easy to scan.
- Coordinators and Managers can take actions (Approve/Reject/Edit) directly from the claims table, reducing unnecessary navigation.
- Users can upload files (drag & drop) and add notes directly from the dashboard, making the process smooth.

#### Navigation
A top navigation bar gives users quick access to Home, Login, Lecturer, Coordinator, and Manager sections.
Navigation adapts to the logged-in user, so lecturers see their dashboard, coordinators see “Manage Claims,” and managers see “Approve Claims.”

#### Consistency
Consistency is applied to:
- Layout: All dashboards have a main heading, claims table, and secondary panels (notes, files, support).
- Tables: The order of columns (Month → Hours → Amount → Status → Action) stays the same across roles.
- Colours: Buttons and status badges use the same colour rules everywhere.
This makes the system predictable, lowering the learning curve for users.

#### User Experience
The goal is clarity and efficiency:
-	Claims are listed immediately in a simple table for quick review.
-	Statuses are colour-coded for instant recognition.
-	Extra tools (notes, messages, uploads) are grouped neatly below the table, keeping the main area focused on claims.
-	Lecturers can clearly track claim history, while coordinators/managers can approve or reject with minimal clicks.
 
#### Lecturer View
Can create a claim with one click via the New Claim button.
- See claim history in a table showing Month, Hours, Amount, Status, and Actions.
-	Upload supporting documents, leave notes, and message coordinators directly.
-	Status badges (Pending/Approved) show the current claim state.
 
#### Programme Coordinator View
Dashboard titled “Manage Claims.”
- Can filter/search claims for quick access.
-	Claims table shows lecturer name, hours, amount, and status.
-	Can edit, view details, or delete claims before approval.
-	Designed for handling multiple claims efficiently with minimal scrolling.
 
#### Academic Manager View

Dashboard titled “Approve Claims.”
- Displays claims table with Approve/Reject buttons right in each row.
-	Approved claims display a disabled button (Already Approved) to avoid duplicate approvals.
-	Interface focuses on fast decision-making with minimal distractions.
 
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
Draw.io (2025)

<img width="672" height="614" alt="Image" src="https://github.com/user-attachments/assets/e7185ccb-244d-4840-bf37-ee6a25585c6e" />

### UML Class Diagram explanation

The UML class diagram was designed to capture the core entities, attributes, and relationships required for the Contract Monthly Claim System (CMCS). 

#### 1.	User Class
- Reasoning: The system needs different types of users (Lecturers, Programme Coordinators, Academic Managers). Instead of hardcoding roles, a role attribute allows flexibility in defining permissions.

#### 2.	Lecturer Class
- Reasoning: Lecturers are the primary claim submitters. A separate Lecturer class was created because their personal and payment-related information is distinct from generic user login credentials.

#### 3.	Claim Class
- Reasoning: The claim is the central entity of the system. Every lecturer submits monthly claims that must be stored, calculated, and tracked.

#### 4.	SupportingDocument Class
- Reasoning: Claims must often be accompanied by evidence (timesheets, contracts, proof of hours worked).

#### 5.	Approval Class
- Reasoning: Every claim goes through an approval process by Programme Coordinators or Academic Managers. This workflow needs to be explicitly modeled.

#### 	Relationships
Satzinger, et al (2016) 

- User → SupportingDocument (1..*): A user can upload many documents, but each document belongs to one user.
- Lecturer → Claim (1..*): A lecturer can submit multiple claims, but each claim belongs to one lecturer.
- Claim → SupportingDocument (1..*): A claim may require multiple supporting documents.
- Claim → Approval (1..1): Each claim has exactly one approval outcome (either approved, rejected, or pending).
  

---

## 📖Project Plan:
(Gido, Clements, Baker, Harinarain and Eresia-Eke. 2022)

<img width="796" height="662" alt="Image" src="https://github.com/user-attachments/assets/f2970761-23f9-4840-8044-7feffe54bf7e" />

### Project Plan explanation
This project plan outlines three key phases of the development of the Contract Monthly Claim System. It includes tasks, start dates, end dates, work hours, and progress tracking to ensure that all deadlines are met.
Part 1 focuses on design, where you create wireframes, UML diagrams, and database design so that when Part 2 comes along, implementation will be smooth.
Part 2 brings the system to life. All main business logic and workflows will be implemented.
Part 3 ensures the system is ready for deployment with thorough testing, bug fixes, and a final report.

---

## ✨GUI:

### Home screen:

<img width="1912" height="877" alt="Image" src="https://github.com/user-attachments/assets/89dc9d52-11cc-4d26-9608-efa9c91c91cd" />

### Login screen:

<img width="1816" height="887" alt="Image" src="https://github.com/user-attachments/assets/882ca836-1b4d-4844-8761-f2985b2c1373" />

### Lecturer screen:

<img width="1907" height="901" alt="Image" src="https://github.com/user-attachments/assets/0af8d420-fa54-4dda-bd34-49e36bf9f4fb" />

<img width="1851" height="855" alt="Image" src="https://github.com/user-attachments/assets/e8d7e420-1b0c-4972-a59b-c4029ccffd9f" />

### Coordinator screen:

<img width="1893" height="908" alt="Image" src="https://github.com/user-attachments/assets/2b929c1f-0a26-4807-9b5c-f366688764ce" />

### Manager screen:

<img width="1908" height="887" alt="Image" src="https://github.com/user-attachments/assets/0bcd03a1-125a-4f99-be0d-bb81db07c64a" />

---

## 📚References:
Draw.io. 2025. UML Class Diagram. [online]. ,Available at: <https://app.diagrams.net/>. [Accessed 12 September 2025].

Gido, J., Clements, JP., Baker, R., Harinarain, N., and Eresia-Eke, C. 2022. Successful Project Management in 
South Africa. 2nd edition. Cengage Learning. 

Indeed. 2025. Assumption vs. Constraint: Definitions, Types and Differences. [online]. Available at:<https://www.indeed.com/career-advice/career-development/assumption-vs-constraint>. [Accessed 12 September 2025].

Satzinger, J.W. Jackson, J.B. and Burd, S.D.,2016. Systems Analysis and Design in a Changing 
World. 7th edition. Boston: Cengage. 
