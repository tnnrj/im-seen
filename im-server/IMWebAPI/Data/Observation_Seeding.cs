using IMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Data
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
                new Observation {ObservationDate=new DateTime(2021,9,20,0,0,0),
                    UserName="Dumbledore",
                    StudentID=context.Students.Where(s => s.LastName == "Harry").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName = "Harry",
                    Severity=4, Description="Put his own name into the Goblet of Fire even though he is underage. Recklessness is concerning."},

                new Observation {ObservationDate=new DateTime(2021,9,19,0,0,0),
                    UserName="McGonagall",
                    StudentID=context.Students.Where(s => s.LastName == "Hermione").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName = "Hermione",
                    Severity=2, Description="I didn't see her at the Halloween Feast. But another student mentioned she was crying in the bathroom."},

                new Observation {ObservationDate=new DateTime(2021,9,17,0,0,0),
                    UserName="Filch",
                    StudentID=context.Students.Where(s => s.LastName == "Hermione").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName = "Hermione",
                    Severity=1, Description="Towards the end of the night, she was crying on the staircase outside the ballroom. Possibly due to drama with a Weasley boy."},

                new Observation {ObservationDate=new DateTime(2021,9,19,0,0,0),
                    UserName="Filch",
                    StudentID=context.Students.Where(s => s.LastName == "Draco").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName = "Draco",
                    Severity=3, Description="He was sulking on the seventh floor left corridor, pacing back and forth and looking like he was up to no good. Then he disappeared."},

                new Observation {ObservationDate=new DateTime(2021,9,18,0,0,0),
                    UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Marcus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Marcus",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,20,0,0,0),
                    UserName="Tasnim",
                    StudentID=context.Students.Where(s => s.LastName == "Woodley").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Woodley",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,21,0,0,0),
                    UserName="Tiffany",
                    StudentID=context.Students.Where(s => s.LastName == "Burch").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Burch",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,21,0,0,0),
                    UserName="Regan",
                    StudentID=context.Students.Where(s => s.LastName == "Garza").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Garza",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,22,0,0,0),
                    UserName="Emir",
                    StudentID=context.Students.Where(s => s.LastName == "Rooney").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Rooney",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,24,0,0,0),
                    UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Goodwin",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,25,0,0,0),
                    UserName="Aeryn",
                    StudentID=context.Students.Where(s => s.LastName == "Pittman").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Pittman",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,25,0,0,0),
                    UserName="Harper",
                    StudentID=context.Students.Where(s => s.LastName == "Murray").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Murray",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,25,0,0,0),
                    UserName="Ismael",
                    StudentID=context.Students.Where(s => s.LastName == "Peacock").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Peacock",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,26,0,0,0),
                    UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Mcghee",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,27,0,0,0),
                    UserName="Kendra",
                    StudentID=context.Students.Where(s => s.LastName == "Herring").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Herring",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,28,0,0,0),
                    UserName="Tabitha",
                    StudentID=context.Students.Where(s => s.LastName == "Ryder").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Ryder",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,28,0,0,0),
                    UserName="Ayaana",
                    StudentID=context.Students.Where(s => s.LastName == "Prince").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Prince",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,28,0,0,0),
                    UserName="Tabitha",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Goodwin",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,29,0,0,0),
                    UserName="Aeryn",
                    StudentID=context.Students.Where(s => s.LastName == "Beach").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Beach",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,30,0,0,0),
                    UserName="Maysa",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Goodwin",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,9,30,0,0,0),
                    UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Beach").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Beach",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,1,0,0,0),
                    UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Harry").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Harry",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,1,0,0,0),
                    UserName="Moses",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Mcghee",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,2,0,0,0),
                    UserName="Madiha",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Mcghee",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,3,0,0,0),
                    UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Mcghee").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Mcghee",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,4,0,0,0),
                    UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Cecily").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Cecily",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,5,0,0,0),
                    UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Mohammed").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Mohammed",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,6,0,0,0),
                    UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Goodwin",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,7,0,0,0),
                    UserName="Aeryn",
                    StudentID=context.Students.Where(s => s.LastName == "Goodwin").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Goodwin",
                    Severity=5, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,8,0,0,0),
                    UserName="Ismael",
                    StudentID=context.Students.Where(s => s.LastName == "Cruz").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Cruz",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,8,0,0,0),
                    UserName="Taran",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Hayward",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,9,0,0,0),
                    UserName="Madiha",
                    StudentID=context.Students.Where(s => s.LastName == "Louisa").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Louisa",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,10,0,0,0),
                    UserName="Kendra",
                    StudentID=context.Students.Where(s => s.LastName == "Cecily").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Cecily",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,10,0,0,0),
                    UserName="Kendra",
                    StudentID=context.Students.Where(s => s.LastName == "Zack").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Zack",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,11,0,0,0),
                    UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Nojus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Nojus",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,12,0,0,0),
                    UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Ismail").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Ismail",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,13,0,0,0),
                    UserName="Zakariyya",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Hayward",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,14,0,0,0),
                    UserName="Mario",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Hayward",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,15,0,0,0),
                    UserName="Harper",
                    StudentID=context.Students.Where(s => s.LastName == "Hayward").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Hayward",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,15,0,0,0),
                    UserName="Danielle",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Powell",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,15,0,0,0),
                    UserName="Moses",
                    StudentID=context.Students.Where(s => s.LastName == "Maheen").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Maheen",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,16,0,0,0),
                    UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Nojus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Nojus",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,16,0,0,0),
                    UserName="Moses",
                    StudentID=context.Students.Where(s => s.LastName == "Zubair").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Zubair",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,17,0,0,0),
                    UserName="Danielle",
                    StudentID=context.Students.Where(s => s.LastName == "Nojus").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Nojus",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,17,0,0,0),
                    UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Arissa").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Arissa",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,18,0,0,0),
                    UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Astrid").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Astrid",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,19,0,0,0),
                    UserName="Tabitha",
                    StudentID=context.Students.Where(s => s.LastName == "Maheen").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Maheen",
                    Severity=4, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,20,0,0,0),
                    UserName="Danielle",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Powell",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,21,0,0,0),
                    UserName="Tudor",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Powell",
                    Severity=1, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,22,0,0,0),
                    UserName="Amal",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Powell",
                    Severity=2, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},

                new Observation {ObservationDate=new DateTime(2021,10,23,0,0,0),
                    UserName="Kendra",
                    StudentID=context.Students.Where(s => s.LastName == "Powell").Select(s => s.StudentID).FirstOrDefault(),
                    StudentLastName="Powell",
                    Severity=3, Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
            };

            context.Observations.AddRange(reports);
            context.SaveChanges();
        }
    }
}
