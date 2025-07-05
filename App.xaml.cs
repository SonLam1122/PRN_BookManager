using System.Configuration;
using System.Data;
using System.Windows;

namespace BookManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new Data.BookStoreContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }

}
