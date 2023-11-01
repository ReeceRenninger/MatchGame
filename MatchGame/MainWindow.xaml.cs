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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>() 
            {
            "🐵", "🐵",
            "🐳","🐳",
            "🐙","🐙",
            "🐸","🐸",
            "🐶","🐶",
            "🦞","🦞",
            "🦇","🦇",
            "🦉","🦉",
            };
            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock") // adding the time text block broke our page so we need to add a condition so it doesn't target the time text block
                {
                textBlock.Visibility = Visibility.Visible;
                int index = random.Next(animalEmoji.Count);
                string nextEmoji = animalEmoji[index];
                textBlock.Text = nextEmoji;
                animalEmoji.RemoveAt(index);
                }
            }
            timer.Start(); //invoke the timer once the first text block is clicked?
            tenthsOfSecondsElapsed = 0; // setting the timer text to 0
            matchesFound = 0; // resetting matchesFound to 0
        }

        // global variables for the TextBLock_MouseDown
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
         TextBlock textBlock = sender as TextBlock;
        if (findingMatch == false) // if our default state is false 
            {
                textBlock.Visibility = Visibility.Hidden; 
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
        else if ( textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++; // increase matchesFound if text block texts matched
                textBlock.Visibility = Visibility.Hidden; // we hide the blocks as they are matched
                findingMatch = false; // ** Not sure what this is doing???
            }
        else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s"); // setting the text of the time text block
            if (matchesFound == 8) // if all matches are found, we restart the game
            {
                SetUpGame();
            }
        }
    }
}
