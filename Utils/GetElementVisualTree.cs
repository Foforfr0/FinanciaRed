﻿using System.Windows;
using System.Windows.Media;

namespace FinanciaRed.Utils {
    public class GetElementVisualTree {
        public static T GetParent<T> (DependencyObject child) where T : DependencyObject {
            DependencyObject parent = VisualTreeHelper.GetParent (child);
            while (parent != null) {
                if (parent is T targetParent) {
                    return targetParent; // Retorna el primer padre del tipo T encontrado
                }
                parent = VisualTreeHelper.GetParent (parent);
            }
            return null; // Retorna null si no se encuentra un padre del tipo T en la jerarquía
        }

    }
}
