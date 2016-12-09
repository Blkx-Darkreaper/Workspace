using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Advanced;

namespace DecisionMaker
{
    public class Experience
    {
        protected static SortedDictionary<int, Condition> conditionReference = new SortedDictionary<int, Condition>();
        protected SortedDictionary<int, SituationalHistory> allExperiences = new SortedDictionary<int, SituationalHistory>();
        protected SortedDictionary<int, SituationalHistory> recentExperiences = new SortedDictionary<int, SituationalHistory>();
        protected static Activity idle { get; set; }
        public struct Attitudes
        {
            public const int Normal = 0;
            public const int Safe = 1;
            public const int Aggressive = 2;
        }
        protected struct Result
        {
            public const int Success = 1;
            public const int Failure = -1;
            public const int Other = 0;
        }

        public override string ToString()
        {
            string output = string.Empty;

            foreach (int key in allExperiences.Keys)
            {
                SituationalHistory history = allExperiences[key];

                output += string.Format("{0}: {1}\n", key, history.ToString());
            }

            return output;
        }

        public static void AddCondition(Condition condition)
        {
            int id = condition.Id;
            conditionReference.Add(id, condition);
        }

        public Activity GetResponse(Condition[] situation, int attitude)
        {
            return GetResponse(situation, attitude, null);
        }

        public Activity GetResponse(Condition[] situation, int attitude, Activity[] unavailableActivities)
        {
            SituationalHistory history = GetAllHistory(situation);

            Activity[] responses;

            switch (attitude)
            {
                case Attitudes.Aggressive:
                    responses = history.GetMostAggressiveResponses();
                    break;

                case Attitudes.Safe:
                    responses = history.GetSafestResponses();
                    break;

                case Attitudes.Normal:
                default:
                    responses = history.GetBestResponses();
                    break;
            }

            if (unavailableActivities == null)
            {
                Activity response = responses[0];
                return response;
            }
            if (unavailableActivities.Length == 0)
            {
                Activity response = responses[0];
                return response;
            }

            foreach (Activity response in responses)
            {
                bool activityAvailable = IsActivityAvailable(response, unavailableActivities);
                if (activityAvailable == false)
                {
                    continue;
                }

                return response;
            }

            return idle;
        }

