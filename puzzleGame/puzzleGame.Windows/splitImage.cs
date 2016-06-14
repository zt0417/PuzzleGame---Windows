using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace puzzleGame
{
    public class splitImage
    {

        //Variable declaration
        public int currentLocalX { get; set; }
        public int currentLocalY { get; set; }
        public int correctLocalX { get; set; }
        public int correctLocalY { get; set; }
        public int moveDirection { get; set; }
        public int numberLocation { get; set; }

        public int realLocation { get; set; }
        public Image bitmapImage { get; set; }
        //0 - no, 1 - up, 2 - left, 3 - down, 4 - right

        public splitImage()
        {
        }

        public splitImage(int theCurrentLocalX, int theCurrentLocalY, int theCorrectLocalX, int theCorrectLocalY, int theNumberLocation,int theRealLocation, int theMoveDirection, Image theBitmapImage)
        {
            currentLocalX = theCurrentLocalX;
            correctLocalX = theCorrectLocalX;
            currentLocalY = theCurrentLocalY;
            correctLocalY = theCorrectLocalY;
            numberLocation = theNumberLocation;
            moveDirection = theMoveDirection;
            realLocation = theRealLocation;
            bitmapImage = theBitmapImage;
        }
        public int getCurrentLocalX()
        {
            return currentLocalX;
        }
        public int getCurrentLocalY()
        {
            return currentLocalY;
        }

        public int getNumberLocation()
        {
            return numberLocation;
        }

    }
}
