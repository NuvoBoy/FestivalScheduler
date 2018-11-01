using System;
using System.Threading.Tasks;
using System.Windows;
using FestivalScheduler.Client.Windows.Backend;
using FestivalScheduler.Data;

namespace FestivalScheduler.Client.Windows
{
    /// <summary>
    /// Interaktionslogik für AddChoirEntry.xaml
    /// </summary>
    public partial class AddChoirEntry
    {
        //puffer existing node
        private readonly ChoirNode _existing;

        /// <summary>
        /// ctor for new entry
        /// </summary>
        public AddChoirEntry()
        {
            InitializeComponent();

            Title = "Neuer Chor";
        }
        /// <summary>
        /// ctor for existing entry
        /// </summary>
        /// <param name="existing"></param>
        public AddChoirEntry(ChoirNode existing)
        {
            InitializeComponent();
            Title = "Chor bearbeiten";

            TextBoxName.Text = existing.Name;
            TextBoxShort.Text = existing.ShortCut;
            TextBoxNational.Text = existing.Nationality;
            TextBoxCount.Text = existing.MemberCount.ToString();
            TextBoxAccomud.Text = existing.AccommodationPlace;

            _existing = existing;
        }
        /// <summary>
        /// handel button close on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// handel button save and close on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveClose_OnClick(object sender, RoutedEventArgs e)
        {
            if (_existing != null)
            {
                _existing.Name = TextBoxName.Text;
                _existing.ShortCut = TextBoxShort.Text;
                _existing.Nationality = TextBoxNational.Text;
                _existing.MemberCount = Convert.ToInt32(TextBoxCount.Text);
                _existing.AccommodationPlace = TextBoxAccomud.Text;

                Task.Factory.StartNew(() => new ChoirManager().ChangeChoirNode(_existing));
            }
            else
            {
                var name = TextBoxName.Text;
                var shortCut = TextBoxShort.Text;
                var nationality = TextBoxNational.Text;
                var memberCount = Convert.ToInt32(TextBoxCount.Text);
                var accommodationPlace = TextBoxAccomud.Text;

                Task.Factory.StartNew(() => new ChoirManager().CreateChoirNode(name, shortCut, nationality, memberCount, accommodationPlace));
            }
            Close();
        }
    }
}
