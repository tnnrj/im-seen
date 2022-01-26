/**
 * This file contains methods for seeding the StudentGroups table of the IM_API_Context
 * Written by Steven Carpadakis & Tanner Jorgensen, U of U School of Computing, Senior Capstone 2021
 **/

using IMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Data
{
    public class Group_Seeding
    {
        public static void SeedGroups(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.StudentGroups.Any())
            {
                return;   // DB has been seeded
            }

            var groups = new StudentGroup[]
            {
                new StudentGroup {PrimaryUserName="mcgonagall", StudentGroupName="A-F"},
                new StudentGroup {PrimaryUserName="snape", StudentGroupName="G-L"},
                new StudentGroup {PrimaryUserName="flitwick", StudentGroupName="M-R"},
                new StudentGroup {PrimaryUserName="sprout", StudentGroupName="S-Z"},
            };

            context.StudentGroups.AddRange(groups);
            context.SaveChanges();
        }

        public static void SeedDelegations(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Delegations.Any())
            {
                return;   // DB has been seeded
            }

            List<Delegation> delegations = new List<Delegation>();

            var students = context.Students.OrderBy(s => s.LastName).ToList();

            foreach (Student student in students)
            {
                if (student.LastName.CompareTo("F") <= 0)
                {
                    delegations.Add(
                      new Delegation
                      {
                          StudentGroup = context.StudentGroups.Where(g => g.StudentGroupName == "A-F").FirstOrDefault(),
                          Student = student
                      });
                }
                else if (student.LastName.CompareTo("L") <= 0)
                {
                    delegations.Add(
                      new Delegation
                      {
                          StudentGroup = context.StudentGroups.Where(g => g.StudentGroupName == "G-L").FirstOrDefault(),
                          Student = student
                      });
                }
                else if (student.LastName.CompareTo("R") <= 0)
                {
                    delegations.Add(
                      new Delegation
                      {
                          StudentGroup = context.StudentGroups.Where(g => g.StudentGroupName == "M-R").FirstOrDefault(),
                          Student = student
                      });
                }
                else
                {
                    delegations.Add(
                      new Delegation
                      {
                          StudentGroup = context.StudentGroups.Where(g => g.StudentGroupName == "S-Z").FirstOrDefault(),
                          Student = student
                      });
                }
            }

            context.Delegations.AddRange(delegations.ToArray());
            context.SaveChanges();
        }

        public static void SeedSupporters(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Supporters.Any())
            {
                return;   // DB has been seeded
            }

            var supporters = new Supporter[]
            {
                new Supporter {UserName="filch", StudentGroup=context.StudentGroups.Where(g => g.StudentGroupName == "A-F").FirstOrDefault()}
            };

            context.Supporters.AddRange(supporters);
            context.SaveChanges();
        }
    }
}
