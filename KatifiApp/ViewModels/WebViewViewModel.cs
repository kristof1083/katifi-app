using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatifiApp.ViewModels;

[QueryProperty("AuthUrl", "AuthUrl")]
public partial class WebViewViewModel : BaseViewModel
{

    public WebViewViewModel()
    {

    }

    [ObservableProperty]
    string authUrl;
}
