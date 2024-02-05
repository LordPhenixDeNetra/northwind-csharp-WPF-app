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
using System.Windows.Shapes;

namespace ProjetWpf2023
{
    /// <summary>
    /// Logique d'interaction pour AjouterCommande.xaml
    /// </summary>
    public partial class AjouterCommande : Window
    {
        NorthwindContext northwindContext = new NorthwindContext();
        private List<Customers> listeClients;
        private List<Products> listeProduits;
        private OrderData selectedOrder;

        // Définition de l'événement pour signaler l'ajout de données
        public event EventHandler DataAdded;

        public AjouterCommande()
        {
            InitializeComponent();
            listeClients = new List<Customers>();
            listeProduits = new List<Products>();
            LoadCustomerIDs();
            LoadProducts();
        }
        // Propriété pour obtenir les modifications apportées
        public OrderData ModifiedOrder { get; private set; }
        public AjouterCommande(OrderData order)
        {
            InitializeComponent();
            selectedOrder = order;

            // Pré-remplir des champs avec les données de l'OrderData
            //txtBox1.Text = order.Prop1;
            //comboBox.SelectedValue = order.Prop2;

            // Autres initialisations ou traitements nécessaires
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            /*
            // Vérifiez si un client et un produit sont sélectionnés
            if (ComboBoxCustomers.SelectedItem != null && ComboBoxProducts.SelectedItem != null)
            {
                // Récupérez le client sélectionné
                Customers selectedCustomer = ComboBoxCustomers.ItemsSource as Customers;

                // Récupérez le produit sélectionné
                Products selectedProduct = ComboBoxProducts.SelectedItem as Products;

                // Vérifiez si la quantité est valide (entier positif)
                int quantity;
                if (int.TryParse(TextBoxQuantity.Text, out quantity) && quantity > 0)
                {
                    // Créez une nouvelle commande
                    Orders newOrder = new Orders
                    {
                        CustomerID = selectedCustomer.CustomerID,
                        OrderDate = DateTime.Now
                    };

                    // Ajoutez la nouvelle commande au contexte de base de données
                    northwindContext.Orders.Add(newOrder);
                    northwindContext.SaveChanges();

                    // Récupérez l'ID de la commande générée
                    int orderID = newOrder.OrderID;

                    // Créez une nouvelle ligne de détail de commande
                    OrderDetails newOrderDetail = new OrderDetails
                    {
                        OrderID = orderID,
                        ProductID = selectedProduct.ProductID,
                        UnitPrice = (decimal)selectedProduct.UnitPrice,
                        Quantity = (short)quantity
                    };

                    // Ajoutez la nouvelle ligne de détail de commande au contexte de base de données
                    northwindContext.OrderDetails.Add(newOrderDetail);
                    northwindContext.SaveChanges();

                    MessageBox.Show("La commande a été enregistrée avec succès!");
                }
                else
                {
                    MessageBox.Show("Veuillez entrer une quantité valide (entier positif).");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client et un produit.");
            }
            */
            if (ComboBoxCustomers.SelectedItem is Customers selectedCustomer && ComboBoxProducts.SelectedItem is Products selectedProduct && int.TryParse(TextBoxQuantity.Text, out int quantity))
            {
                using (var context = new NorthwindContext())
                {
                    // Créer une nouvelle commande
                    Orders newOrder = new Orders
                    {
                        CustomerID = selectedCustomer.CustomerID,
                        OrderDate = DateTime.Now
                    };

                    // Ajouter la nouvelle commande au contexte de base de données
                    context.Orders.Add(newOrder);

                    // Créer une nouvelle ligne de détail de commande
                    OrderDetails newOrderDetail = new OrderDetails
                    {
                        Orders = newOrder,
                        ProductID = selectedProduct.ProductID,
                        UnitPrice = (decimal)selectedProduct.UnitPrice,
                        Quantity = (short)quantity
                    };

                    // Ajouter la nouvelle ligne de détail de commande au contexte de base de données
                    context.OrderDetails.Add(newOrderDetail);

                    // Enregistrer les modifications dans la base de données
                    context.SaveChanges();

                    // Lever l'événement DataAdded
                    OnDataAdded(EventArgs.Empty);

                    MessageBox.Show("La commande a été enregistrée avec succès!");
                }
            }
            else
            {
                MessageBox.Show("Pas d'insertion");
            }
        }
        private void LoadCustomerIDs()
        {
            /*
             * List<string> customerIDs = northwindContext.Customers.Select(c => c.CustomerID).ToList();
            ComboBoxCustomers.ItemsSource = customerIDs;

            */

            using (var context = new NorthwindContext())
            {
                listeClients = context.Customers.ToList();
                ComboBoxCustomers.ItemsSource = listeClients;
                ComboBoxCustomers.DisplayMemberPath = "CustomerID";
            }
        }

