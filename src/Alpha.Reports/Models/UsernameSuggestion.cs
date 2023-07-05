namespace Alpha.Reports.Models
{
    public class UsernameSuggestion
    {
        public UsernameSuggestion()
        {
        }

        public UsernameSuggestion(string username, string firstName, string lastName, string username2)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Username2 = username2;
        }

        public string Username { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Username2 { get; }
    }
}
