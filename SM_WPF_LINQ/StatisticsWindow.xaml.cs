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
using System.Windows.Shapes;
using System.Data.Linq;

namespace SM_WPF_LINQ
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        private readonly StaffManagementDataContext smdc = new StaffManagementDataContext();
        private Table<DSNV> staffs;
        private Table<DMPHONG> departments;

        public StatisticsWindow()
        {
            InitializeComponent();
            this.Loaded += StatisticsWindow_Loaded;
        }

        private void StatisticsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            staffs = smdc.GetTable<DSNV>();
            departments = smdc.GetTable<DMPHONG>();

            statisticStaffCountPerDepartment();
        }

        private void statisticStaffCountPerDepartment()
        {
            dgStaffCountPerDepartment.ItemsSource = from d in departments
                                                    join s in staffs on d.MaPhong equals s.MaPhong into d_s
                                                    from ds in d_s.DefaultIfEmpty()
                                                    group ds by new { d.MaPhong, d.TenPhong } into res
                                                    select new
                                                    {
                                                        DepartmentName = res.Key.TenPhong,
                                                        TotalStaff = res.Where(s => s != null).Count()
                                                    };
        }
    }
}
