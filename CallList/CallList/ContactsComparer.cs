namespace CallList
{
    public class ContactsComparer : IComparer<Contact>
    {
        public int Compare(Contact x, Contact y)
        {
            return string.Compare(x.FullName, y.FullName);
        }
    }
}
