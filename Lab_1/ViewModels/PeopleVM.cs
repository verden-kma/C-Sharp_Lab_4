﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Lab_1.Models;
using Lab_1.Tools;
using Lab_1.Tools.DataStorage;
using Lab_1.Tools.Managers;
using Lab_1.Tools.MVVM;

namespace Lab_1.ViewModels
{
    internal class PeopleVM
    {
        #region Fields

        private RelayCommand<object> _save;
        private RelayCommand<object> _remove;
        private RelayCommand<object> _sort;
        private RelayCommand<object> _add;
        private RelayCommand<object> _filter;

        public PeopleCollection ViewList { get; set; }
        private List<Person> _backList = SerializedDataStorage.Instance.PeopleList;
        public Person Selected { get; set; }

        #endregion


        internal PeopleVM()
        {
            ViewList = new PeopleCollection(_backList);
        }


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
            if (!string.IsNullOrEmpty(errorMessage)) MessageBox.Show(errorMessage);
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
            return true;
        }

        private void AddCommandImpl()
        {
            MessageBox.Show("not implemented");
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
            return _backList.Count != 0;
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
    }
}