        private void LoadProducts()
        {
            /*  List<Products> products = northwindContext.Products.ToList();
              ComboBoxProducts.ItemsSource = products;
              ComboBoxProducts.DisplayMemberPath = "ProductName";
              ComboBoxProducts.SelectedValuePath = "UnitPrice";
            */


            using (var context = new NorthwindContext())
            {
                listeProduits = context.Products.ToList();
                ComboBoxProducts.ItemsSource = listeProduits;
                ComboBoxProducts.DisplayMemberPath = "ProductName";
            }


        }

        private void ComboBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* // Vérifiez si un produit est sélectionné
             if (ComboBoxProducts.SelectedItem != null)
             {
                 // Récupérez l'objet du produit sélectionné dans le ComboBox
                 Products selectedProduct = ComboBoxProducts.SelectedItem as Products;

                 // Vérifiez si la quantité est un nombre valide
                 if (int.TryParse(TextBoxQuantity.Text, out int quantity))
                 {
                     // Calculez le montant total en multipliant la quantité par le prix unitaire
                     decimal? totalPrice = selectedProduct.UnitPrice * quantity;

                     // Mettez à jour le contenu du textBoxTotal avec le montant total calculé
                     TextBoxTotal.Text = totalPrice.ToString();
                 }
             }*/
            if (ComboBoxProducts.SelectedItem is Products selectedProduct)
            {
                TextBoxUnitPrice.Text = selectedProduct.UnitPrice.ToString() ;
                CalculateTotal();
            }
        }

        private void TextBoxQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*
            // Vérifiez si un produit est sélectionné dans le ComboBox
            if (ComboBoxProducts.SelectedItem != null)
            {
                // Récupérez l'objet du produit sélectionné dans le ComboBox
                Products selectedProduct = ComboBoxProducts.SelectedItem as Products;

                // Vérifiez si la quantité est un nombre valide
                if (int.TryParse(TextBoxQuantity.Text, out int quantity))
                {
                    // Calculez le montant total en multipliant la quantité par le prix unitaire
                    decimal? totalPrice = selectedProduct.UnitPrice * quantity;

                    // Mettez à jour le contenu du textBoxTotal avec le montant total calculé
                    TextBoxTotal.Text = totalPrice.ToString();
                }
            }
            */
            CalculateTotal();
        }

        private void ComboBoxCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Vérifier si un client est sélectionné
            if (ComboBoxCustomers.SelectedItem != null)
            {
                // Récupérer le client sélectionné
                Customers selectedCustomer = ComboBoxCustomers.SelectedItem as Customers;

                // Utiliser la valeur de selectedCustomer.CustomerID
                // ou effectuer d'autres opérations en fonction du client sélectionné
            }
        }
        private void CalculateTotal()
        {
            if (int.TryParse(TextBoxQuantity.Text, out int quantity) && ComboBoxProducts.SelectedItem is Products selectedProduct)
            {
                decimal unitPrice = (decimal)selectedProduct.UnitPrice;
                decimal total = quantity * unitPrice;
                TextBoxTotal.Text = total.ToString();
            }
        }


        // Méthode pour lever l'événement DataAdded
        protected virtual void OnDataAdded(EventArgs e)
        {
            DataAdded?.Invoke(this, e);
        }





    }
}
