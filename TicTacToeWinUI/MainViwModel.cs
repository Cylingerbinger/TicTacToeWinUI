using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstWinUIAPP;

    public partial class MainViwModel : ObservableObject
    {
        [ObservableProperty]
        public partial int Count {  get; set; }

        public void IncrementCount()
        {
            Count++;
        }
    }

