using System;
using System.Collections.Generic;
using ConsoleTest.Models;

namespace ConsoleTest
{
    /*Troelson and Japiske, 2021*/
        public class LecturerController
        {
            private readonly List<Claim> _claims;

            public LecturerController(List<Claim> claims)
            {
                _claims = claims;
            }

            public string CreateClaim(int lecturerId, int totalHours, decimal hourlyRate, string month)
            {
                decimal totalAmount = totalHours * hourlyRate;
                var claim = new Claim
                {
                    ClaimId = _claims.Count + 1,
                    LecturerId = lecturerId,
                    Month = month,
                    TotalHoursWorked = totalHours,
                    HourlyRate = hourlyRate,
                    TotalAmount = totalAmount,
                    ClaimStatus = "Pending",
                    SubmissionDate = DateTime.Now
                };
                _claims.Add(claim);
                return $"Claim created successfully for {month}, amount: {totalAmount:C}";
            }
        }
    }
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */