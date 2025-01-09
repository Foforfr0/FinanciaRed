using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FinanciaRed.Utils {
    public class ShowCsvDatagrid {
        public static DataTable LoadCsvData (byte[] csvBytes) {
            DataTable dt = new DataTable ();

            // Convertir bytes a string
            string csvContent;
            using (MemoryStream ms = new MemoryStream (csvBytes))
            using (StreamReader reader = new StreamReader (ms)) {
                csvContent = reader.ReadToEnd ();
            }

            // Separar las líneas
            string[] lines = csvContent.Split (new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return dt;

            // Procesar encabezados
            string[] headers = lines[0].Split (',');
            foreach (string header in headers) {
                dt.Columns.Add (header.Trim ());
            }

            // Procesar datos
            for (int i = 1; i < lines.Length; i++) {
                string[] fields = lines[i].Split (',');
                DataRow row = dt.NewRow ();

                for (int j = 0; j < headers.Length && j < fields.Length; j++) {
                    row[j] = fields[j].Trim ();
                }

                dt.Rows.Add (row);
            }

            return dt;
        }

        public static void LoadCsvData (byte[] csvBytes, DataGrid dataGrid) {
            try {
                // Convertir bytes a string usando encoding UTF8
                string csvContent;
                using (MemoryStream ms = new MemoryStream (csvBytes))
                using (StreamReader reader = new StreamReader (ms, Encoding.UTF8)) {
                    csvContent = reader.ReadToEnd ();
                }

                // Crear una lista para almacenar los datos
                List<Dictionary<string, string>> data = new List<Dictionary<string, string>> ();

                // Separar las líneas y remover líneas vacías
                string[] lines = csvContent.Split (new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length == 0) {
                    MessageBox.Show (
                        "Archivo CSV vacío o nulo.",
                        "Sin datos.",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener los headers
                string[] headers = lines[0].Split (',');

                // Procesar cada línea
                for (int i = 1; i < lines.Length; i++) {
                    Dictionary<string, string> row = new Dictionary<string, string> ();
                    string[] values = lines[i].Split (',');

                    for (int j = 0; j < headers.Length && j < values.Length; j++) {
                        row[headers[j].Trim ()] = values[j].Trim ();
                    }

                    data.Add (row);
                }

                // Asignar los datos al DataGrid
                dataGrid.ItemsSource = data;

                // Crear las columnas explícitamente
                dataGrid.Columns.Clear ();
                foreach (string header in headers) {
                    dataGrid.Columns.Add (new DataGridTextColumn {
                        Header = header.Trim (),
                        Binding = new Binding ($"[{header.Trim ()}]")
                    });
                }
                dataGrid.Columns[0].Width = 220;
                dataGrid.Columns[1].Width = 220;
                dataGrid.Columns[2].Width = 220;
            } catch (Exception ex) {
                MessageBox.Show (
                    $"Error al cargar el CSV: {ex.Message}", 
                    "Error inesperado.", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
