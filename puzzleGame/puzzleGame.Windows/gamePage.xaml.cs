/*
* FILE : gamePage.cs
* PROJECT : Windows Programming – PROG2120 Windows Project Option 1 puzzle game
* PROGRAMMER : Tong Zhnag
* FIRST VERSION : 2015-12-17
* DESCRIPTION :The puzzle game can use a picture from your file system or from the camera as a source of your puzzle picture.
               When using the picture from a file, user can see a preview of the picture to make your selection. The game should
               be able to “cut up” your picture into a 4 x 4 matrix, and discard the bottom right corner for the puzzle.  
               The game should be able to randomize the tiles as a starting point. The action on the screen should be a swipe
               of a valid tile to the empty spot. It can be animated. The game has a determine a winning solution. In addition,
               the following features should be implemented. The game tile on the Start Menu should show the current puzzle 
               if the game is loaded, or the last puzzle if the game is not loaded. Game state must be maintained so user can
               continue from where you left off if Windows suspends or terminates your program. Appropriate splash screen and
               icons must be implemented.
* 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace puzzleGame
{

    public class message
    {
        public List<splitImage> orginal;
        public int time;
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class gamePage : Page
    {

        //File IO Initialization.
        private StorageFile gameDataFile = null;
        private const string gameDataFileName = "GameData.dat";
        private string fileContent;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        //Leaderboards file
        private StorageFile leaderboardsDataFile = null;
        private const string leaderboardsFileName = "Leaderboards.dat";


        private BitmapImage SplitImage = new BitmapImage();
        BitmapImage bitmapImage = new BitmapImage();
        List<splitImage> countImage = new List<puzzleGame.splitImage>();
        message old = new message();

        Image simpleImage1 = new Image();

        //Timer variables
        private DispatcherTimer timer;

        //Game Logic
        private bool gameRunning;
        private bool weArePausing = false;

        private int theScore = 0;

        String thePlayerName;

        /// <summary>
        /// 
        /// </summary>
        public gamePage()
        {
            this.InitializeComponent();
            gameRunning = true;

            CreateLeaderboards();
            //If we're pausing the game clal the pause_game_click function
            if (weArePausing)
            {
                Pause_Game_Click(null, null);
            }

            startGame();
            Pause_Game_Click(null, null);

            weArePausing = true;
            txtblckElapsedTime.Text = Convert.ToString(theScore);
        }

        /// <summary>
        /// 
        /// </summary>
        private void startGame()
        {
            //Timer
            StartTimer();
            gameRunning = true;
        }



        /// \brief  waitForReadFromFile
        ///
        /// \details <b>Details</b>
        /// - Waits for the read to happen from the leaderboards file.
        ///
        /// \param <b>N/A</b> - N/A
        /// 
        /// \return <b>Task</b> - Waits on a task to complete
        private async Task waitForReadFromFile()
        {
            await readFromFile();
        }




        /// \brief  loadGame
        ///
        /// \details <b>Details</b>
        /// - loadGame initializes all the variables from the game database files.
        ///   Starts the timer and sets the location of all the mines in their last recorded locations.
        ///   It also places the robot where it was last found.
        ///
        /// \param <b>N/A</b> - N/A
        /// 
        /// \return <b>N/A</b> - N/A
        private async void loadGame()
        {
            //Variable declaration
            await waitForReadFromFile();
        }








        /// \brief  StartTimer
        ///
        /// \details <b>Details</b>
        /// - Starts the timer for the game
        ///         ///
        /// \param <b>N/A</b> - N/A.
        /// 
        /// \return <b>N/A</b> - N/A.
        private void StartTimer()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                timer.Tick += timer_Tick;
            }
            timer.Start();
        }



        /// \brief  timer_Tick
        ///
        /// \details <b>Details</b>
        /// - Starts the timer for the game. Every time it ticks the game is saved
        ///   and written to the file. It also keeps track of the score as long as the game is running
        ///   displaying it to the user. One final functionality is that it checks to see if the game
        ///   is finished or not.
        /// 
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private async void timer_Tick(object sender, object args)
        {
            //Write to the file.
            if (theScore >= 0 && gameRunning == true)
            {
                theScore = theScore + 1;
                txtblckElapsedTime.Text = Convert.ToString(theScore);

                //string theStringToWrite = (gameRunning + "," + theRobot.row + "|" + theRobot.col + "," + "false" + "," + theScore + "," + numberOfMinesDestroyed + "," + numberOfActiveMines);
                //for (int i = 0; i < numberOfActiveMines; i++)
                //{
                //    int theRow = mineList[i].getRow();
                //    int theCol = mineList[i].getCol();
                //    theStringToWrite += ("," + theRow + "|" + theCol);
                //}
                //await writeToFile(theStringToWrite);


            }

            //We're pausing the game so pause it.
            if (weArePausing == true)
            {
                Pause_Game_Click(null, null);
            }

            compare();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        async private void BitmapTransformTest(StorageFile file)
        {
            Random r = new Random();

            int z = 0;

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {

                    Image simpleImage = new Image();
                    var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                    // create a new stream and encoder for the new image
                    InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
                    BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

                    // convert the entire bitmap to a 800px by 800px bitmap
                    enc.BitmapTransform.ScaledHeight = 800;
                    enc.BitmapTransform.ScaledWidth = 800;

                    BitmapBounds bounds = new BitmapBounds();
                    bounds.Height = 200;
                    bounds.Width = 200;
                    bounds.X = Convert.ToUInt32(0 + x * 200);
                    bounds.Y = Convert.ToUInt32(0 + y * 200);
                    enc.BitmapTransform.Bounds = bounds;

                    // write out to the stream
                    try
                    {
                        await enc.FlushAsync();
                    }
                    catch (Exception ex)
                    {
                        string s = ex.ToString();
                    }

                    BitmapImage bImg = new BitmapImage();
                    bImg.SetSource(ras);

                    int theCurrentLocalX = r.Next(0, 4);
                    int theCurrentLocalY = r.Next(0, 4);
                    int theNumberLocation = 4 * theCurrentLocalX + theCurrentLocalY;

                    if (countImage != null)
                    {
                        for (int j = 0; j < countImage.Count(); j++)
                        {
                            if (theCurrentLocalX == countImage[j].getCurrentLocalX() && theCurrentLocalY == countImage[j].getCurrentLocalY())
                            {
                                theCurrentLocalX = r.Next(0, 4);
                                theCurrentLocalY = r.Next(0, 4);
                                theNumberLocation = 4 * theCurrentLocalX + theCurrentLocalY;
                                j = -1;
                            }
                        }

                    }
                    if (x == 3 && y == 3)
                    {
                        StorageFile file1 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/blank.png"));
                        //await file1.CopyAsync(ApplicationData.Current.LocalFolder, "image.png");
                        BitmapImage img = new BitmapImage();

                        img = await LoadImage(file1);
                        simpleImage1.Source = img;



                        //simpleImage.Source = null; // image element in xaml
                        splitImage splitImage = new splitImage(theCurrentLocalX, theCurrentLocalY, x, y, theNumberLocation, z, 0, simpleImage1);
                        countImage.Add(splitImage);
                        Grid.SetRow(simpleImage1, theCurrentLocalY);
                        Grid.SetColumn(simpleImage1, theCurrentLocalX);
                        gridGameField.Children.Add(simpleImage1);
                    }
                    else
                    {
                        simpleImage.Source = bImg; // image element in xaml
                        splitImage splitImage = new splitImage(theCurrentLocalX, theCurrentLocalY, x, y, theNumberLocation, z, 0, simpleImage);
                        countImage.Add(splitImage);
                        Grid.SetRow(simpleImage, theCurrentLocalY);
                        Grid.SetColumn(simpleImage, theCurrentLocalX);
                        gridGameField.Children.Add(simpleImage);
                    }

                    z++;
                }
            }
        }

        private int CanMoveImage(int touchNumber)
        {
            int boardLoc = -1;
            int blackLoc = -1;

            foreach (splitImage item in this.countImage)
            {
                if (item.numberLocation == touchNumber)
                {
                    boardLoc = item.numberLocation;
                }
                else if (item.bitmapImage == simpleImage1)
                {
                    blackLoc = item.numberLocation;
                }
            }

            if ((boardLoc == blackLoc + 1) || (boardLoc == blackLoc - 1) ||
                (boardLoc == blackLoc + 4) || (boardLoc == blackLoc - 4))
            {
                if (boardLoc + 1 == blackLoc)
                {
                    return 3;
                }
                else if (boardLoc - 1 == blackLoc)
                {
                    return 1;
                }
                else if (boardLoc - 4 == blackLoc)
                {
                    return 4;
                }
                else if (boardLoc + 4 == blackLoc)
                {
                    return 2;
                }
            }
            return 0;
        }

        private int directionLoc(int location)
        {
            if (location == 1)
            {
                return -1;
            }
            else if (location == 2)
            {
                return 4;
            }
            else if (location == 3)
            {
                return 1;
            }
            else if (location == 4)
            {
                return -4;
            }

            return 0;
        }

        private async System.Threading.Tasks.Task<bool> MovePiece(int from, int to)
        {
            int boardLoc = from;//need move
            int direction = to; // to
            int blackLoc = -1;

            foreach (splitImage item in this.countImage)
            {
                if (item.bitmapImage == simpleImage1)
                {
                    blackLoc = item.numberLocation;  //to
                }
            }

            // Check if we can move
            if ((boardLoc == blackLoc + 1) || (boardLoc == blackLoc - 1) ||
                (boardLoc == blackLoc + 4) || (boardLoc == blackLoc - 4))
            {
                splitImage temp = new splitImage();

                foreach (splitImage item in this.countImage)
                {
                    if (item.numberLocation == from)
                    {
                        foreach (splitImage subItem in this.countImage)
                        {
                            if (subItem.numberLocation == to)
                            {
  
                                temp.currentLocalX = item.currentLocalX;
                                temp.currentLocalY = item.currentLocalY;
                                temp.numberLocation = item.numberLocation;
                             
                                item.currentLocalX = subItem.currentLocalX;
                                item.currentLocalY = subItem.currentLocalY;
                                item.numberLocation = subItem.numberLocation;
                            
                                subItem.currentLocalX = temp.currentLocalX;
                                subItem.currentLocalY = temp.currentLocalY;
                                subItem.numberLocation = temp.numberLocation;
                          

                                Grid.SetColumn(item.bitmapImage, item.currentLocalX);
                                Grid.SetRow(item.bitmapImage, item.currentLocalY);

                                Grid.SetColumn(subItem.bitmapImage, subItem.currentLocalX);
                                Grid.SetRow(subItem.bitmapImage, subItem.currentLocalY);

                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        private static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;

        }
        private void Grid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (gameRunning == true)
            {
                int x, y, z;
                int whichLoc = 0;
                Image obj = (Image)e.OriginalSource;
                y = Grid.GetRow(obj);
                x = Grid.GetColumn(obj);
                z = 4 * x + y;

                int direction = CanMoveImage(z);

                whichLoc = directionLoc(direction);

                if ((whichLoc != 0))
                {
                    MovePiece(z, z + whichLoc);

                    compare();

                }
            }

        }

        private void compare()
        {
            int k = 0;
            foreach (splitImage e in countImage)
            {

                if (e.numberLocation != e.realLocation)
                {
                    break;
                }
                k++;
            }
            if (k == 16)
            {
                gameRunning = false;
                timer.Stop();
                btnBackgroundYouWin.Visibility = Visibility.Visible;
                txtBoxUserName.Visibility = Visibility.Visible;
                txtBlockCongratulationsLabel.Visibility = Visibility.Visible;
                btnSubmitUsername.Visibility = Visibility.Visible;
            }
        }


        //Top App Bar
        /// \brief  Resume_Game_Click
        ///
        /// \details <b>Details</b>
        /// - When the view Resume_Game_Click is clicked the game gets resumed 
        ///   by setting the gameRunning to true and it removes the pause message.
        ///   The timer is now running.
        ///
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private void Resume_Game_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            gameRunning = true;
            weArePausing = false;
            btnPaused.Opacity = 0;
        }


        /// \brief  Pause_Game_Click
        ///
        /// \details <b>Details</b>
        /// - When the view Pause_Game_Click is clicked the game gets paused 
        ///   by setting the gameRunning to false and it displays the pause message.
        ///   The timer is no longer running.
        ///
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private void Pause_Game_Click(object sender, RoutedEventArgs e)
        {
            btnPaused.Opacity = 100;
            timer.Stop();
            gameRunning = false;
            
        }


        /// \brief  Toggle_SreenLock_Checked
        ///
        /// \details <b>Details</b>
        /// - When the view Toggle_SreenLock_Checked is clicked the user is now locking their screen to the 
        ///   current orientation. This is part of the top app bar.
        ///
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private void Toggle_SreenLock_Checked(object sender, RoutedEventArgs e)
        {
            DisplayInformation.AutoRotationPreferences = DisplayInformation.GetForCurrentView().CurrentOrientation;
        }


        /// \brief  Toggle_SreenLock_Unchecked
        ///
        /// \details <b>Details</b>
        /// - When the view Toggle_SreenLock_Unchecked is clicked the user is no longer locking their screen to the 
        ///   current orientation. This is part of the top app bar.
        ///
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private void Toggle_SreenLock_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            StorageFile parameter = e.Parameter as StorageFile;

            BitmapImage photo = new BitmapImage();
            photo = await LoadImage1(parameter);
            image.Source = photo;

            BitmapTransformTest(parameter);

            // leaderboard for inner
            //try
            //{

            //}
            //catch (Exception)
            //{

            //}
            //old = e.Parameter as message;
            //countImage = old.orginal;
            //theScore = old.time;

            //foreach (splitImage item in countImage)
            //{

            //    Grid.SetColumn(item.bitmapImage, item.currentLocalX);
            //    Grid.SetRow(item.bitmapImage, item.currentLocalY);
            //    gridGameField.Children.Add(item.bitmapImage);
            //}
        }


        private static async Task<BitmapImage> LoadImage1(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;

        }

        /// \brief  btnSubmitUsername_Click
        ///
        /// \details <b>Details</b>
        /// - When the view btnSubmitUsername_Click is clicked the user submits their name 
        ///   and score to the leaderboards file. If the user didnt enter a name, the value is
        ///   defaulted to Username. The game data file is now deleted and the game is ready
        ///   to start a new session.
        ///
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private async void btnSubmitUsername_Click(object sender, RoutedEventArgs e)
        {
            btnBackgroundYouWin.Visibility = Visibility.Collapsed;
            txtBoxUserName.Visibility = Visibility.Collapsed;
            txtBlockCongratulationsLabel.Visibility = Visibility.Collapsed;
            btnSubmitUsername.Visibility = Visibility.Collapsed;

            thePlayerName = txtBoxUserName.Text;

            if (thePlayerName.Length < 1)
            {
                thePlayerName = "Username";
            }

            //Writes to the leaderboards file
            await writeToLeaderboards(thePlayerName + "," + theScore + ",", 1);


            this.Frame.Navigate(typeof(LeaderboardsPage));
        }



       

        /// \brief  View_Leaderboards_Click
        ///
        /// \details <b>Details</b>
        /// - When the view leaderbords button is clicked the user is navigated to the leaderboards.
        ///   The game is also paused. This is part of the top app bar.
        ///
        /// \param sender - <b>object</b> - The object being sent.
        /// \param e - <b>RoutedEventArgs</b> - The event associated with the call.
        /// 
        /// \return <b>N/A</b> - N/A
        private void View_Leaderboards_Click(object sender, RoutedEventArgs e)
        {
            message msg = new message();
            msg.orginal = countImage;
            msg.time = theScore;

            Pause_Game_Click(null, null);
            this.Frame.Navigate(typeof(LeaderboardsPage), msg);
        }




        /// <summary>
        /// End of Game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void End_Game_Click(object sender, RoutedEventArgs e)
        {
            foreach (splitImage item in countImage)
            {
                item.numberLocation = item.realLocation;
                Grid.SetRow(item.bitmapImage, item.correctLocalY);
                Grid.SetColumn(item.bitmapImage, item.correctLocalX);
                await Task.Delay(TimeSpan.FromSeconds(0.2));
            }
            compare();
        }

        /// \brief  CreateTheFile
        ///
        /// \details <b>Details</b>
        /// - If the gamedata file is found, we do not create the file since it exists.
        ///   Otherwise we create a new file with the name and if for whatever reason there's a collision
        ///   we replace the file found.
        /// 
        ///  If the file is found, we load the game - otherwise we're starting a new one.
        ///
        /// \param <b>N/A</b> - N/A.
        /// 
        /// \return <b>N/A</b> - N/A.
        async private void CreateTheFile(String theFileName)
        {
            try
            {
                StorageFile theFile = await localFolder.GetFileAsync(theFileName);

                if (theFile != null)
                {
                    gameDataFile = theFile;
                    loadGame();
                }
            }
            catch (Exception e)
            {
                StorageFile createTheFile = await localFolder.CreateFileAsync(theFileName, CreationCollisionOption.ReplaceExisting);
                startGame();
                //ErrorTextBlock.Text = e.ToString();
                //ErrorTextBlock1.Text = e.ToString();
            }
        }

        /// \brief  CreateLeaderboards
        ///
        /// \details <b>Details</b>
        /// - If the leaderboard file is found, we do not create the file since it exists.
        ///   Otherwise we create a new file with the name and if for whatever reason there's a collision
        ///   we replace the file found.
        ///
        /// \param <b>N/A</b> - N/A.
        /// 
        /// \return <b>N/A</b> - N/A.
        async private void CreateLeaderboards()
        {
            try
            {
                StorageFile theFile = await localFolder.GetFileAsync(leaderboardsFileName);

                if (theFile != null)
                {
                    leaderboardsDataFile = theFile;
                }
            }
            catch (Exception e)
            {
                StorageFile createTheLeaderboardsFile = await localFolder.CreateFileAsync(leaderboardsFileName, CreationCollisionOption.ReplaceExisting);
            }
        }

        /// \brief  writeToFile
        ///
        /// \details <b>Details</b>
        /// - If the gamedata file is found, we write the content pased into it.
        ///
        /// \param userContent - <b>string</b> - The content being written.
        /// 
        /// \return <b>Task</b> - Waits on a task to complete.
        private async Task writeToFile(string userContent)
        {
            StorageFile theFile = await localFolder.GetFileAsync(gameDataFileName);
            gameDataFile = theFile;
            if (gameDataFile != null)
            {
                try
                {
                    if (!String.IsNullOrEmpty(userContent))
                    {
                        await FileIO.WriteTextAsync(gameDataFile, userContent);
                    }
                    else
                    {
                        //ErrorTextBlock.Text = "Nothing to write";
                        //ErrorTextBlock1.Text = "Nothing to write";
                    }
                }
                catch (FileNotFoundException)
                {
                    //ErrorTextBlock.Text = "File doesn't exist";
                    //ErrorTextBlock1.Text = "File doesn't exist";
                }
            }
            else
            {
                CreateTheFile(gameDataFileName);
                //ErrorTextBlock.Text = "File doesn't exist2";
                //ErrorTextBlock1.Text = "File doesn't exist2";
            }
        }

        /// \brief  writeToLeaderboards
        ///
        /// \details <b>Details</b>
        /// - If the leaderboards file is found, we write the content pased into it.
        ///   If type of write is 1 then the contents are appeneded to the file.
        ///   If the type is 0, the contents overwrite the file. 
        ///
        /// \param userContent - <b>string</b> - The content being written.
        /// \param whatTypeOfWrite - <b>int</b> - 1 indicates append, any other number indicates overwrite.
        /// 
        /// \return <b>Task</b> - Waits on a task to complete.
        private async Task writeToLeaderboards(string userContent, int whatTypeOfWrite)
        {
            StorageFile theFile = await localFolder.GetFileAsync(leaderboardsFileName);
            leaderboardsDataFile = theFile;
            if (leaderboardsDataFile != null)
            {
                try
                {
                    if (!String.IsNullOrEmpty(userContent))
                    {
                        if (whatTypeOfWrite == 1)
                        {
                            await FileIO.AppendTextAsync(leaderboardsDataFile, userContent);
                        }
                        else
                        {
                            await FileIO.WriteTextAsync(leaderboardsDataFile, userContent);
                        }
                    }
                    else
                    {
                        //ErrorTextBlock.Text = "Nothing to write";
                        //ErrorTextBlock1.Text = "Nothing to write";
                    }
                }
                catch (FileNotFoundException)
                {
                    //ErrorTextBlock.Text = "File doesn't exist";
                    //ErrorTextBlock1.Text = "File doesn't exist";
                }
            }
            else
            {
                CreateTheFile(leaderboardsFileName);
                //ErrorTextBlock.Text = "File doesn't exist2";
                //ErrorTextBlock1.Text = "File doesn't exist";
            }
        }

        /// \brief  readFromFile
        ///
        /// \details <b>Details</b>
        /// - If the gamedata file is found, the file gets read from into a string called fileContent
        ///
        /// \param sender - <b>N/A</b> - N/A.
        /// 
        /// \return <b>Task</b> - Waits on a task to complete.
        private async Task readFromFile()
        {
            if (gameDataFile != null)
            {
                try
                {
                    fileContent = await FileIO.ReadTextAsync(gameDataFile);
                    //ErrorTextBlock.Text = "" + (gameDataFile.Name + "" + Environment.NewLine + "" + fileContent);
                    //ErrorTextBlock1.Text = "" + (gameDataFile.Name + "" + Environment.NewLine + "" + fileContent);

                }
                catch (FileNotFoundException)
                {
                    //ErrorTextBlock.Text = "Doesn't exist";
                    //ErrorTextBlock1.Text = "Doesn't exist";
                }
            }
            else
            {
                //ErrorTextBlock.Text = "Doesn't exist2";
                //ErrorTextBlock1.Text = "Doesn't exist2";
            }
        }


    }
}
