using System;
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

namespace Sorea_Stefania_Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private DoughnutMachine myDoughnutMachine;

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine = new DoughnutMachine();
            myDoughnutMachine.DoughnutComplete += new DoughnutMachine.DoughnutCompleteDelegate(DoughnutCompleteHandler);
            cmbType.ItemsSource = PriceList;
            cmbType.DisplayMemberPath = "Key";
            cmbType.SelectedValuePath = "Value";
        }
        private int mRaisedGlazed;
        private int mRaisedSugar;
        private int mFilledLemon;
        private int mFilledChocolate;
        private int mFilledVanilla;

        private void exitToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void glazedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            glazedMenuItem.IsChecked = true;
            sugarMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Glazed);
        }

        private void sugarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            sugarMenuItem.IsChecked = true;
            glazedMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Sugar);
        }

        private void mnuStop_Click(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine.Enabled = false;
        }

        private void DoughnutCompleteHandler()
        {
            switch (myDoughnutMachine.Flavor)
            {
                case DoughnutType.Glazed:
                    mRaisedGlazed++;
                    txtRaisedGlazed.Text = mRaisedGlazed.ToString();
                    break;
                case DoughnutType.Sugar:
                    mRaisedSugar++;
                    txtSugarRaised.Text = mRaisedSugar.ToString();
                    break;
                case DoughnutType.Lemon:
                    mFilledLemon++;
                    txtLemonFilled.Text = mFilledLemon.ToString();
                    break;
                case DoughnutType.Chocolate:
                    mFilledChocolate++;
                    txtChocolateFilled.Text = mFilledChocolate.ToString();
                    break;
                case DoughnutType.Vanilla:
                    mFilledVanilla++;
                    txtVanillaFilled.Text = mFilledVanilla.ToString();
                    break;
            }
        }

        private void txtQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if(!(e.Key>=Key.D0 && e.Key<=Key.D9))
            {
                MessageBox.Show("Numai cifre se pot introduce!", "InputError", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilledItems_Click(object sender, RoutedEventArgs e)
        {
            string DoughnutFlavour;

            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            DoughnutFlavour = SelectedItem.Header.ToString();
            Enum.TryParse(DoughnutFlavour, out DoughnutType myFlavour);
            myDoughnutMachine.MakeDoughnuts(myFlavour);
        }

        private void FilledItemsShow_Click(object sender, RoutedEventArgs e)
        {
            string mesaj;
            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            mesaj = SelectedItem.Header.ToString() + " doughnuts are being cooked!";
            this.Title = mesaj;
        }

        KeyValuePair<DoughnutType, double>[] PriceList = {
            new KeyValuePair<DoughnutType, double>(DoughnutType.Sugar, 2.5),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Glazed,3),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Chocolate,4.5),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Vanilla,4),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Lemon,3.5)
        };

        DoughnutType selectedDoughnut;

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtPrice.Text = cmbType.SelectedValue.ToString();
            KeyValuePair<DoughnutType, double> selectedEntry =(KeyValuePair<DoughnutType, double>)cmbType.SelectedItem;
            selectedDoughnut = selectedEntry.Key;
        }
        private int ValidateQuantity(DoughnutType selectedDoughnut)
        {
            int q = int.Parse(txtQuantity.Text);
            int r = 1;

            switch (selectedDoughnut)
            {
                case DoughnutType.Glazed:
                    if (q > mRaisedGlazed)
                        r = 0;
                    break;
                case DoughnutType.Sugar:
                    if (q > mRaisedSugar)
                        r = 0;
                    break;
                case DoughnutType.Chocolate:
                    if (q > mFilledChocolate)
                        r = 0;
                    break;
                case DoughnutType.Lemon:
                    if (q > mFilledLemon)
                        r = 0;
                    break;
                case DoughnutType.Vanilla:
                    if (q > mFilledVanilla)
                        r = 0;
                    break;
            }
            return r;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateQuantity(selectedDoughnut) > 0)
            {
                lstSale.Items.Add(txtQuantity.Text + " " + selectedDoughnut.ToString() +":" + txtPrice.Text + " " + double.Parse(txtQuantity.Text) *double.Parse(txtPrice.Text));
            }
            else
            {
                MessageBox.Show("Cantitatea introdusa nu este disponibila in stoc!");
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            lstSale.Items.Remove(lstSale.SelectedItem);
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            txtTotal.Text = (double.Parse(txtTotal.Text) + double.Parse(txtQuantity.Text)* double.Parse(txtPrice.Text)).ToString();
            foreach (string s in lstSale.Items)
            {
                switch (s.Substring(s.IndexOf(" ") + 1, s.IndexOf(":") - s.IndexOf(" ") -
               1))
                {
                    case "Glazed":
                        mRaisedGlazed = mRaisedGlazed - Int32.Parse(s.Substring(0,
                       s.IndexOf(" ")));
                        txtRaisedGlazed.Text = mRaisedGlazed.ToString();
                        break;
                    case "Sugar":
                        mRaisedSugar = mRaisedSugar - Int32.Parse(s.Substring(0,
                       s.IndexOf(" ")));
                        txtSugarRaised.Text = mRaisedSugar.ToString();
                        break;
                    case "Chocolate":
                        mFilledChocolate = mFilledChocolate - Int32.Parse(s.Substring(0,
                       s.IndexOf(" ")));
                        txtChocolateFilled.Text = mFilledChocolate.ToString();
                        break;
                    case "Lemon":
                        mFilledLemon = mFilledLemon - Int32.Parse(s.Substring(0,
                       s.IndexOf(" ")));
                        txtLemonFilled.Text = mFilledLemon.ToString();
                        break;
                    case "Vanilla":
                        mFilledVanilla = mFilledVanilla - Int32.Parse(s.Substring(0,
                       s.IndexOf(" ")));
                        txtVanillaFilled.Text = mFilledVanilla.ToString();
                        break;
                }
            }
        }

    }
}
