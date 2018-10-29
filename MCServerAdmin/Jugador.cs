using System.Collections.Generic;
using DataAccessLib;
using Microsoft.Data.Sqlite;

namespace MCServerAdmin.datos
{
	class Jugador
	{
		public string Nombre { get; set; }
		public string Skin { get; set; }

		public Jugador()
		{
			this.Nombre = "Jugador Autogenerado";
			Skin = "Assets/PorDefecto.Skin.png";
		}

		public Jugador(string Nombre)
		{
			this.Nombre = Nombre;
			Skin = "Assets/PorDefecto.Skin.png";
		}

		public Jugador(string Nombre, string Skin)
		{
			this.Nombre = Nombre;
			this.Skin = Skin;
		}

		public override string ToString()
		{
			return Nombre;
		}
	}

	class AdminJugs
	{
		private static List<Jugador> Lista;

		public static List<Jugador> GetJugadores()
		{
			if (Lista is null)
			{
				Lista = new List<Jugador>();
				using (SqliteConnection db = new SqliteConnection("Filename=" + MainPage.RUTA_DB))
				{
					db.Open();

					string tableCommand = "select * from Jugadores";

					SqliteCommand selectCommand = new SqliteCommand(tableCommand, db);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{
						Lista.Add(new Jugador(query.GetString(1), query.GetString(2)));
					}
					db.Close();
				}
			}
			
			return Lista;
		}

		public static void GuardarJugadores()
		{
			using (SqliteConnection db = new SqliteConnection("Filename=" + MainPage.RUTA_DB))
			{
				db.Open();

				string limpiar = "delete from Jugadores";

				SqliteCommand limpiarExe = new SqliteCommand(limpiar, db);

				limpiarExe.ExecuteReader();

				if (Lista.Count > 0)
				{
					string reescribir = "";
					int i = 0;
					foreach (Jugador jug in Lista)
					{
						reescribir += "insert into Jugadores values(" + i++ + ", '" + jug.Nombre + "', '" + jug.Skin + "'); ";
					}

					SqliteCommand reescribirExe = new SqliteCommand(reescribir, db);

					reescribirExe.ExecuteReader();
				}

				db.Close();
			}
		}
	}
}