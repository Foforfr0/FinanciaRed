using System;
using System.IO;
using System.Windows.Controls;

namespace FinanciaRed.Utils {
    internal class ShowPDFWebBrowser {
        public static string ShowPDF (string nameFile, byte[] pdfFile, WebBrowser pdfViewer) {
            string status = "";
            try {
                if (pdfFile != null && pdfFile.Length > 0) {
                    string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string tempDirectory = Path.Combine (projectDirectory, "TempFiles");

                    if (!Directory.Exists (tempDirectory)) {
                        Directory.CreateDirectory (tempDirectory);
                    }

                    string tempFilePath = Path.Combine (tempDirectory, $"{Guid.NewGuid ()}_{nameFile}");

                    File.WriteAllBytes (tempFilePath, pdfFile);

                    pdfViewer.Navigate (new Uri (tempFilePath));

                    pdfViewer.Navigated += (sender, e) => {
                        try {
                            // Eliminar archivos anteriores relacionados con este visor
                            foreach (var file in Directory.GetFiles (tempDirectory, $"*_{nameFile}")) {
                                if (file != tempFilePath) // No eliminar el archivo actual
                                {
                                    File.Delete (file);
                                }
                            }
                        } catch (Exception) {
                            status = "Fallo al eliminar cache de visualizador.";
                        }
                    };
                } else {
                    status = "El archivo PDF está vacío o no disponible.";
                }
            } catch (Exception ex) {
                status = $"Error al mostrar el PDF: {ex.Message}";
            }
            return status;
        }

    }
}
