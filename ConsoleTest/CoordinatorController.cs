
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTest.Models;

namespace ConsoleTest
{
    /*Troelson and Japiske, 2021*/
    public class CoordinatorController
    {
        private readonly List<Claim> _claims;
        private readonly List<Approval> _approvals;

        public CoordinatorController(List<Claim> claims, List<Approval> approvals)
        {
            _claims = claims;
            _approvals = approvals;
        }

        public List<Claim> GetPendingClaims()
        {
            return _claims.Where(c => c.ClaimStatus == "Pending").ToList();
        }

        public string Approve(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null) return "Claim not found";
            claim.ClaimStatus = "Approved by Coordinator";
            _approvals.Add(new Approval
            {
                ClaimId = id,
                Decision = "Approved",
                Role = "Coordinator",
                ApprovalDate = DateTime.Now
            });
            return "Approved successfully";
        }

        public string Reject(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null) return "Claim not found";
            claim.ClaimStatus = "Rejected by Coordinator";
            _approvals.Add(new Approval
            {
                ClaimId = id,
                Decision = "Rejected",
                Role = "Coordinator",
                ApprovalDate = DateTime.Now
            });
            return "Rejected successfully";
        }
    }
}

/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */