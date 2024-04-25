using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace App.Views
{
    public sealed partial class Person : UserControl
    {
        public Person()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}