using IMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
{
    public class Group_Seeding
    {
        public static void SeedGroups(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Groups.Any())
            {
                return;   // DB has been seeded
            }

            var groups = new Group[]
            {
                new Group {PrimaryUserName="mcgonogall", GroupName="A-F"},
                new Group {PrimaryUserName="snape", GroupName="G-L"},
                new Group {PrimaryUserName="flitwick", GroupName="M-R"},
                new Group {PrimaryUserName="sprout", GroupName="S-Z"},
            };

            context.Groups.AddRange(groups);
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
                          Group = context.Groups.Where(g => g.GroupName == "A-F").FirstOrDefault(),
                          Student = student
                      });
                }
                else if (student.LastName.CompareTo("L") <= 0)
                {
                    delegations.Add(
                      new Delegation
                      {
                          Group = context.Groups.Where(g => g.GroupName == "G-L").FirstOrDefault(),
                          Student = student
                      });
                }
                else if (student.LastName.CompareTo("R") <= 0)
                {
                    delegations.Add(
                      new Delegation
                      {
                          Group = context.Groups.Where(g => g.GroupName == "M-R").FirstOrDefault(),
                          Student = student
                      });
                }
                else
                {
                    delegations.Add(
                      new Delegation
                      {
                          Group = context.Groups.Where(g => g.GroupName == "S-Z").FirstOrDefault(),
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
                new Supporter {UserName="filch", Group=context.Groups.Where(g => g.GroupName == "A-F").FirstOrDefault()}
            };

            context.Supporters.AddRange(supporters);
            context.SaveChanges();
        }
    }
}
