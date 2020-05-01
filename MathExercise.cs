using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using WPFFinalTest2.Model;

namespace WPFFinalTest2
{
    public class MathExercise : INotifyPropertyChanged
         {
        public bool Succeeded { get; set; }
        public int ButtonOneNumber { get; set; }
        public int ButtonTwoNumber { get; set; }
        public int ButtonThreeNumber { get; set; }
        public int ButtonFourNumber { get; set; }
        private int _counter;
        private bool _exerciseOn = true;
        public bool ExerciseOn { get { return _exerciseOn; } private set { _exerciseOn = value; OnPropertyChanged("ExerciseOn"); } }
        public int Counter { get { return _counter; } set { _counter = value; OnPropertyChanged("Counter"); ChooseResultCommand.RaiseCanExecuteChanged(); } }

        private Question _currentQuestion;
        public Question CurrentQuestion { get { return _currentQuestion; } private set { _currentQuestion = value; OnPropertyChanged("CurrentQuestion"); } }
        private List<string> _buttons;
        public List<string> Buttons { get { return _buttons; } private set { _buttons = value; OnPropertyChanged("Buttons"); } }
        private List<int> Options { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand<string> ChooseResultCommand { get; set; }
        private bool _areButtonsEnabled = true;
        public bool AreButtonsEnabled { get { return _areButtonsEnabled; } private set { _areButtonsEnabled = value; OnPropertyChanged("AreButtonsEnabled"); ChooseResultCommand.RaiseCanExecuteChanged(); } }
        private SolidColorBrush _currentBackground;
        public SolidColorBrush CurrentBackground { get { return _currentBackground; } set { _currentBackground = value; OnPropertyChanged("CurrentBackground"); } }

        public MathExercise()
        {
            CurrentBackground = new SolidColorBrush(Colors.White);
            CurrentQuestion = new Question();
            SetOptionsToButtons();
            DispatcherTimer timer = new DispatcherTimer();
            ChooseResultCommand = new DelegateCommand<string>((answer) => { if (answer != null) timer.Stop(); { if (CheckAnswer(Convert.ToInt32(answer))) { AreButtonsEnabled = false; CurrentBackground = new SolidColorBrush(Colors.LightGreen); Succeeded = true; ExerciseOn = false; } else { AreButtonsEnabled = false; CurrentBackground = new SolidColorBrush(Colors.Red); ExerciseOn = false; } } }, (answer) => { if (Counter == 0) return false; else return AreButtonsEnabled; });

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler((o, e) => { if (Counter > 0) Counter--; if (Counter == 0) { AreButtonsEnabled = false; CurrentBackground = new SolidColorBrush(Colors.Red); ExerciseOn = false; } });
            Counter = 30;
            timer.Start();
        }

        private void SetOptionsToButtons()
        {
            Options = new List<int>();
            Buttons = new List<string>();
            Random randomNumber = new Random();

            int maximum = CurrentQuestion.Result + 150;
            int minimum;
            if ((CurrentQuestion.Result - 100) < 0)
                minimum = 0;
            else
                minimum = CurrentQuestion.Result - 100;

            //check if not equal.
            for (int i = 0; i < 3; i++)
            {
                int currentValue = randomNumber.Next(minimum, maximum);
                while (Options.Contains(currentValue))
                {
                    currentValue = randomNumber.Next(minimum, maximum);
                }

                Options.Add(currentValue);
            }

            Options.Add(CurrentQuestion.Result);

            //check that resullt must appear, check that not twice - MAKE BETTER THAN THIS!
            for (int i = 0; i < Options.Count; i++)
            {
                Buttons.Add(Options[i].ToString());
            }

            Buttons = Buttons.OrderBy(a => Guid.NewGuid()).ToList();
        }
        private bool CheckAnswer(int answer)
        {
            if (answer == CurrentQuestion.Result)
                return true;
            else
                return false;
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}

