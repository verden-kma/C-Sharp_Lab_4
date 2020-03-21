using System.Collections.Generic;
using Lab_1.Models;

namespace Lab_1.Tools.DataStorage
{
    internal interface IDataStorage
    {
        // bool UserExists(string login);
        //
        // Person GetUserByLogin(string login);

        void AddPerson(Person p);

        void RemovePerson(Person p);
        
        List<Person> PeopleList { get; }
    }
}
