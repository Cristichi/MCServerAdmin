using MCServerAdmin.datos;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace MCServerAdmin
{
	/// <summary>
	/// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
	/// </summary>
	public sealed partial class RegistrarNuevo : Page
	{
		ObservableCollection<String> cbItems = new ObservableCollection<String>();
		ObservableCollection<Rango> cbRangos = new ObservableCollection<Rango>();
		public RegistrarNuevo()
		{
			this.InitializeComponent();
			cbItems.Add("Alex");
			cbItems.Add("Steve");
			cbItems.Add("Capitan Price");

			cbRangos.Add(Rango.USUARIO);
			cbRangos.Add(Rango.VIP);
			cbRangos.Add(Rango.YOUTUBER);
			cbRangos.Add(Rango.ADMIN);
			cbRangos.Add(Rango.OWNER);
		}

		private void Button_Click_Registrar(object sender, RoutedEventArgs e)
		{
			if (CBSkin.SelectedIndex<0)
			{
				CBSkin.PlaceholderText = "Elige una Skin";
			}
			else if (CBRango.SelectedIndex<0)
			{
				CBSkin.PlaceholderText = "Elige un Rango";
			}
			else
			{
				Jugador Nuevo = new Jugador(TxtNombre.Text, "Assets/" + CBSkin.SelectedItem.ToString().Replace(" ", "") + ".Skin.png", (Rango)(CBRango.SelectedItem));
				AdminJugs.GetJugadores().Add(Nuevo);
				AdminJugs.GuardarJugadores();
				Frame.Navigate(typeof(NuevoJugRegistrado), Nuevo.Nombre);
				Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
			}
		}

		private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack)
			{
				Frame.GoBack();
			}
			else
			{
				Frame.Navigate(typeof(VerJugadores));
			}
		}
	}
}
