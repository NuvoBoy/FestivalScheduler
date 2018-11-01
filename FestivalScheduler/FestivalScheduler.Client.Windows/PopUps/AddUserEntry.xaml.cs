using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FestivalScheduler.Client.Windows.Backend;
using FestivalScheduler.Client.Windows.SharedData;
using FestivalScheduler.Data;

namespace FestivalScheduler.Client.Windows
{
    /// <summary>
    /// Interaktionslogik für AddUserEntry.xaml
    /// </summary>
    public partial class AddUserEntry
    {
        private readonly DriverNode _driverToModify;
        private readonly AdvisorNode _advisorToModify;
        /// <summary>
        /// ctor
        /// </summary>
        public AddUserEntry()
        {
            InitializeComponent();
            ComboBoxChoir.ItemsSource = ClientDataContext.GetInstance().ViewChoirs;
        }
        /// <summary>
        /// ctor for existing driver
        /// </summary>
        /// <param name="driver"></param>
        public AddUserEntry(DriverNode driver)
        {
            _driverToModify = driver;
            InitializeComponent();

            TextBoxUserName.Text = _driverToModify.UserName;
            TextBoxUserName.IsEnabled = false;
            TextBoxPw.Text = _driverToModify.Password;
            TextBoxName.Text = _driverToModify.Name;
            TextBoxPhone.Text = _driverToModify.PhoneNumber;
            ComboBoxPermission.SelectedValue = _driverToModify.PermissionLevel;
            ComboBoxDriver.SelectedValue = _driverToModify.Membership;

            CheckBoxDriver.IsChecked = true;
            
        }
        /// <summary>
        /// ctor for existing advisor
        /// </summary>
        /// <param name="advisor"></param>
        public AddUserEntry(AdvisorNode advisor)
        {
            _advisorToModify = advisor;
            InitializeComponent();
            ComboBoxChoir.ItemsSource = ClientDataContext.GetInstance().ViewChoirs;

            TextBoxUserName.Text = _advisorToModify.UserName;
            TextBoxPw.Text = _advisorToModify.Password;
            TextBoxName.Text = _advisorToModify.Name;
            TextBoxPhone.Text = _advisorToModify.PhoneNumber;
            ComboBoxChoir.SelectedValue = ClientDataContext.GetInstance()
                .ViewChoirs.FirstOrDefault(x => x.NodeId.Equals(_advisorToModify.ChoirId));
            ComboBoxPermission.SelectedValue = _advisorToModify.PermissionLevel;

            CheckBoxAdvisor.IsChecked = true;
        }
        /// <summary>
        /// hanlde checkbox advisior on check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxAdvisor_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckBoxDriver.IsChecked = false;
            ComboBoxDriver.IsEnabled = false;
            ComboBoxChoir.IsEnabled = true;
        }
        /// <summary>
        /// handle checkbox driver on check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDriver_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckBoxAdvisor.IsChecked = false;
            ComboBoxDriver.IsEnabled = true;
            ComboBoxChoir.IsEnabled = false;
        }
        /// <summary>
        /// handle button close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// handle button save & close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveClose_OnClick(object sender, RoutedEventArgs e)
        {
            if(_advisorToModify != null) ChangeAdvisor();
            else if(_driverToModify != null) ChangeDriver();
            else if (CheckBoxAdvisor.IsChecked != null && (bool) CheckBoxAdvisor.IsChecked)
            { if (!CreateAdvisor()) return; }
            else { if (!CreateDriver()) return; }
            Close();
        }
        /// <summary>
        /// create new driver
        /// </summary>
        /// <returns></returns>
        private bool CreateDriver()
        {
            var username = TextBoxUserName.Text;
            var name = TextBoxName.Text;
            var pw = TextBoxPw.Text;
            var phone = TextBoxPhone.Text;
            if (ComboBoxPermission.SelectedValue == null) return false;
            var permission = (PermissionGroups)ComboBoxPermission.SelectedValue;
            if (ComboBoxDriver.SelectedValue == null) return false;
            var membership = (DriverMembership)ComboBoxDriver.SelectedValue;

            if (username.Equals("") || name.Equals("") || pw.Equals("")) return false;

            Task.Factory.StartNew(() => new DriverManager().CreateDriverNode(username, pw, name, permission, membership, phone));
            return true;
        }
        /// <summary>
        /// change existing driver
        /// </summary>
        private void ChangeDriver()
        {
            if(_driverToModify == null) return;

            _driverToModify.Membership = (DriverMembership)ComboBoxDriver.SelectedValue;
            _driverToModify.PermissionLevel = (PermissionGroups)ComboBoxPermission.SelectedValue;
            _driverToModify.Name = TextBoxName.Text;
            _driverToModify.Password = TextBoxPw.Text;
            _driverToModify.PhoneNumber = TextBoxPhone.Text;

            Task.Factory.StartNew(() => new DriverManager().ChangeDriverNode(_driverToModify));
            Close();
        }
        /// <summary>
        /// create new advisor
        /// </summary>
        /// <returns></returns>
        private bool CreateAdvisor()
        {
            var username = TextBoxUserName.Text;
            var name = TextBoxName.Text;
            var pw = TextBoxPw.Text;
            var phone = TextBoxPhone.Text;
            if (ComboBoxPermission.SelectedValue == null) return false;
            var permission = (PermissionGroups)ComboBoxPermission.SelectedValue;

            if (ComboBoxChoir.SelectedValue == null) return false;
            var choir = ((ChoirNode)ComboBoxChoir.SelectedValue).Name;

            if (username.Equals("") || name.Equals("") || pw.Equals("")) return false;

            Task.Factory.StartNew(() => new AdvisorManager().CreateAdvisorNode(username, pw, name, permission, choir, phone));
            return true;
        }
        /// <summary>
        /// update an existing object
        /// </summary>
        private void ChangeAdvisor()
        {
            if (_advisorToModify == null) return;

            _advisorToModify.PermissionLevel = (PermissionGroups)ComboBoxPermission.SelectedValue;
            _advisorToModify.ChoirId = ((ChoirNode)ComboBoxChoir.SelectedValue).Name;
            _advisorToModify.Name = TextBoxName.Text;
            _advisorToModify.Password = TextBoxPw.Text;
            _advisorToModify.PhoneNumber = TextBoxPhone.Text;

            Task.Factory.StartNew(() => new AdvisorManager().ChangeAdvisorNode(_advisorToModify));
            Close();
        }
    }
}
