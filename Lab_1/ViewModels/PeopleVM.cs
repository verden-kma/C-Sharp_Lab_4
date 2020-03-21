using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Lab_1.Models;
using Lab_1.Tools;
using Lab_1.Tools.DataStorage;
using Lab_1.Tools.Managers;
using Lab_1.Tools.MVVM;

namespace Lab_1.ViewModels
{
    internal class PeopleVM
    {
        private RelayCommand<object> _save;
        private RelayCommand<object> _remove;
        
        public ObservableCollection<Person> ViewList { get; set; }
        private List<Person> _backList = SerializedDataStorage.Instance.PeopleList;

        internal PeopleVM()
        {
            ViewList = new PeopleCollection(_backList);
        }

        public Person Selected { get; set; }
        
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
            try
            {
                SerializationManager.Serialize(_backList, FileFolderHandler.StorageFilePath);
            }
            catch (Exception e)
            {
                return Task.FromResult(e.Message);
            }

            return null;
        }

        #endregion

        #region RemoveCmd

        public RelayCommand<object> RemoveCommand
        {
            get { return _remove ?? (_remove = new RelayCommand<object>
                (o => RemoveCommandImpl(), o => CanRemove())); }
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

        private class PeopleCollection : ObservableCollection<Person>
        {
            internal PeopleCollection(List<Person> basePeople) : base(basePeople)
            {
            }
        }
    }
}