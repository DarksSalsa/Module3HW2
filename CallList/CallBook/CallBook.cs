using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CallList
{
    public class CallBook : IEnumerable<string>
    {
        private SortedDictionary<string, List<Contact>> _contacts = new SortedDictionary<string, List<Contact>>() { };
        private Regex _currentAlphabet;

        public CallBook(CultureInfo culture)
        {
            Culture = culture;
            _currentAlphabet = LocalizationSubstitutioner.SwitchLanguage(culture);
        }

        public CultureInfo Culture { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var title in _contacts.Keys)
            {
                yield return title;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _contacts.GetEnumerator();
        }

        public void Add(string fullname, string number)
        {
            string title = fullname[0].ToString();
            string numberCategory = "0-9";
            string otherCategory = "#";
            if (_currentAlphabet.IsMatch(title))
            {
                if (_contacts.ContainsKey(title))
                {
                    _contacts[title].Add(new Contact(fullname, number));
                }
                else
                {
                    _contacts.Add(title, new List<Contact> { new Contact(fullname, number) });
                }

                _contacts[title].Sort(new ContactsComparer());
            }
            else
            {
                if (Regex.IsMatch(title.ToString(), "[0-9]"))
                {
                    if (_contacts.ContainsKey(numberCategory))
                    {
                        _contacts[numberCategory].Add(new Contact(fullname, number));
                    }
                    else
                    {
                        _contacts.Add(numberCategory, new List<Contact> { new Contact(fullname, number) });
                    }

                    _contacts[numberCategory].Sort(new ContactsComparer());
                }
                else
                {
                    if (_contacts.ContainsKey(otherCategory))
                    {
                        _contacts[otherCategory].Add(new Contact(fullname, number));
                    }
                    else
                    {
                        _contacts.Add(otherCategory, new List<Contact> { new Contact(fullname, number) });
                    }

                    _contacts[otherCategory].Sort(new ContactsComparer());
                }
            }
        }

        public void ShowCallBook()
        {
            Console.WriteLine($"Your current language code is '{Culture.TwoLetterISOLanguageName}'\n");
            if (_contacts.Count == 0)
            {
                Console.WriteLine("Your callbook is empty.");
                return;
            }

            foreach (var title in _contacts)
            {
                Console.WriteLine($"{title.Key}: ");
                foreach (var contact in _contacts[title.Key])
                {
                    Console.WriteLine($"    {contact.FullName} - +{contact.PhoneNumber}");
                }
            }
        }

        public void SwitchLocale(CultureInfo culture)
        {
            _currentAlphabet = LocalizationSubstitutioner.SwitchLanguage(culture);
            Culture = culture;
            Reshuffle(culture);
        }

        private void Reshuffle(CultureInfo culture)
        {
            CallBook replacementBook = new (culture);
            foreach (var title in _contacts.Values)
            {
                foreach (var contact in title)
                {
                    replacementBook.Add(contact.FullName, contact.PhoneNumber);
                }
            }

            _contacts = replacementBook._contacts;
        }
    }
}