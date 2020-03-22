﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Lab_1.Annotations;
using Lab_1.Models;
using Lab_1.Tools;
using Lab_1.Tools.DataStorage;
using Lab_1.Tools.Managers;
using Lab_1.Tools.MVVM;

namespace Lab_1.ViewModels
{
    internal class PeopleVM : INotifyPropertyChanged
    {
        #region Fields

        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthday = DateTime.Today;

        private RelayCommand<object> _save;
        private RelayCommand<object> _remove;
        private RelayCommand<object> _sort;
        private RelayCommand<object> _add;
        private RelayCommand<object> _filter;

        public PeopleCollection ViewList { get; set; }
        private List<Person> _backList = SerializedDataStorage.Instance.PeopleList;
        public Person Selected { get; set; }

        #endregion

        #region FilterProps

        private DateTime _fromDate = DateTime.Today;
        private DateTime _toDate = DateTime.Today;

        private uint _minAge;
        private uint _maxAge;

        public string NameStart { get; set; }
        public string SurnameStart { get; set; }

        public bool NeedDate { get; set; }
        
        public DateTime BDayFrom
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime BDayTo
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
            }
        }

        public uint AgeMin
        {
            get => _minAge;
            set
            {
                if (value > 135)
                {
                    MessageBox.Show("Age = [0;135]");
                    throw new ArgumentException("too old");
                }

                _minAge = value;
            }
        }

        public uint AgeMax
        {
            get => _maxAge;
            set
            {
                if (value > 135)
                {
                    MessageBox.Show("Age = [0;135]");
                    throw new ArgumentException("too old");
                }

                _maxAge = value;
            }
        }

        public bool NeedAdult { get; set; }
        public bool NeedBirthday { get; set; }

        #region Zodiacs

        public bool NeedAries { get; set; }
        public bool NeedTaurus { get; set; }
        public bool NeedGemini { get; set; }
        public bool NeedCancer { get; set; }
        public bool NeedLeo { get; set; }
        public bool NeedVirgo { get; set; }
        public bool NeedLibra { get; set; }
        public bool NeedScorpio { get; set; }
        public bool NeedSagittarius { get; set; }
        public bool NeedCapricorn { get; set; }
        public bool NeedAquarius { get; set; }
        public bool NeedPisces { get; set; }

        public bool NeedMonkey { get; set; }
        public bool NeedRooster { get; set; }
        public bool NeedDog { get; set; }
        public bool NeedPig { get; set; }
        public bool NeedRat { get; set; }
        public bool NeedOx { get; set; }
        public bool NeedTiger { get; set; }
        public bool NeedRabbit { get; set; }
        public bool NeedDragon { get; set; }
        public bool NeedSnake { get; set; }
        public bool NeedHorse { get; set; }
        public bool NeedGoat { get; set; }

        #endregion

        #endregion

        internal PeopleVM()
        {
            ViewList = new PeopleCollection(_backList);
        }

        #region CreationData

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public DateTime Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region SortCmd

        public RelayCommand<object> SortCommand
        {
            get { return _sort ?? (_sort = new RelayCommand<object>(SortCommandImpl, o => CanSort())); }
        }

        private async void SortCommandImpl(object obj)
        {
            string propertyName = obj.ToString();
            IEnumerable<Person> people = ViewList.AsEnumerable();
            // does not support changes from thread different from dispatcher thread
            LoaderManager.Instance.ShowLoader();
            List<Person> sortedPeople = await Task.Run(() =>
                (from person in people
                    orderby person.GetType().GetProperty(propertyName).GetValue(person, null)
                    select person).ToList());
            ViewList.Clear();
            foreach (Person p in sortedPeople)
            {
                ViewList.Add(p);
            }

            LoaderManager.Instance.HideLoader();
        }

        private bool CanSort()
        {
            return true;
        }

        #endregion

        #region SaveCmd

        public RelayCommand<object> SaveCommand
        {
            get
            {
                return _save ?? (_save = new RelayCommand<object>
                    (o => SaveCommandImpl(), o => CanSave()));
            }
        }

        private bool CanSave()
        {
            return true;
        }

        private async void SaveCommandImpl()
        {
            LoaderManager.Instance.ShowLoader();
            string errorMessage = await SaveList();
            LoaderManager.Instance.HideLoader();
            MessageBox.Show(!string.IsNullOrEmpty(errorMessage) ? errorMessage : "Saved successfully.");
        }

        private Task<string> SaveList()
        {
            _backList = ViewList.ToList();
            IEnumerable<PersonCoreData> pd = from person in _backList select person.PersonCoreExtract;
            try
            {
                SerializationManager.Serialize(pd.ToList(), FileFolderHandler.StorageFilePath);
            }
            catch (Exception e)
            {
                return Task.FromResult(e.Message);
            }

            return Task.FromResult("");
        }

        #endregion

        #region RemoveCmd

        public RelayCommand<object> RemoveCommand
        {
            get
            {
                return _remove ?? (_remove = new RelayCommand<object>
                    (o => RemoveCommandImpl(), o => CanRemove()));
            }
        }

        private bool CanRemove()
        {
            return Selected != null;
        }

        private void RemoveCommandImpl()
        {
            ViewList.Remove(Selected);
        }

        #endregion

        #region AddCmd

        public RelayCommand<object> AddCommand
        {
            get
            {
                return _add ?? (_add = new RelayCommand<object>
                    (o => AddCommandImpl(), o => CanAdd()));
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(_name) && !string.IsNullOrWhiteSpace(_surname) &&
                   !string.IsNullOrWhiteSpace(_email);
        }

        private void AddCommandImpl()
        {
            if (Name.Length < 2 || Name.Length > 20
                                || Surname.Length < 2 || Surname.Length > 20
                                || !Person.EmailIsValid(Email)
                                || !Person.IsAlive(Birthday))
            {
                MessageBox.Show("Incorrect person data input.");
                return;
            }

            Person p = new Person(Name, Surname, Email, Birthday);
            ViewList.Add(p);
            _backList.Add(p);
        }

        #endregion

        #region FilterCmd

        public RelayCommand<object> FilterCommand
        {
            get
            {
                return _filter ?? (_filter = new RelayCommand<object>
                    (o => FilterCommandImpl(), o => CanFilter()));
            }
        }

        private bool CanFilter()
        {
            return _backList.Count != 0 &&
                   (!string.IsNullOrEmpty(NameStart) || !string.IsNullOrEmpty(SurnameStart) || NeedDate 
                                                     || NeedBirthday || NeedAdult || AgeMax != 0 || CheckSigns() );
        }

        private bool CheckSigns()
        {
            return NeedAries || NeedTaurus || NeedGemini || NeedCancer || NeedLeo || NeedVirgo || NeedLibra ||
                   NeedScorpio || NeedSagittarius || NeedCapricorn || NeedAquarius || NeedPisces
                   || NeedMonkey || NeedRooster || NeedDog || NeedPig || NeedRat || NeedOx || NeedTiger || NeedRabbit ||
                   NeedDragon || NeedSnake || NeedHorse || NeedGoat;
        }
        
        private void FilterCommandImpl()
        {
            MessageBox.Show("not implemented");
        }

        #endregion

        internal class PeopleCollection : ObservableCollection<Person>
        {
            internal PeopleCollection(List<Person> basePeople) : base(basePeople)
            {
            }
        }

        #region OnProperty

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}