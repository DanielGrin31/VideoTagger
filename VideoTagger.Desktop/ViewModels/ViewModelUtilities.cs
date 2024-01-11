using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace VideoTagger.Desktop.ViewModels
{
    public static class ViewModelUtilities
    {
        public static vmT Build<vmT>(this ViewModelBase shell) where vmT : ViewModelBase
        {
            vmT vm = Ioc.Default.GetRequiredService<vmT>();
            vm.Parent = shell;
            return vm;
        }
    }
}