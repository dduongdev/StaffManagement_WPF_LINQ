using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Linq;
using SM_WPF_LINQ.ViewModels;
using SM_WPF_LINQ.Utils;

namespace SM_WPF_LINQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StaffManagementDataContext smdc = new StaffManagementDataContext();
        private Table<DSNV> staffs;
        private Table<DMPHONG> departments;
        private Table<CHUCVU> staffRoles;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            staffs = smdc.GetTable<DSNV>();
            departments = smdc.GetTable<DMPHONG>();
            staffRoles = smdc.GetTable<CHUCVU>();

            //Default value
            rdMale.IsChecked = true;

            dgStaffs.SelectionChanged += DgStaffs_SelectionChanged;
            btnRefresh.Click += BtnRefresh_Click;
            btnSaveStaff.Click += BtnSaveStaff_Click;
            btnUpdateStaff.Click += BtnUpdateStaff_Click;
            btnDeleteStaff.Click += BtnDeleteStaff_Click;
            btnSearch.Click += BtnSearch_Click;
            btnStatistic.Click += BtnStatistic_Click;

            loadDepartments();
            loadStaffRoles();
            loadStaffs();
        }

        private void BtnStatistic_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow statisticWindow = new StatisticsWindow();
            statisticWindow.Show();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchTarget.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên cần tìm.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtSearchTarget.Focus();
                return;
            }

            dgStaffs.ItemsSource = (from s in staffs
                                    where s.HoTen.Contains(txtSearchTarget.Text.Trim())
                                    select new StaffView(s.MaNV, s.HoTen, s.NgaySinh, s.GioiTinh,
                                                         s.SoDT, s.HeSoLuong, s.MaPhong, s.MaChucVu)).ToList();
        }

        private void BtnDeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            if(dgStaffs.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xoá.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                dgStaffs.Focus();
                return;
            }

            StaffView selectedStaff = dgStaffs.SelectedItem as StaffView;
            staffs.DeleteOnSubmit(staffs.Single(s => s.MaNV == selectedStaff.MaNV));
            smdc.SubmitChanges();

            MessageBox.Show("Xoá nhân viên thành công.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnUpdateStaff_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStaffId.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffId.Focus();
                return;
            }

            if (!staffs.Any(s => s.MaNV.ToString() == txtStaffId.Text.Trim()))
            {
                MessageBox.Show("Mã nhân viên không tồn tại.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffId.Focus();
                return;
            }

            if (dpStaffBirthdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                dpStaffBirthdate.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtStaffPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffPhone.Focus();
                return;
            }

            if (!Validator.IsPhone(txtStaffPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffPhone.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtStaffCoefSalary.Text))
            {
                MessageBox.Show("Vui lòng nhập hệ số lương.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffCoefSalary.Focus();
                return;
            }

            if (!Validator.IsNumeric(txtStaffCoefSalary.Text))
            {
                MessageBox.Show("Hệ số lương không hợp lệ.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffCoefSalary.Focus();
                return;
            }

            if (cmbDepartments.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng ban.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                cmbDepartments.Focus();
                return;
            }

            if (cmbStaffRoles.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chức vụ.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                cmbStaffRoles.Focus();
                return;
            }

            DSNV selectedStaff = staffs.Single(s => s.MaNV.ToString() == txtStaffId.Text.Trim());
            selectedStaff.HoTen = txtStaffName.Text;
            selectedStaff.NgaySinh = dpStaffBirthdate.SelectedDate ?? DateTime.Now;
            selectedStaff.GioiTinh = rdMale.IsChecked == true;
            selectedStaff.SoDT = txtStaffPhone.Text;
            selectedStaff.HeSoLuong = double.Parse(txtStaffCoefSalary.Text.Replace('.', ','));
            selectedStaff.MaPhong = cmbDepartments.SelectedValue.ToString();
            selectedStaff.MaChucVu = cmbStaffRoles.SelectedValue.ToString();

            smdc.SubmitChanges();

            MessageBox.Show("Sửa nhân viên thành công.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnSaveStaff_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStaffId.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffId.Focus();
                return;
            }

            if (!Validator.IsNumericInteger(txtStaffId.Text))
            {
                MessageBox.Show("Mã nhân viên không hợp lệ.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffId.Focus();
                return;
            }

            if (staffs.Any(s => s.MaNV.ToString() == txtStaffId.Text.Trim()))
            {
                MessageBox.Show("Mã nhân viên đã tồn tại.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffId.Focus();
                return;
            }

            if (dpStaffBirthdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                dpStaffBirthdate.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtStaffPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffPhone.Focus();
                return;
            }

            if (!Validator.IsPhone(txtStaffPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffPhone.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtStaffCoefSalary.Text))
            {
                MessageBox.Show("Vui lòng nhập hệ số lương.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffCoefSalary.Focus();
                return;
            }

            if (!Validator.IsNumeric(txtStaffCoefSalary.Text))
            {
                MessageBox.Show("Hệ số lương không hợp lệ.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtStaffCoefSalary.Focus();
                return;
            }

            if (cmbDepartments.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng ban.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                cmbDepartments.Focus();
                return;
            }

            if (cmbStaffRoles.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chức vụ.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                cmbStaffRoles.Focus();
                return;
            }

            DSNV staff = new DSNV();
            staff.MaNV = int.Parse(txtStaffId.Text);
            staff.HoTen = txtStaffName.Text;
            staff.NgaySinh = dpStaffBirthdate.SelectedDate ?? DateTime.Now;
            staff.GioiTinh = rdMale.IsChecked == true;
            staff.SoDT = txtStaffPhone.Text;
            staff.HeSoLuong = double.Parse(txtStaffCoefSalary.Text.Replace('.', ','));
            staff.MaPhong = cmbDepartments.SelectedValue.ToString();
            staff.MaChucVu = cmbStaffRoles.SelectedValue.ToString();

            staffs.InsertOnSubmit(staff);
            smdc.SubmitChanges();

            MessageBox.Show("Thêm nhân viên thành công.", "Quản lý nhân viên", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadStaffs();

            var inputControls = ((Grid)grStaffInfo.Content).Children;
            foreach (var control in inputControls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }

                if (control is DatePicker)
                {
                    ((DatePicker)control).SelectedDate = null;
                }

                if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
            }

            rdMale.IsChecked = true;

            txtSearchTarget.Clear();

            txtStaffId.Focus();
        }

        private void DgStaffs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaffView selectedStaff = dgStaffs.SelectedItem as StaffView;
            if (selectedStaff != null)
            {
                txtStaffId.Text = selectedStaff.MaNV.ToString();
                txtStaffName.Text = selectedStaff.HoTen;
                dpStaffBirthdate.SelectedDate = selectedStaff.NgaySinh;
                if (selectedStaff.GioiTinh)
                    rdMale.IsChecked = true;
                else
                    rdFemale.IsChecked = true;

                txtStaffPhone.Text = selectedStaff.SoDT;
                txtStaffCoefSalary.Text = selectedStaff.HeSoLuong.ToString();
                cmbDepartments.SelectedValue = selectedStaff.MaPhong;
                cmbStaffRoles.SelectedValue = selectedStaff.MaChucVu;
            }
        }

        #region Load data to control
        private void loadStaffs()
        {
            dgStaffs.ItemsSource = (from s in staffs
                                    select new StaffView(s.MaNV, s.HoTen, s.NgaySinh, s.GioiTinh,
                                                         s.SoDT, s.HeSoLuong, s.MaPhong, s.MaChucVu)).
                                    ToList();
        }


        private void loadDepartments()
        {
            cmbDepartments.ItemsSource = (from d in departments
                                          select d).ToList();
            cmbDepartments.DisplayMemberPath = "TenPhong";
            cmbDepartments.SelectedValuePath = "MaPhong";
        }

        private void loadStaffRoles()
        {
            cmbStaffRoles.ItemsSource = (from sr in staffRoles
                                         select new
                                         {
                                             sr.MaChucVu,
                                             sr.TenChucVu
                                         }).ToList();
            cmbStaffRoles.DisplayMemberPath = "TenChucVu";
            cmbStaffRoles.SelectedValuePath = "MaChucVu";
        }
        #endregion
    }
}
