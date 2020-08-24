using Desktop.Model;
using System.Collections.Generic;

namespace Desktop.Repository
{
    public class ContactRepository
    {
        private class Finder : GenericRepository<IEnumerable<Contact>>
        {
            public Finder(string token) : base(token) { }

            public IEnumerable<Contact> Find(string name)
            {
                return Get($"contact/find?name={name}");
            }
        }

        private class ByIdGetter : GenericRepository<Contact>
        {
            public ByIdGetter(string token) : base(token) { }

            public Contact GetById(int id)
            {
                return Get($"contact/{id}");
            }
        }

        private readonly string _token;

        public ContactRepository(string token)
        {
            _token = token;
        }

        public IEnumerable<Contact> Find(string name)
        {
            var finder = new Finder(_token);
            return finder.Find(name);
        }

        public Contact GetById(int id)
        {
            var getter = new ByIdGetter(_token);
            return getter.GetById(id);
        }
    }
}
