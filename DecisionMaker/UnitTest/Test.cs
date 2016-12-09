using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DecisionMaker;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void ResponseComparison()
        {
            Activity action = new Activity(null);
            Response action1 = new Response(action);
            action1.AddOccurrence();

            Response action2 = new Response(action);

            SortedSet<Response> sortedSet = new SortedSet<Response>();
            sortedSet.Add(action1);
            sortedSet.Add(action2);

            Response max = sortedSet.Max;
            Assert.IsTrue(max.Equals(action1));
        }

        [TestMethod]
        public void BestResponseComparison()
        {
            Activity expectedBest = new Activity("Best", null);
            Activity expectedMostSuccessful = new Activity("Most Successful", null);
            Activity expectedLeastFailed = new Activity("Least Failed", null);
            Activity expectedMostPerformed = new Activity("Most Performed", null);
            Activity expectedWorst = new Activity("Worst", null);
            
            SituationalHistory history = new SituationalHistory();

            Activity[] successful = new Activity[] { expectedBest, expectedBest, expectedMostSuccessful, expectedMostSuccessful, expectedMostSuccessful };
            foreach (Activity activity in successful)
            {
                history.ResponseSuccessful(activity);
            }

            Activity[] failed = new Activity[] { expectedMostSuccessful, expectedMostSuccessful, expectedBest, expectedMostPerformed, expectedWorst, expectedWorst, expectedWorst };
            foreach (Activity activity in failed)
            {
                history.ResponseFailed(activity);
            }

            Activity[] performed = new Activity[] { expectedLeastFailed, expectedMostPerformed, expectedMostPerformed, expectedMostPerformed, expectedMostPerformed, expectedMostPerformed, expectedMostPerformed };
            foreach (Activity activity in performed)
            {
                history.ResponsePerformed(activity);
            }

            string status = history.ToString();

            Activity best = history.GetBestResponse();
            Assert.IsTrue(expectedBest.Equals(best));

            Activity mostSuccessful = history.GetMostSuccessfulResponse();
            Assert.IsTrue(expectedMostSuccessful.Equals(mostSuccessful));

            Activity leastFailed = history.GetSafestResponse();
            Assert.IsTrue(expectedLeastFailed.Equals(leastFailed));
        }

        [TestMethod]
        public void Situation()
        {
            Condition[] conditions = SetupConditions();

            Condition[] expectedSituation = new Condition[] { conditions[1], conditions[3] };
            int key = Experience.GetPerformanceHistoryKey(expectedSituation);
            Assert.IsTrue(key == 10);

            Condition[] situation = Experience.GetSituation(key);

            Condition first = situation[0];
            Assert.IsTrue(first.Equals(conditions[1]));

            Condition second = situation[1];
            Assert.IsTrue(second.Equals(conditions[3]));
        }

        private static Condition[] SetupConditions()
        {
            Condition[] conditions = new Condition[10];
            conditions[0] = new Condition("First");
            conditions[1] = new Condition("Second");
            conditions[2] = new Condition("Third");
            conditions[3] = new Condition("Fourth");
            conditions[4] = new Condition("Fifth");
            conditions[5] = new Condition("Sixth");
            conditions[6] = new Condition("Seventh");
            conditions[7] = new Condition("Eighth");
            conditions[8] = new Condition("Nineth");
            conditions[9] = new Condition("Tenth");

            foreach (Condition conditionToAdd in conditions)
            {
                Experience.AddCondition(conditionToAdd);
            }

            return conditions;
        }

        [TestMethod]
        public void CopyExperience()
        {
            Condition[] conditions = SetupConditions();

            Experience teacher = new Experience();
            Experience student = new Experience();

            Activity victory = new Activity("Victory", null);
            Activity defeat = new Activity("Defeat", null);
            Activity stalemate = new Activity("Stalemate", null);

            Condition[] victorySituation = new Condition[] { conditions[0], conditions[2], conditions[4] };
            teacher.AddSuccessfulExperience(victory, victorySituation);

            Condition[] defeatSituation = new Condition[] { conditions[1], conditions[3], conditions[5] };
            teacher.AddFailedExperience(defeat, defeatSituation);

            Condition[] otherSituation = new Condition[] { conditions[6], conditions[7], conditions[8] };
            teacher.AddExperience(stalemate, otherSituation);

            string teacherStatus = teacher.ToString();

            teacher.CopyRecentExperienceTo(student);

            string studentStatus = student.ToString();

            Assert.IsTrue(teacherStatus.Equals(studentStatus));
        }
    }
}