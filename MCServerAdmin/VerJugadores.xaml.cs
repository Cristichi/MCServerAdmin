using MCServerAdmin.datos;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MCServerAdmin
{

	public sealed partial class VerJugadores : Page
	{
		List<Jugador> ListaJugadores;

		public VerJugadores()
		{
			this.InitializeComponent();
			ListaJugadores = AdminJugs.GetJugadores();
		}

		private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count>0)
			{
				Jugador jug = (Jugador) e.AddedItems.First();
				SPHerramientasJug.Visibility = Visibility.Visible;
				string nombre = jug.Nombre;
				if (nombre.Length>30)
				{
					nombre = nombre.Substring(0,27)+"...";
				}
				TxtNombre.Text = "Jugador: "+nombre;
				TxtRango.Text = "Rango: "+jug.Rango.ToString();
				TxtFecha.Text = "Se unió el día " + jug.FechaIngreso.Day + "/" + jug.FechaIngreso.Month + "/" + jug.FechaIngreso.Year;
                if (jug.Rango.Equals(Rango.ADMIN) || jug.Rango.Equals(Rango.OWNER))
                {
                    TxtDinero.Text = "Tiene acceso al modo creativo.";
                }
                else if (jug.DineroVirtual == 0)
                {
                    TxtDinero.Text = "No tiene diamantes.";
                }
                else if (jug.DineroVirtual == 1)
                {
                    TxtDinero.Text = "Tiene 1 diamante en su cuenta.";
                }
                else if (jug.DineroVirtual > 0)
                {
                    TxtDinero.Text = "Tiene " + jug.DineroVirtual + " diamantes en su cuenta.";
                }
                else if (jug.DineroVirtual == -1)
                {
                    TxtDinero.Text = "Tiene que pagar 1 diamante.";
                }
                else
                {
                    TxtDinero.Text = "Tiene que pagar " + -jug.DineroVirtual + " diamantes.";
                }
                ImgImagen.Source = new BitmapImage(new Uri("ms-appx:///"+jug.Skin));
			}
			else
			{
				SPHerramientasJug.Visibility = Visibility.Collapsed;
			}
		}

		private void BtnBorrarJug_Click(object sender, RoutedEventArgs e)
		{
			AdminJugs.GetJugadores().RemoveAt(GVVerJugs.SelectedIndex);
			AdminJugs.GuardarJugadores();
			GVVerJugs.ItemsSource = AdminJugs.GetJugadores();
			Frame.Navigate(typeof(VerJugadores));
			Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
		}

		private void BtnEditarJug_Editar(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(RegistrarNuevo), GVVerJugs.SelectedIndex);
			Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
		}

		//AutosuggestBox
		private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			// Only get results when it was a user typing, 
			// otherwise assume the value got filled in by TextMemberPath 
			// or the handler for SuggestionChosen.
			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				//Set the ItemsSource to be your filtered dataset
				List<Jugador> Busca = new List<Jugador>();
				foreach (Jugador Candy in AdminJugs.GetJugadores())
				{
					if (Candy.Nombre.Contains(ASBBuscar.Text))
					{
						Busca.Add(Candy);
					}
				}
				sender.ItemsSource = Busca;
			}
		}

		private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			// Set sender.Text. You can use args.SelectedItem to build your text string.
			sender.Text = args.SelectedItem.ToString();
		}

		private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			if (args.ChosenSuggestion != null)
			{
				// User selected an item from the suggestion list, take an action on it here.
			}
			else
			{
				// Use args.QueryText to determine what to do.
			}
		}

		private void Grid_Loaded(object sender, RoutedEventArgs e)
		{
			SolidColorBrush borde = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
			Grid grid = (Grid)sender;
			foreach (UIElement el in grid.Children)
			{
				if (el is TextBlock txt)
				{
					if (txt.Name.Equals("TxtCadaRango"))
					{
						switch (txt.Text)
						{
							case "USUARIO":
								borde = new SolidColorBrush(Windows.UI.Color.FromArgb(200, 200, 200, 200));
								break;
							case "VIP":
								borde = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 155, 130, 0));
								break;
							case "YOUTUBER":
								borde = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 155, 0, 0));
								break;
							case "ADMIN":
								borde = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
								break;
							case "OWNER":
								borde = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 0));
								break;
						}
					}
					grid.Background = borde;
				}
			}

		}
	}
}
