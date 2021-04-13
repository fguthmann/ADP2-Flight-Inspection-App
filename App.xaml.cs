﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ex1.Model;
using ex1.ViewModel;

namespace ex1
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            View.MenuWindow window = new View.MenuWindow();
            

            
            //window.DataContext = flightInfoViewModel;
            window.Show();

        }
    }
}