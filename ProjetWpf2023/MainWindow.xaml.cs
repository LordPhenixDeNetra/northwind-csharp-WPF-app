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


namespace ProjetWpf2023
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NorthwindContext northwindContext = new NorthwindContext();
        

        public MainWindow()
        {
            InitializeComponent();

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        public void LoadProductGlobal()
        {
            // Récupérer les 50 dernières commandes
            var last50Orders = northwindContext.Orders
                .OrderByDescending(order => order.OrderDate)
                .Take(50)
                .Join(northwindContext.OrderDetails,
                    order => order.OrderID,
                    orderDetail => orderDetail.OrderID,
                    (order, orderDetail) => new OrderData
                    {
                       
                        OrderID = order.OrderID,
                        CustomerID = order.CustomerID,
                        ProductID = orderDetail.ProductID,
                        UnitPrice = orderDetail.UnitPrice,
                        Quantity = orderDetail.Quantity,
                        OrderDate = (DateTime)order.OrderDate

                        /*
                        order.OrderID,
                        order.CustomerID,
                        orderDetail.ProductID,
                        orderDetail.UnitPrice,
                        orderDetail.Quantity,
                        order.OrderDate
                        */

                    }).ToList();

            // Définir la source de données pour le DataGrid
            dGVDetailCommandes.ItemsSource = last50Orders;
        }

        public void FormatDataGrid(/*object DeleteCommandExecute*/)
        {
            dGVDetailCommandes.Columns.Clear(); // Supprime toutes les colonnes existantes (au cas où)

            // Définir les colonnes manuellement
            dGVDetailCommandes.Columns.Add(new DataGridTextColumn
            {
                Header = "OrderID",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Binding("OrderID")
            });

            dGVDetailCommandes.Columns.Add(new DataGridTextColumn
            {
                Header = "CustomerID",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Binding("CustomerID")
            });

            dGVDetailCommandes.Columns.Add(new DataGridTextColumn
            {
                Header = "ProductID",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Binding("ProductID")
            });

            dGVDetailCommandes.Columns.Add(new DataGridTextColumn
            {
                Header = "UnitPrice",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Binding("UnitPrice")
            });

            dGVDetailCommandes.Columns.Add(new DataGridTextColumn
            {
                Header = "Quantity",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Binding("Quantity")
            });

            dGVDetailCommandes.Columns.Add(new DataGridTextColumn
            {
                Header = "OrderDate",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Binding("OrderDate")
            });


            /*
            // Ajouter la colonne d'actions (Supprimer et Modifier)
            var actionsColumn = new DataGridTemplateColumn
            {
                Header = "Actions",
                Width = new DataGridLength(1, DataGridLengthUnitType.Auto)
            };
            
            
            var actionsCellTemplate = new DataTemplate();
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var modifyButtonFactory = new FrameworkElementFactory(typeof(Button));
            modifyButtonFactory.SetValue(Button.ContentProperty, "Modifier");
            modifyButtonFactory.SetValue(Button.CommandProperty, new RelayCommand(ModifyCommandExecute));
            modifyButtonFactory.SetBinding(Button.CommandParameterProperty, new Binding());
            stackPanelFactory.AppendChild(modifyButtonFactory);

            var deleteButtonFactory = new FrameworkElementFactory(typeof(Button));
            deleteButtonFactory.SetValue(Button.ContentProperty, "Supprimer");
            deleteButtonFactory.SetValue(Button.CommandProperty, new RelayCommand(DeleteCommandExecute));
            deleteButtonFactory.SetBinding(Button.CommandParameterProperty, new Binding());
            stackPanelFactory.AppendChild(deleteButtonFactory);

            actionsCellTemplate.VisualTree = stackPanelFactory;
            actionsColumn.CellTemplate = actionsCellTemplate;

            dGVDetailCommandes.Columns.Add(actionsColumn);
            */
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var orders = northwindContext.Orders.ToList();
            //dGVDetailCommandes.ItemsSource = orders;

            /*
            dGVDetailCommandes.ItemsSource = northwindContext.Orders.Select(orders => new {
                OrderID = orders.OrderID,
                CustomerID = orders.Customers.CustomerID,
                ProduitID = orders.OrderDetails_.ProductID,
                UnitPrice = orders.OrderDetails_.UnitPrice,
                Quantity = orders.OrderDetails_.Quantity,
                OrderDate = orders.OrderDate
            }).ToList();
            */

            /*
            dGVDetailCommandes.ItemsSource = northwindContext.Orders.Join(
            northwindContext.OrderDetails,
            order => order.OrderID,
            orderDetail => orderDetail.OrderID,
            (order, orderDetail) => new
            {
                order.OrderID,
                order.CustomerID,
                orderDetail.ProductID,
                orderDetail.UnitPrice,
                orderDetail.Quantity,
                order.OrderDate
            }).ToList();
            */

            FormatDataGrid();

            // Récupérer les 50 dernières commandes
            LoadProductGlobal();

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AjouterCommande ajouterCommande = new AjouterCommande();

            ajouterCommande.ShowDialog();
        }

        private void btnRechercher_Click(object sender, RoutedEventArgs e)
        {
            string searchText = textBoxRechercherProduit.Text;

            // Effectuer la recherche en utilisant le texte saisi (searchText)
            var searchResults = northwindContext.Orders
        .Where(order => order.CustomerID.Contains(searchText) ||
                        order.OrderID.ToString().Contains(searchText) ||
                        order.OrderDetails.Any(orderDetail => orderDetail.Products.ProductName.Contains(searchText)))
        .Join(northwindContext.OrderDetails,
            order => order.OrderID,
            orderDetail => orderDetail.OrderID,
            (order, orderDetail) => new
            {
                order.OrderID,
                order.CustomerID,
                orderDetail.ProductID,
                orderDetail.UnitPrice,
                orderDetail.Quantity,
                order.OrderDate
            })
        .ToList();

            // Définir la source de données pour le DataGrid avec les résultats de la recherche
            dGVDetailCommandes.ItemsSource = searchResults;
        }

        private void MenuItemAccuil_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les 50 dernières commandes
            LoadProductGlobal();
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la ligne sélectionnée du DataGrid
            var selectedOrder = dGVDetailCommandes.SelectedItem as OrderData;



            if (selectedOrder != null)
            {
                // Supprimer la ligne de la base de données
                int orderID = selectedOrder.OrderID;

                // Supprimer la ligne de la table "OrderDetails"
                var orderDetails = northwindContext.OrderDetails.Where(od => od.OrderID == orderID);
                northwindContext.OrderDetails.RemoveRange(orderDetails);

                // Supprimer la ligne de la table "Orders"
                var order = northwindContext.Orders.FirstOrDefault(o => o.OrderID == orderID);
                if (order != null)
                {
                    northwindContext.Orders.Remove(order);
                }

                // Enregistrer les modifications dans la base de données
                northwindContext.SaveChanges();

                // Mettre à jour le DataGrid en récupérant à nouveau les données

            }
        }


        private void dGVDetailCommandes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            /*
            if (sender is DataGrid dataGrid && dataGrid.SelectedItems.Count == 1)
            {
                // Créez une instance de votre boîte de dialogue personnalisée
                var dataGridActionDialogue = new DataGridActionDialogue();

                // Affichez la boîte de dialogue modale
                dataGridActionDialogue.ShowDialog();

                // Vous pouvez également obtenir des valeurs ou des résultats de la boîte de dialogue après sa fermeture
                // par exemple, en interrogeant les propriétés ou en appelant des méthodes de la boîte de dialogue
            }*/

            // Gestionnaire d'événements pour le double-clic sur une ligne du DataGrid

            var selectedOrder = dGVDetailCommandes.SelectedItem as OrderData;

            if (selectedOrder != null)
            {
                // Afficher votre boîte de dialogue personnalisée
                DataGridActionDialogue dialog = new DataGridActionDialogue();

                // Affecter le gestionnaire d'événements de suppression au bouton "SUPPRIMER"
                dialog.btnSupprimer.Click += btnSupprimer_Click;

                // Afficher la boîte de dialogue
                dialog.ShowDialog();

                MessageBox.Show("Commande supprimer avec succee");
            }
        }
    }
}

