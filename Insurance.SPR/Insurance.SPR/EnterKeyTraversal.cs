using System.Windows;
using System.Windows.Input;
using DevExpress.XtraEditors;
using DevExpress.Utils;


namespace Insurance_SPR
{
    public class EnterKeyTraversal
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        static void ue_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var ue = e.OriginalSource as FrameworkElement;

            if (e.Key == Key.Enter)
            {
                string tag = "";


                if (ue.Name == "PART_Editor" && ue.DataContext is BaseEdit)
                    tag = (string)((BaseEdit)ue.DataContext).Tag;

                if ((ue.Tag != null && ue.Tag.ToString() == "IgnoreEnterKeyTraversal") || tag == "IgnoreEnterKeyTraversal")
                {
                    return;
                }

                e.Handled = true;
                ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                //e.Handled = true;
                //ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private static void ue_Unloaded(object sender, RoutedEventArgs e)
        {
            var ue = sender as FrameworkElement;
            if (ue == null) return;

            ue.Unloaded -= ue_Unloaded;
            ue.PreviewKeyUp -= ue_PreviewKeyDown;
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool),

            typeof(EnterKeyTraversal), new UIPropertyMetadata(false, IsEnabledChanged));

        static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ue = d as FrameworkElement;
            if (ue == null) return;

            if ((bool)e.NewValue)
            {
                ue.Unloaded += ue_Unloaded;
                ue.PreviewKeyUp += ue_PreviewKeyDown;
            }
            else
            {
                ue.PreviewKeyUp -= ue_PreviewKeyDown;
            }
        }
    }
}
