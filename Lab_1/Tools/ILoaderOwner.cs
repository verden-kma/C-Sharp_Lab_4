using System.ComponentModel;
using System.Windows;

namespace Lab_1.Tools
{
    internal interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { set; }
        bool IsControlEnabled { set; }
    }
}