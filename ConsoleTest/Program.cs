using System;
using System.Collections.Generic;
using ConsoleTest.Models;
namespace ConsoleTest
{
    internal class Program
    {/*Troelson and Japiske, 2021*/
            static void Main(string[] args)
            {
                List<Claim> claims = new();
                List<Approval> approvals = new();

                var lecturer = new LecturerController(claims);
                var coordinator = new CoordinatorController(claims, approvals);
                var manager = new ManagerController(claims, approvals);

                Console.WriteLine(lecturer.CreateClaim(1, 10, 200, "October"));

                Console.WriteLine(coordinator.Approve(1));

                Console.WriteLine(manager.Approve(1));

                Console.WriteLine("\nFinal claim status:");
                foreach (var c in claims)
                {
                    Console.WriteLine($"Claim {c.ClaimId}: {c.ClaimStatus}, Total: {c.TotalAmount:C}");
                }

                Console.WriteLine("\nApprovals:");
                foreach (var a in approvals)
                {
                    Console.WriteLine($"Claim {a.ClaimId}: {a.Decision} by {a.Role}");
                }
            }
        }
    }
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */