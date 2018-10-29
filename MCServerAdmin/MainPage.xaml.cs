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
using Windows.UI.Xaml.Navigation;
using DataAccessLib;
using Microsoft.Data.Sqlite;
using MCServerAdmin.datos;

namespace MCServerAdmin
{

	public sealed partial class MainPage : Page
	{
		public static string RUTA_DB = "dbJugadores.db";

		public MainPage()
		{
			this.InitializeComponent();
			InitializeDatabase();
			//FramePrincipal.Navigate(typeof(Bienvenida));
			FramePrincipal.Navigate(typeof(VerJugadores));
		}

		public static void InitializeDatabase()
		{
			
			using (SqliteConnection db = new SqliteConnection("Filename="+RUTA_DB))
			{
				db.Open();

				String tableCommand = "CREATE TABLE IF NOT EXISTS Jugadores (" +
					"Id INTEGER PRIMARY KEY, " +
					"Nombre nvarchar(2048) NOT NULL, " +
					"Skin nvarchar(2048))";

				SqliteCommand createTable = new SqliteCommand(tableCommand, db);

				createTable.ExecuteReader();

				/*
				try
				{
					String limpiar = "delete from Jugadores";

					SqliteCommand exeLimpiar = new SqliteCommand(limpiar, db);
					exeLimpiar.ExecuteReader();

					String sujetosDePrueba = "INSERT INTO Jugadores VALUES(0, 'Cristichi', 'CapitanPrice')";

					SqliteCommand insertSujetos = new SqliteCommand(sujetosDePrueba, db);
					insertSujetos.ExecuteReader();
				}
				catch(Exception)
				{

				}*/

				db.Close();
			}
		}

		private void AppBarButton_Click_Back(object sender, RoutedEventArgs e)
		{
			try
			{
				FramePrincipal.GoBack();
			}
			catch(Exception)
			{
				BtnBack.IsEnabled = false;
			}
		}

		private void AppBarButton_Click_Forward(object sender, RoutedEventArgs e)
		{
			FramePrincipal.GoForward();
		}

		private void AppBarButton_Click_VerJugadores(object sender, RoutedEventArgs e)
		{
			FramePrincipal.Navigate(typeof(VerJugadores));
		}

		private void FramePrincipal_Navigated(object sender, NavigationEventArgs e)
		{
			BtnBack.IsEnabled = FramePrincipal.CanGoBack;
			BtnForward.IsEnabled = FramePrincipal.CanGoForward;

			BtnIrLista.IsEnabled = !e.SourcePageType.Equals(typeof(VerJugadores));
			BtnNuevoJugador.IsEnabled = !e.SourcePageType.Equals(typeof(RegistrarNuevo));
		}

		private void AppBarButton_Click_NuevoJugador(object sender, RoutedEventArgs e)
		{
			FramePrincipal.Navigate(typeof(RegistrarNuevo));
		}
	}
}
