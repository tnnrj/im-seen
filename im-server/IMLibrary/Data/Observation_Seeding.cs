/**
 * This file contains methods for seeding the Observations table of the IM_API_Context
 * Written by Steven Carpadakis & Tanner Jorgensen, U of U School of Computing, Senior Capstone 2021
 **/

using IMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Data
{
    public class Observation_Seeding
    {
        public static void SeedObservations(IM_API_Context context)
        {
            // Look for any existing Users.
            if (context.Observations.Any())
            {
                return;   // DB has been seeded
            }

            var reports = new Observation[]
            {
                new Observation {ObservationDate=new DateTime(2021,10,20,0,0,0),
                    UserName="Dumbledore",
                    StudentID=context.Students.Where(s => s.LastName == "Harry").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "James", StudentLastName = "Harry", Status="New",
                    Severity=4, Description="Put his own name into the Goblet of Fire even though he is underage. Recklessness is concerning."},

                new Observation {ObservationDate=new DateTime(2021,10,19,0,0,0),
                    UserName="McGonagall",
                    StudentID=context.Students.Where(s => s.LastName == "Hermione").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Anna", StudentLastName = "Hermione", Status="New",
                    Severity=2, Description="I didn't see her at the Halloween Feast. But another student mentioned she was crying in the bathroom."},

                new Observation {ObservationDate=new DateTime(2021,10,17,0,0,0),
                    UserName="Filch",
                    StudentID=context.Students.Where(s => s.LastName == "Hermione").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Anna", StudentLastName = "Hermione", Status="New",
                    Severity=1, Description="Towards the end of the night, she was crying on the staircase outside the ballroom. Possibly due to drama with a Weasley boy."},

                new Observation {ObservationDate=new DateTime(2021,10,19,0,0,0),
                    UserName="Filch",
                    StudentID=context.Students.Where(s => s.LastName == "Draco").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Lucius", StudentLastName = "Draco", Status="New",
                    Severity=3, Description="He was sulking on the seventh floor left corridor, pacing back and forth and looking like he was up to no good. Then he disappeared."},

                new Observation {ObservationDate=new DateTime(2021,10,18,0,0,0),
                    // UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Marcus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Abby", StudentLastName="Marcus", Status="New",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,20,0,0,0),
                    // UserName="Tasnim",
                    StudentID=context.Students.Where(s => s.LastName == "Woodley").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Phil", StudentLastName="Woodley", Status="New",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,21,0,0,0),
                    // UserName="Tiffany",
                    StudentID=context.Students.Where(s => s.LastName == "Burch").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Jacob", StudentLastName="Burch", Status="New",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,21,0,0,0),
                    // UserName="Regan",
                    StudentID=context.Students.Where(s => s.LastName == "Garza").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Tiffany", StudentLastName="Garza", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,22,0,0,0),
                    // UserName="Emir",
                    StudentID=context.Students.Where(s => s.LastName == "Rooney").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Gary", StudentLastName="Rooney", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,24,0,0,0),
                    // UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Linus", StudentLastName="Goodwin", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,25,0,0,0),
                    // UserName="Aeryn",
                    StudentID=context.Students.Where(s => s.LastName == "Pittman").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Colin", StudentLastName="Pittman", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,25,0,0,0),
                    // UserName="Harper",
                    StudentID=context.Students.Where(s => s.LastName == "Murray").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Bill", StudentLastName="Murray", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,25,0,0,0),
                    // UserName="Ismael",
                    StudentID=context.Students.Where(s => s.LastName == "Peacock").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Spencer", StudentLastName="Peacock", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,26,0,0,0),
                    // UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Jess", StudentLastName="Mcghee", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,27,0,0,0),
                    // UserName="Kendra",
                    StudentID=context.Students.Where(s => s.LastName == "Herring").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Peter", StudentLastName="Herring", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,28,0,0,0),
                    // UserName="Tabitha",
                    StudentID=context.Students.Where(s => s.LastName == "Ryder").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Gavin", StudentLastName="Ryder", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,28,0,0,0),
                    // UserName="Ayaana",
                    StudentID=context.Students.Where(s => s.LastName == "Prince").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Fiona", StudentLastName="Prince", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,28,0,0,0),
                    // UserName="Tabitha",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Linus", StudentLastName="Goodwin", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,29,0,0,0),
                    // UserName="Aeryn",
                    StudentID=context.Students.Where(s => s.LastName == "Beach").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Harry", StudentLastName="Beach", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,30,0,0,0),
                    // UserName="Maysa",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Linus", StudentLastName="Goodwin", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,30,0,0,0),
                    // UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Beach").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Harry", StudentLastName="Beach", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,1,0,0,0),
                    // UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Harry").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "James", StudentLastName="Harry", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,1,0,0,0),
                    // UserName="Moses",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Jess", StudentLastName="Mcghee", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,2,0,0,0),
                    // UserName="Madiha",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Jess", StudentLastName="Mcghee", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,3,0,0,0),
                    // UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Jess", StudentLastName="Mcghee", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,4,0,0,0),
                    // UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Cecily").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "John", StudentLastName="Cecily", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,5,0,0,0),
                    // UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Mohammed").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Zeke", StudentLastName="Mohammed", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,6,0,0,0),
                    // UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Linus", StudentLastName="Goodwin", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,7,0,0,0),
                    // UserName="Aeryn",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Linus", StudentLastName="Goodwin", Status="New",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,8,0,0,0),
                    // UserName="Ismael",
                    StudentID=context.Students.Where(s => s.LastName == "Cruz").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Tammy", StudentLastName="Cruz", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,8,0,0,0),
                    // UserName="Taran",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Ursula", StudentLastName="Hayward", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,9,0,0,0),
                    // UserName="Madiha",
                    StudentID=context.Students.Where(s => s.LastName == "Louisa").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Josh", StudentLastName="Louisa", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,10,0,0,0),
                    // UserName="Kendra",
                    StudentID=context.Students.Where(s => s.LastName == "Cecily").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "John", StudentLastName="Cecily", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,10,0,0,0),
                    // UserName="Kendra",
                    StudentID=context.Students.Where(s => s.LastName == "Zack").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Jeremy", StudentLastName="Zack", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,11,0,0,0),
                    // UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Nojus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Rhys", StudentLastName="Nojus", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,12,0,0,0),
                    // UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Ismail").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Raimi", StudentLastName="Ismail", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,13,0,0,0),
                    // UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Ursula", StudentLastName="Hayward", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,14,0,0,0),
                    // UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Ursula", StudentLastName="Hayward", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,15,0,0,0),
                    // UserName="Harper",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Ursula", StudentLastName="Hayward", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,15,0,0,0),
                    // UserName="Danielle",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Gus", StudentLastName="Powell", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,15,0,0,0),
                    // UserName="Moses",
                    StudentID=context.Students.Where(s => s.LastName == "Maheen").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Julie", StudentLastName="Maheen", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,16,0,0,0),
                    // UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Nojus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Rhys", StudentLastName="Nojus", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,16,0,0,0),
                    // UserName="Moses",
                    StudentID=context.Students.Where(s => s.LastName == "Zubair").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Robert", StudentLastName="Zubair", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,17,0,0,0),
                    // UserName="Danielle",
                    StudentID=context.Students.Where(s => s.LastName == "Nojus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Rhys", StudentLastName="Nojus", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,17,0,0,0),
                    // UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Arissa").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Wendy", StudentLastName="Arissa", Status="New",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,18,0,0,0),
                    // UserName="Amal",
                    // StudentID=context.Students.Where(s => s.LastName == "Astrid").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Hank", StudentLastName="A", Status="New",
                    Severity=2, Description="Hank mentioned that his home life has been rough lately. He seems to be doing well on the surface but I'm not sure."},

                new Observation {ObservationDate=new DateTime(2021,11,19,0,0,0),
                    // UserName="Tabitha",
                    StudentID=context.Students.Where(s => s.LastName == "Maheen").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Julie", StudentLastName="Maheen", Status="New",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,20,0,0,0),
                    // UserName="Danielle",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Gus", StudentLastName="Powell", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,21,0,0,0),
                    // UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Gus", StudentLastName="Powell", Status="New",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,22,0,0,0),
                    // UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentFirstName = "Gus", StudentLastName="Powell", Status="New",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,11,23,0,0,0),
                    // UserName="Kendra",
                    StudentFirstName = "Gus", StudentLastName="P", Status="New",
                    Severity=3, Description="Gus had a really tough time in PE with the other students today. Not sure of his last name but I think it starts with P."},
            };

            context.Observations.AddRange(reports);
            context.SaveChanges();
        }
    }
}
