using PROG6212_POE.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleTest;
using ConsoleTest.Models;
using System.Collections.Generic;

namespace TestProject
{

        [TestClass]
        public class ControllerTests
        {
            [TestMethod]
            public void Lecturer_CreateClaim_ShouldAddClaim()
            {
                var claims = new List<Claim>();
                var lecturer = new ConsoleTest.LecturerController(claims);

                string result = lecturer.CreateClaim(1, 10, 200m, "October");

                Assert.AreEqual(1, claims.Count);
                Assert.AreEqual("Pending", claims[0].ClaimStatus);
                Assert.IsTrue(result.Contains("October"));
            }

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
                Assert.AreEqual("Approved successfully", result);
            }

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