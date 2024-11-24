using System;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.Utils {
    internal class ShowPDFWebBrowser {
        public static void ShowPDF (string NameFile, byte[] PdfFile, WebBrowser PdfViewer) {
            try {
                // Verifica si el atributo tiene contenido
                if (PdfFile != null && PdfFile.Length > 0) {
                    // Ruta temporal para guardar el PDF
                    string tempFilePath = System.IO.Path.Combine (System.IO.Path.GetTempPath (), NameFile);

                    // Escribir el contenido del byte[] en el archivo temporal
                    System.IO.File.WriteAllBytes (tempFilePath, PdfFile);

                    // Cargar el archivo en el WebBrowser
                    PdfViewer.Navigate (new Uri (tempFilePath));
                } else {
                    MessageBox.Show ("El archivo PDF está vacío o no disponible.");
                }
            } catch (Exception ex) {
                MessageBox.Show ($"Error al mostrar el PDF: {ex.Message}");
            }
        }
    }
}
