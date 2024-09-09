using LEGO;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace LEGO
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<LegoItem> LegoItems { get; set; } = new ObservableCollection<LegoItem>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            dataGrid.ItemsSource = CollectionViewSource.GetDefaultView(LegoItems);

        }

        private void LoadData(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    MessageBox.Show("The selected file does not exist.");
                    return;
                }

                XDocument xaml = XDocument.Load(filePath);
                foreach (var elem in xaml.Descendants("Item"))
                {
                    cmbcategoryFilter.Items.Add(elem.Element("CategoryName").Value);
                    LegoItems.Add(new LegoItem
                    {
                        ItemID = elem.Element("ItemID").Value,
                        ItemName = elem.Element("ItemName").Value,
                        CategoryName = elem.Element("CategoryName").Value,
                        ColorName = elem.Element("ColorName").Value,
                        Qty = int.Parse(elem.Element("Qty").Value)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}");
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BSX files (*.bsx)|*.bsx|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                LoadData(openFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("hiba");
            }
        }



        private void ApplyFilters(string categoryName)
        {
            var view = CollectionViewSource.GetDefaultView(LegoItems);
            view.Filter = item =>
            {
                var legoItem = item as LegoItem;
                bool categoryMatch = string.IsNullOrEmpty(categoryName) || legoItem.ItemName.ToLower().StartsWith(categoryName);

                return categoryMatch;
            };
        }

        private void OnFilterChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters(ItemNameFilterTextBox.Text);
        }

        private void cmbcategoryFilter_SelectionChanged(object sender, TextChangedEventArgs e)
        {
            kategoriaFilter(cmbcategoryFilter.SelectedItem.ToString());
        }
        private void kategoriaFilter(string valami)
        {
            var view = CollectionViewSource.GetDefaultView(LegoItems);
            view.Filter = item =>
            {
                var legoItem = item as LegoItem;
                bool categoryMatch = string.IsNullOrEmpty(valami) || legoItem.ItemName == valami ;

                return categoryMatch;
            };
        }
    }
}