﻿using System;
using System.Collections.Generic;
using Lab_1.Models;
using Lab_1.Tools.Managers;

namespace Lab_1.Tools.DataStorage
{
    internal class SerializedDataStorage : IDataStorage
    {
        public List<Person> PeopleList { get; }

        private static readonly object Locker = new object();
        private static SerializedDataStorage _instance;

        public static SerializedDataStorage Instance
        {
            get
            {
                lock (Locker)
                {
                    return _instance ?? (_instance = new SerializedDataStorage());
                }
            }
        }

        private SerializedDataStorage()
        {
            try
            {
                List<PersonCoreData> peopleData =
                    SerializationManager.Deserialize<List<PersonCoreData>>(FileFolderHandler.StorageFilePath);
                PeopleList = new List<Person>(peopleData.Count);
                foreach (PersonCoreData pd in peopleData)
                {
                    PeopleList.Add(new Person(pd.Name, pd.Surname, pd.Email, pd.Birthday));
                }
            }
            catch (Exception)
            {
                PeopleList = GeneratePersonList();
            }
        }

        private static List<Person> GeneratePersonList()
        {
            const int numPeople = 10;
            List<Person> list = new List<Person>(numPeople);
            Random random = new Random();
            for (int i = 0; i < numPeople; i++)
            {
                char[] name = new char[random.Next() % 15 + 3];
                for (int j = 0; j < name.Length; j++)
                {
                    name[j] = (char) (random.Next() % 25 + 65);
                }

                char[] surname = new char[random.Next() % 15 + 3];
                for (int j = 0; j < surname.Length; j++)
                {
                    surname[j] = (char) (random.Next() % 25 + 65);
                }

                DateTime bday = new DateTime(1900 + random.Next() % 100, random.Next() % 12 + 1,
                    random.Next() % 28 + 1);
                Person p = new Person(new string(name), new string(surname),
                    $"{new string(name)}.{new string(surname)}@gmail.com", bday);
                list.Add(p);
            }

            return list;
        }

        public void AddPerson(Person user)
        {
            PeopleList.Add(user);
            SaveChanges();
        }

        public void RemovePerson(Person p)
        {
            PeopleList.Remove(p);
            SaveChanges();
        }


        private void SaveChanges()
        {
            SerializationManager.Serialize(PeopleList, FileFolderHandler.StorageFilePath);
        }
    }
}