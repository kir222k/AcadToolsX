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

// объявим делегат:
public delegate void UpdateGridEventHandler();
public delegate void SelectAllGridRowsEventHandler();
public delegate void ExplodeDrawingEventHandler();

namespace ACADTOOLSX.Classes.GUI.Windows
{
    /// <summary>
    /// Interaction logic for M.xaml
    /// </summary>
    public partial class MzBaseWindow : UserControl
    {
        // объявление события:
        public event UpdateGridEventHandler UpdateGridEvent;
        public event SelectAllGridRowsEventHandler SelectAllGridRowsEvent;
        public event ExplodeDrawingEventHandler ExplodeDrawingEvent;

        public MzBaseWindow()
        {
            InitializeComponent();

            //ButtonAddStick.IsEnabled=false;
        }

        void OnChecked(object sender, RoutedEventArgs e)
        {
           // AcDocsGrid.Items.Refresh();
        }



        // Метод для вызова самого события (имя прописать в XAML кнопки Click="<метод>"
        private  void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateGridEvent?.Invoke();
        }

        //ButtonSelectAll_Click
        private void ButtonSelectAll_Click(object sender, RoutedEventArgs e)
        {
            SelectAllGridRowsEvent?.Invoke();
        }

        //ButtonExplode_Click
        private void ButtonExplode_Click(object sender, RoutedEventArgs e)
        {
            ExplodeDrawingEvent?.Invoke();
        }


    }
}
