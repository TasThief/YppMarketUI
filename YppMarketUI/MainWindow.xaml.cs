﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YppMarketUI.Source;
using WindowsAccessBridgeInterop;
namespace YppMarketUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            Bridge.Initialize();
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e) {
            Bridge.PairWithGame();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            Bridge.CollectMarket();
        }
    }
}
