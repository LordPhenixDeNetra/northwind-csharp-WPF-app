using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Logique d'interaction pour DataGridActionDialogue.xaml
    /// </summary>
    public partial class DataGridActionDialogue : Window
    {
        private AjouterCommande ajouterCommande;
        public DataGridActionDialogue()
        {
            InitializeComponent();

        }
      

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

            var selectedOrder = dGVDetailCommandes.SelectedItem as OrderData;

            if (selectedOrder != null)
            {
                // Afficher votre boîte de dialogue personnalisée
                DataGridActionDialogue dialog = new DataGridActionDialogue();

                // Affecter le gestionnaire d'événements de suppression au bouton "SUPPRIMER"
                dialog.btnSupprimer.Click += btnSupprimer_Click;

                //.DeleteButton.Click += DeleteButton_Click;

                // Afficher la boîte de dialogue
                dialog.ShowDialog();

                MessageBox.Show("Commande supprimer avec succee");
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnModifier_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
