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

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username2 { get; set; }
    }
}
