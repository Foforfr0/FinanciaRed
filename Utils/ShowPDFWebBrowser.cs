using System;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.Utils {
    internal class ShowPDFWebBrowser {
        public static void ShowPDF (string NameFile, byte[] PdfFile, WebBrowser PdfViewer) {
            try {
                if (PdfFile != null && PdfFile.Length > 0) {
                    string tempFilePath = System.IO.Path.Combine (System.IO.Path.GetTempPath (), NameFile);

                    System.IO.File.WriteAllBytes (tempFilePath, PdfFile);

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
