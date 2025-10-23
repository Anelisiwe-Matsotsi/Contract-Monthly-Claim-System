using PROG6212_POE.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleTest;
using ConsoleTest.Models;
using System.Collections.Generic;
/*Troelson and Japikse, 2021*/
namespace TestProject
{
    /*Microsoft, 2025a*/
        [TestClass]
        public class ControllerTests
        {
        //Test that creating a claim adds it to the list with correct status
        [TestMethod]
            public void Lecturer_CreateClaim_ShouldAddClaim()
            {
                var claims = new List<Claim>();
                var lecturer = new ConsoleTest.LecturerController(claims);

                string result = lecturer.CreateClaim(1, 10, 200m, "October");

                Assert.AreEqual(1, claims.Count);
                Assert.AreEqual("Pending", claims[0].ClaimStatus);
                Assert.IsTrue(result.Contains("October"));  /*Microsoft, 2025b*/
        }

        //Test that Coordinator approves a claim correctly
            [TestMethod]
            public void Coordinator_Approve_ShouldUpdateClaimStatus()
            {
                var claims = new List<Claim> { new Claim { ClaimId = 1, ClaimStatus = "Pending" } };
                var approvals = new List<Approval>();
                var coordinator = new ConsoleTest.CoordinatorController(claims, approvals);

                string result = coordinator.Approve(1);

                Assert.AreEqual("Approved by Coordinator", claims[0].ClaimStatus);
                Assert.AreEqual(1, approvals.Count);
                Assert.AreEqual("Approved", approvals[0].Decision);
                Assert.AreEqual("Approved successfully", result);  /*Microsoft, 2025b*/
        }
        //Test that Manager rejects a claim correctly
        [TestMethod]
            public void Coordinator_Reject_ShouldUpdateClaimStatus()
            {
                var claims = new List<Claim> { new Claim { ClaimId = 2, ClaimStatus = "Pending" } };
                var approvals = new List<Approval>();
                var coordinator = new ConsoleTest.CoordinatorController(claims, approvals);

                string result = coordinator.Reject(2);

                Assert.AreEqual("Rejected by Coordinator", claims[0].ClaimStatus);
                Assert.AreEqual(1, approvals.Count);
                Assert.AreEqual("Rejected", approvals[0].Decision);
                Assert.AreEqual("Rejected successfully", result);
            }
        //Test that Manager approves a claim correctly
        [TestMethod]
            public void Manager_Approve_ShouldUpdateClaimStatus()
            {
                var claims = new List<Claim> { new Claim { ClaimId = 3, ClaimStatus = "Approved by Coordinator" } };
                var approvals = new List<Approval>();
                var manager = new ConsoleTest.ManagerController(claims, approvals);

                string result = manager.Approve(3);

                Assert.AreEqual("Approved by Manager", claims[0].ClaimStatus);
                Assert.AreEqual(1, approvals.Count);
                Assert.AreEqual("Approved", approvals[0].Decision);
                Assert.AreEqual("Approved successfully", result);
            }
        //Test that Manager rejects a claim correctly
        [TestMethod]
            public void Manager_Reject_ShouldUpdateClaimStatus()
            {
                var claims = new List<Claim> { new Claim { ClaimId = 4, ClaimStatus = "Approved by Coordinator" } };
                var approvals = new List<Approval>();
                var manager = new ConsoleTest.ManagerController(claims, approvals);

                string result = manager.Reject(4);

                Assert.AreEqual("Rejected by Manager", claims[0].ClaimStatus);
                Assert.AreEqual(1, approvals.Count);
                Assert.AreEqual("Rejected", approvals[0].Decision);
                Assert.AreEqual("Rejected successfully", result);
            }

        //Test that Coordinator retrieves only pending claims
        [TestMethod]
            public void Coordinator_GetPendingClaims_ShouldReturnOnlyPending()
            {
                var claims = new List<Claim>
            {
                new Claim { ClaimId = 1, ClaimStatus = "Pending" },
                new Claim { ClaimId = 2, ClaimStatus = "Approved by Coordinator" }
            };
                var approvals = new List<Approval>();
                var coordinator = new ConsoleTest.CoordinatorController(claims, approvals);

                var result = coordinator.GetPendingClaims();

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Pending", result[0].ClaimStatus);
            }
        //Test that Manager retrieves only claims approved by Coordinator
        [TestMethod]
            public void Manager_GetCoordinatorApprovedClaims_ShouldReturnOnlyApprovedByCoordinator()
            {
                var claims = new List<Claim>
            {
                new Claim { ClaimId = 1, ClaimStatus = "Approved by Coordinator" },
                new Claim { ClaimId = 2, ClaimStatus = "Pending" }
            };
                var approvals = new List<Approval>();
                var manager = new ConsoleTest.ManagerController(claims, approvals);

                var result = manager.GetCoordinatorApprovedClaims();

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Approved by Coordinator", result[0].ClaimStatus);
            }
        }
    }
/*Reference List:
Microsoft.2025a.Create and run unit tests for .NET.[online]. Available at: <http://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022 >. [Accessed 22 October 2025].
Microsoft. 2025b.Get started with unit testing. [online].Available at: <https://learn.microsoft.com/en-us/visualstudio/test/getting-started-with-unit-testing?view=vs-2022&tabs=dotnet%2Cmstest>. [Accessed 22 October 2025].
Troelsen, A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
*/ 