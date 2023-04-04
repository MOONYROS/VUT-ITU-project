using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace project.BL.Models
{
    public abstract record ModelBase : INotifyPropertyChanged, IModel
    {
        public Guid Id { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