        protected static bool IsActivityAvailable(Activity response, Activity[] unavailableActivities)
        {
            foreach (Activity unavailable in unavailableActivities)
            {
                if (!response.Equals(unavailable))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        public void ResponseExperience(Activity response, Condition[] initialSituation, Condition[] finalSituation)
        {
            int result = GetResult(initialSituation, finalSituation);

            switch (result)
            {
                case Result.Success:
                    AddSuccessfulExperience(response, initialSituation);
                    break;

                case Result.Failure:
                    AddFailedExperience(response, initialSituation);
                    break;

                case Result.Other:
                default:
                    AddExperience(response, initialSituation);
                    break;
            }
        }

        protected virtual int GetResult(Condition[] initialSitutation, Condition[] finalSituation)
        {
            return Result.Other;
        }

        public void AddSuccessfulExperience(Activity response, Condition[] initialSituation)
        {
            AddExperience(response, Result.Success, initialSituation);
        }

        public void AddFailedExperience(Activity response, Condition[] initialSituation)
        {
            AddExperience(response, Result.Failure, initialSituation);
        }

        public void AddExperience(Activity response, Condition[] initialSituation)
        {
            AddExperience(response, Result.Other, initialSituation);
        }

        protected void AddExperience(Activity response, int result, Condition[] initialSituation)
        {
            SituationalHistory[] histories = new SituationalHistory[2];
            histories[0] = GetAllHistory(initialSituation);
            histories[1] = GetRecentHistory(initialSituation);

            foreach (SituationalHistory history in histories)
            {
                switch(result) {
                    case Result.Success:
                        history.ResponseSuccessful(response);
                        break;

                    case Result.Failure:
                        history.ResponseFailed(response);
                        break;

                    case Result.Other:
                    default:
                        history.ResponsePerformed(response);
                        break;
                }
            }
        }

        public void CopyRecentExperienceTo(Experience other)
        {
            foreach (int key in recentExperiences.Keys)
            {
                SituationalHistory history = recentExperiences[key];
                SituationalHistory otherHistory = other.GetAllHistory(key);

                history.CopyTo(otherHistory);
            }
        }

        public static int GetPerformanceHistoryKey(Condition[] situation)
        {
            int key = 0;
            foreach (Condition condition in situation)
            {
                int conditionId = condition.Id;
                int conditionValue = (int)Math.Pow(2, conditionId);

                key += conditionValue;
            }

            return key;
        }

        public static Condition[] GetSituation(int key)
        {
            LinkedList<Condition> conditions = new LinkedList<Condition>();
            string binary = Convert.ToString(key, 2);

            for (int i = 0; i < binary.Length; i++)
            {
                // move right to left
                int position = binary.Length - 1 - i;
                char digit = binary[position];
                int bit = (int)Char.GetNumericValue(digit);

                if (bit == 0)
                {
                    continue;
                }

                Condition condition = conditionReference[i];
                conditions.AddLast(condition);
            }

            return conditions.ToArray<Condition>();
        }

        public SituationalHistory GetAllHistory(Condition[] situation)
        {
            return GetSituationalHistory(situation, ref allExperiences);
        }

        protected SituationalHistory GetAllHistory(int key)
        {
            return GetSituationalHistory(key, ref allExperiences);
        }

        public SituationalHistory GetRecentHistory(Condition[] situation)
        {
            return GetSituationalHistory(situation, ref recentExperiences);
        }

        protected SituationalHistory GetRecentHistory(int key)
        {
            return GetSituationalHistory(key, ref recentExperiences);
        }

        public SituationalHistory GetSituationalHistory(Condition[] situation, ref SortedDictionary<int, SituationalHistory> experiences)
        {
            int key = GetPerformanceHistoryKey(situation);

            return GetSituationalHistory(key, ref experiences);
        }

        protected SituationalHistory GetSituationalHistory(int key, ref SortedDictionary<int, SituationalHistory> experiences)
        {
            SituationalHistory history;

            bool hasKey = experiences.ContainsKey(key);
            if (hasKey == false)
            {
                history = new SituationalHistory();
                experiences.Add(key, history);
            }
            else
            {
                history = experiences[key];
            }

            return history;
        }

        public Activity GetBestAction(Condition[] situation)
        {
            SituationalHistory history = GetAllHistory(situation);

            Activity bestAction = history.GetBestResponse();
            return bestAction;
        }
    }

    //class PerformanceResults
    //{
    //    public float successRate { get; protected set; }
    //    public float failureRate { get; protected set; }

    //    public PerformanceResults(int successes, int failures, int otherHistory)
    //    {
    //        float total = successes + failures + otherHistory;

    //        this.successRate = (float)successes / total;
    //        this.failureRate = (float)failures / total;
    //    }

    //    public PerformanceResults(float successRate, float failureRate)
    //    {
    //        this.successRate = successRate;
    //        this.failureRate = failureRate;
    //    }
    //}

    public class SituationalHistory
    {
        protected SortedDictionary<Activity, Response> allResponses { get; set; }
        protected IComparer<Response> bestComparator { get; set; }
        protected IComparer<Response> mostSuccessfulComparator { get; set; }
        protected IComparer<Response> safestComparator { get; set; }

        public SituationalHistory()
        {
            allResponses = new SortedDictionary<Activity, Response>();

            bestComparator = new BestResponseComparator();
            mostSuccessfulComparator = new MostSuccessfulResponseComparator();
            safestComparator = new SafestResponseComparator();
        }

        public override string ToString()
        {
            string output = string.Empty;
            int total = allResponses.Values.Count;
            for (int i = 0; i < total; i++)
            {
                if (i != 0)
                {
                    output += "\n";
                }

                Response response = allResponses.Values.ElementAt(i);
                output += response.ToString();
            }

            return output;
        }

        public void CopyTo(SituationalHistory otherHistory)
        {
            foreach (Response response in allResponses.Values)
            {
                Activity activity = response.activity;

                bool hasResponse = otherHistory.allResponses.ContainsKey(activity);
                if (hasResponse == false)
                {
                    otherHistory.AddResponse(activity);
                }

                Response otherResponse = otherHistory.allResponses[activity];
                response.CopyTo(otherResponse);
            }
        }

        protected void AddResponse(Activity activity)
        {
            Response response = new Response(activity);
            allResponses.Add(activity, response);
        }

        public void ResponseSuccessful(Activity activity)
        {
            Response response = GetResponse(activity);
            response.AddSuccess();
        }

        public void ResponseFailed(Activity activity)
        {
            Response response = GetResponse(activity);
            response.AddFailure();
        }

        public void ResponsePerformed(Activity activity)
        {
            Response response = GetResponse(activity);
            response.AddOccurrence();
        }

        protected Response GetResponse(Activity activity)
        {
            bool hasKey = allResponses.ContainsKey(activity);
            if (hasKey == false)
            {
                AddResponse(activity);
            }

            Response response = allResponses[activity];
            return response;
        }

        public Activity[] GetSortedResponses(IComparer<Response> comparator)
        {
            List<Response> sortedResponses = allResponses.Values.ToList();
            sortedResponses.Sort(comparator);

            int totalResponses = sortedResponses.Count;

            Activity[] responses = new Activity[totalResponses];
            for (int i = 0; i < totalResponses; i++)
            {
                Response response = sortedResponses.ElementAt(i);
                Activity activity = response.activity;
                responses[i] = activity;
            }

            return responses;
        }

        public Activity GetFirstSortedResponse(IComparer<Response> comparator)
        {
            List<Response> sortedResponses = allResponses.Values.ToList();
            sortedResponses.Sort(comparator);

            Response firstResponse = sortedResponses.First();

            Activity firstActivity = firstResponse.activity;
            return firstActivity;
        }

        public Activity[] GetBestResponses()
        {
            return GetSortedResponses(bestComparator);
        }

        public Activity GetBestResponse()
        {
            return GetFirstSortedResponse(bestComparator);
        }

        public Activity[] GetMostAggressiveResponses()
        {
            return GetSortedResponses(mostSuccessfulComparator);
        }

        public Activity GetMostSuccessfulResponse()
        {
            return GetFirstSortedResponse(mostSuccessfulComparator);
        }

        public Activity[] GetSafestResponses()
        {
            return GetSortedResponses(safestComparator);
        }

        public Activity GetSafestResponse()
        {
            return GetFirstSortedResponse(safestComparator);
        }
    }

    class BestResponseComparator : IComparer<Response>
    {
        public int Compare(Response x, Response y)
        {
            int xSuccesses = x.successes;
            int ySuccesses = y.successes;

            int xFailures = x.failures;
            int yFailures = y.failures;
            
            int xOccurrences = x.occurrences;
            int yOccurrences = y.occurrences;

            int xSuccessRate = (int)((float)xSuccesses / (float)xOccurrences * 100);
            int ySuccessRate = (int)((float)ySuccesses / (float)yOccurrences * 100);

            int xFailureRate = (int)((float)xFailures / (float)xOccurrences * 100);
            int yFailureRate = (int)((float)yFailures / (float)yOccurrences * 100);

            int xOverallRate = xSuccessRate - xFailureRate;
            int yOverallRate = ySuccessRate - yFailureRate;

            //int result = yOverallRate.CompareTo(xOverallRate);
            int result = yOverallRate - xOverallRate;
            if (result != 0)
            {
                return result;
            }

            //result = yOccurrences.CompareTo(xOccurrences);
            result = yOccurrences - xOccurrences;
            return result;
        }
    }

    class MostSuccessfulResponseComparator : IComparer<Response>
    {
        public int Compare(Response x, Response y)
        {
            int xSuccesses = x.successes;
            int ySuccesses = y.successes;

            int result = ySuccesses.CompareTo(xSuccesses);
            //int result = ySuccesses - xSuccesses;
            if (result != 0)
            {
                return result;
            }

            int xFailures = x.failures;
            int yFailures = y.failures;

            result = xFailures.CompareTo(yFailures);
            //result = xFailures - yFailures;
            if (result != 0)
            {
                return result;
            }

            int xOccurrences = x.occurrences;
            int yOccurrences = y.occurrences;

            result = yOccurrences.CompareTo(xOccurrences);
            //result = yOccurrences - xOccurrences;
            return result;
        }
    }

    class SafestResponseComparator : IComparer<Response>
    {
        public int Compare(Response x, Response y)
        {
            int xFailures = x.failures;
            int yFailures = y.failures;

            int result = xFailures.CompareTo(yFailures);
            //int result = xFailures - yFailures;
            if (result != 0)
            {
                return result;
            }

            int xOccurrences = x.occurrences;
            int yOccurrences = y.occurrences;

            result = yOccurrences.CompareTo(xOccurrences);
            //result = yOccurrences - xOccurrences;
            return result;
        }
    }

    //class SituationalHistory {
    //    protected SortedSet<Response> successfulPerformance { get; set; }
    //    protected SortedSet<Response> failedPerformance { get; set; }
    //    protected SortedSet<Response> otherPerformance { get; set; }
    //    protected SortedSet<Activity> allPerformedActivities { get; set; }

    //    public SituationalHistory()
    //    {
    //        successfulPerformance = new SortedSet<Response>();
    //        failedPerformance = new SortedSet<Response>();
    //        otherPerformance = new SortedSet<Response>();
    //        allPerformedActivities = new SortedSet<Activity>();
    //    }

    //    public Activity GetMostSuccessful()
    //    {
    //        Response mostSuccessful = successfulPerformance.Max;
    //        Activity action = mostSuccessful.activity;
    //        return action;
    //    }

    //    public Activity[] GetActionsBySuccess()
    //    {
    //        Activity[] allActions = new Activity[successfulPerformance.Count];
    //        for (int level = 0; level < successfulPerformance.Count; level++)
    //        {
    //            Response nextActionPerformed = successfulPerformance.ElementAt(level);
    //            Activity nextAction = nextActionPerformed.activity;
    //            allActions[level] = nextAction;
    //        }

    //        return allActions;
    //    }

    //    public Activity GetLeastFailed()
    //    {
    //        Response leastFailed = failedPerformance.Min;
    //        Activity action = leastFailed.activity;
    //        return action;
    //    }

    //    public Activity[] GetActionsByFailure()
    //    {
    //        Activity[] allActions = new Activity[failedPerformance.Count];
    //        for (int level = failedPerformance.Count; level >= 0; level--)
    //        {
    //            Response nextActionPerformed = failedPerformance.ElementAt(level);
    //            Activity nextAction = nextActionPerformed.activity;
    //            allActions[level] = nextAction;
    //        }

    //        return allActions;
    //    }

    //    public Activity GetBestResponse()
    //    {
    //        Activity bestAction = null;
    //        float bestActionSuccessRate = 0f;
    //        float bestActionFailureRate = 0f;

    //        foreach (Activity action in allPerformedActivities)
    //        {
    //            if (bestAction == null)
    //            {
    //                bestAction = action;
    //            }

    //            PerformanceResults results = GetPerformanceResults(action);
    //            float successRate = results.successRate;
    //            float failureRate = results.failureRate;

    //            float outcome = successRate - failureRate;
    //            float bestOutcome = bestActionSuccessRate - bestActionFailureRate;

    //            if (outcome < bestOutcome)
    //            {
    //                continue;
    //            }

    //            // tie breaker
    //            if (outcome == bestOutcome)
    //            {
    //                if (successRate < bestActionSuccessRate)
    //                {
    //                    continue;
    //                }

    //                if (successRate == bestActionSuccessRate)
    //                {
    //                    if (failureRate >= bestActionFailureRate)
    //                    {
    //                        continue;
    //                    }
    //                }
    //            }

    //            bestActionSuccessRate = successRate;
    //            bestActionFailureRate = failureRate;
    //            bestAction = action;
    //        }

    //        return bestAction;
    //    }

    //    public void AddSuccess(Activity successfulAction)
    //    {
    //        AddOccurrence(successfulAction, successfulPerformance);
    //    }

    //    public void AddFailure(Activity failedAction)
    //    {
    //        AddOccurrence(failedAction, failedPerformance);
    //    }

    //    public void AddOther(Activity otherAction)
    //    {
    //        AddOccurrence(otherAction, otherPerformance);
    //    }

    //    public void AddOccurrence(Activity action, SortedSet<Response> actionsPerformed)
    //    {
    //        Response performedAction = GetResponse(action, actionsPerformed);
    //        if (performedAction == null)
    //        {
    //            // No match found, add new entry
    //            actionsPerformed.Add(new Response(action));
    //            allPerformedActivities.Add(action);
    //            return;
    //        }

    //        performedAction.AddOccurrence();
    //    }

    //    public PerformanceResults GetPerformanceResults(Activity action)
    //    {
    //        Response successfulActions = GetResponse(action, successfulPerformance);
    //        Response failedActions = GetResponse(action, failedPerformance);
    //        Response otherActions = GetResponse(action, otherPerformance);

    //        int successes = successfulActions.occurrences;
    //        int failures = failedActions.occurrences;
    //        float totalOccurrences = successes + failures + otherActions.occurrences;

    //        float successRate = (float)successes / totalOccurrences;
    //        float failureRate = (float)failures / totalOccurrences;

    //        PerformanceResults results = new PerformanceResults(successRate, failureRate);
    //        return results;
    //    }

    //    protected static Response GetResponse(Activity action, SortedSet<Response> actionsPerformed)
    //    {
    //        foreach (Response performedAction in actionsPerformed)
    //        {
    //            bool match = action.Equals(performedAction.activity);
    //            if (match == false)
    //            {
    //                continue;
    //            }

    //            return performedAction;
    //        }

    //        return null;
    //    }

    //    public Activity[] GetAllPerformedActions()
    //    {
    //        return allPerformedActivities.ToArray();
    //    }
    //}

    public class Response : IComparable<Response>
    {
        public int successes { get; protected set; }
        public int failures { get; protected set; }
        public int occurrences { get; protected set; }
        public Activity activity { get; protected set; }

        public Response(Activity activity)
        {
            this.successes = 0;
            this.failures = 0;
            this.occurrences = 0;
            this.activity = activity;
        }

        public override string ToString()
        {
            string output = string.Format("Activity: {0}, Successes: {1}, Failures: {2}, Total: {3}", activity.ToString(), successes, failures, occurrences);
            return output;
        }

        public int CompareTo(Response other)
        {
            if (other == null)
            {
                return 1;
            }

            int comparison = occurrences.CompareTo(other.occurrences);
            return comparison;
        }

        public void CopyTo(Response other)
        {
            if (other == null)
            {
                throw new NullReferenceException();
            }

            other.successes += successes;
            other.failures += failures;
            other.occurrences += occurrences;
        }

        public void AddOccurrence()
        {
            occurrences++;
        }

        public void AddSuccess()
        {
            successes++;
            AddOccurrence();
        }

        public void AddFailure()
        {
            failures++;
            AddOccurrence();
        }
    }
}