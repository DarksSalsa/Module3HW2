namespace CallList
{
    public class Contact
    {
        public Contact(string fullName, string phoneNumber)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }

        public string FullName { get; init; }
        public string PhoneNumber { get; init; }
    }
}
