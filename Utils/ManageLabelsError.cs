using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinanciaRed.Utils {
    internal class ManageLabelsError {
        public static bool ExistsLabelInStack (StackPanel containerStackPanel, string nameLabel) {
            if (containerStackPanel.Children.OfType<Label> ().FirstOrDefault (label => label.Name == nameLabel) == null) {
                return false;
            }
            return true;
        }

        public static Label CreateNewLabel (string name, string content, int fontSize, int leftMargin) {
            Label newLabel = new Label () {
                Name = name,
                Content = content,
                FontSize = fontSize,
                Foreground = new SolidColorBrush (Colors.Red),
                Margin = new System.Windows.Thickness (leftMargin, 0, 0, 0),
                Padding = new System.Windows.Thickness (0)
            };
            return newLabel;
        }

        public static void RemoveLabel (StackPanel containerStackPanel, string name) {
            containerStackPanel.Children.Remove (containerStackPanel.Children.OfType<Label> ().
                                                    FirstOrDefault (label => label.Name == name));
        }
    }
}
