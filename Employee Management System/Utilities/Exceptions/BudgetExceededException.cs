namespace Employee_Management_System.Utilities.Exceptions
{
    public class BudgetExceededException : Exception
    {
        private const string TechnicalSuffix = "The transaction ended in the trigger. The batch has been aborted.";

        public BudgetExceededException(string rawMessage) : base(CleanMessage(rawMessage)) { }

        private static string CleanMessage(string rawMessage)
        {
            if (string.IsNullOrEmpty(rawMessage))
            {
                return "Budget exceeded, transaction aborted.";
            }

            if (rawMessage.EndsWith(TechnicalSuffix, StringComparison.OrdinalIgnoreCase))
            {
                string userMessage = rawMessage.Substring(0, rawMessage.Length - TechnicalSuffix.Length).Trim();

                return userMessage;
            }

            return rawMessage;
        }
    }
}


