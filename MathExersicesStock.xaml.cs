using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using WPFFinalTest2.ViewModel;

namespace WPFFinalTest2
{
    /// <summary>
    /// Interaction logic for MathExersicesStock.xaml
    /// </summary>
    public partial class MathExersicesStock : UserControl, INotifyPropertyChanged
    {
        private MathExercise _currentExercise;
        MathExercise CurrentExcercise { get { return _currentExercise; } set { _currentExercise = value; OnPropertyChanged("CurrentExcercise"); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private int _resultCounter;
        public int ResultCounter { get { return _resultCounter; } set { _resultCounter = value; OnPropertyChanged("ResultCounter"); } }
        private int _totalExercises;
        public int TotalExercises { get { return _totalExercises; } set { _totalExercises = value; OnPropertyChanged("TotalExercises"); } }
        public MathExersicesStock()
        {
            CurrentExcercise = new MathExercise();
            ResultCounter = 0;
            TotalExercises = 0;

            InitializeComponent();
            this.DataContext = CurrentExcercise;
            this.Counter1.DataContext = this;
            this.Counter2.DataContext = this;

            Task RunExercises = Task.Run(() =>
            {
                while (true)
                {
                    if (!CurrentExcercise.AreButtonsEnabled)
                    {
                        Action NewExercise = () => {
                            if (CurrentExcercise.Succeeded)
                                ResultCounter++;
                            CurrentExcercise = new MathExercise();
                            TotalExercises++;
                            this.DataContext = CurrentExcercise; 
                        };
                        Thread.Sleep(3000);
                        if (Dispatcher.CheckAccess())
                            NewExercise.Invoke();
                        else
                        {
                            Dispatcher.Invoke(NewExercise);
                        }
                    }

                    Thread.Sleep(1);
                }

            });
        }
        private void OnPropertyChanged (string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }



    }
}
