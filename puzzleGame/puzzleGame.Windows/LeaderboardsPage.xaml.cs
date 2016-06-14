using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace puzzleGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LeaderboardsPage : Page
    {
      
        //Variable declaration

        //Leaderboards file
        public StorageFile leaderboardsDataFile = null;
        public const string leaderboardsFileName = "Leaderboards.dat";
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        string fileContent;
        bool splitIt = true;
        message obj;

        /// \brief  LeaderboardsPage
        ///
        /// \details <b>Details</b>
        /// - LeaderboardsPage constructor. Initalizes components on the page.
        ///   Then immediately loads the leaderboards and structures them for display.
        ///
        /// \param <b>N/A</b> - N/A
        /// 
        /// \return <b>N/A</b> - N/A
        public LeaderboardsPage()
        {
            this.InitializeComponent();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
            waitForReadFromFile();
            structureTheLeaderboards();
        }

        /// \brief  structureTheLeaderboards
        ///
        /// \details <b>Details</b>
        /// - Structures the leaderboards to display all the users from the leaderboards file.
        ///   Essentially there's a file that gets loaded in with delimiters breaking apart each player.
        ///   The players are then identified with their corrisponding score and are placed into the 
        ///   leaderboards appropriately.
        ///
        /// \param <b>N/A</b> - N/A
        /// 
        /// \return <b>Task</b> - Waits on a task to complete
        private async Task structureTheLeaderboards()
        {
            //Variable declaration
            int rank = 1;
            await readFromFile();

            //If a file is loaded , we can split the strings
            if (splitIt)
            {
                //Start splitting the strings
                String[] content = new String[] { };
                content = fileContent.Split(",".ToCharArray());

                //Place the strings into a list
                List<Tuple<string, int>> theLeaderboards = new List<Tuple<string, int>>();

                //The list is a tuple so we can keep both the name and score contained to a single person.
                for (int i = 0; i < content.Count() - 1; i = i + 2)
                {
                    theLeaderboards.Add(new Tuple<string, int>(content[i], Convert.ToInt32(content[i + 1])));
                }

                //Sort the leaderboards in ascending order by the score.
                var sortedLeaderboards = from score in theLeaderboards orderby score.Item2 ascending select score;

                theLeaderboards = sortedLeaderboards.ToList();

                //Cycle through each player and add them to our leaderboard to display to the user.
                foreach (var thePlayers in theLeaderboards)
                {
                    TextBlock textBlockRank = new TextBlock();
                    TextBlock textBlockName = new TextBlock();
                    TextBlock textBlockScore = new TextBlock();

                    textBlockRank.Text = "" + rank;
                    textBlockName.Text = thePlayers.Item1;
                    textBlockScore.Text = "" + thePlayers.Item2;

                    textBlockRank.FontSize = 16;
                    textBlockName.FontSize = 16;
                    textBlockScore.FontSize = 16;

                    gridLeaderboards.Children.Add(textBlockRank);
                    gridLeaderboards.Children.Add(textBlockName);
                    gridLeaderboards.Children.Add(textBlockScore);

                    Grid.SetRow(textBlockRank, rank);
                    Grid.SetColumn(textBlockRank, 0);

                    Grid.SetRow(textBlockName, rank);
                    Grid.SetColumn(textBlockName, 1);

                    Grid.SetRow(textBlockScore, rank);
                    Grid.SetColumn(textBlockScore, 2);

                    rank++;
                }
            }
        }

        /// \brief  waitForReadFromFile
        ///
        /// \details <b>Details</b>
        /// - Waits for the read to happen from the leaderboards file.
        ///
        /// \param <b>N/A</b> - N/A
        /// 
        /// \return <b>Task</b> - Waits on a task to complete
        public async Task waitForReadFromFile()
        {
            await readFromFile();
        }

        /// \brief  readFromFile
        ///
        /// \details <b>Details</b>
        /// - Attempts to read from the leaderboards file.
        ///
        /// \param <b>N/A</b> - N/A
        /// 
        /// \return <b>Task</b> - Waits on a task to complete
        private async Task readFromFile()
        {
            //The storage file
            StorageFile theFile = await localFolder.GetFileAsync(leaderboardsFileName);

            if (theFile != null)
            {
                leaderboardsDataFile = theFile;
            }
            if (leaderboardsDataFile != null)
            {
                //Try to read the contents into our variable.
                try
                {
                    fileContent = await FileIO.ReadTextAsync(leaderboardsDataFile);
                    splitIt = true;
                }
                catch (FileNotFoundException)
                {
                    splitIt = false;
                }
            }
            else
            {
                splitIt = false;
            }
        }

        /// \brief  backButton_Click
        ///
        /// \details <b>Details</b>
        /// - When the back button is clicked the user is navigated to the gamepage.
        ///
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(gamePage), obj);
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            obj = (message)e.Parameter;
        }
    }
}
