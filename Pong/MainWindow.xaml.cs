using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Threading;

namespace Pong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Reset button
        bool reset;

        //player1 up and down
        bool upOne, downOne;

        //player2 up and down
        bool upTwo, downTwo;

        int playerSpeed = 10;
        int speed = 3;

        //Inital coordinates for players and ball
        int ballTop = 197;
        int ballLeft = 386;
        int playerTop = 150;
        int playerOneLeft = 33;
        int playerTwoLeft = 742;

        //Bool ball direction (false = ball starts going towards player2)
        bool ballDirectionStart = false;


        //player1 Score
        int score1 = 0;
        

        //player2 Score
        int score2 = 0;

        //Ball-directions
        string direction = "left";

        DispatcherTimer gamerTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            myCanvas.Focus();

            gamerTimer.Tick += GameTimerEvent;
            gamerTimer.Interval = TimeSpan.FromMilliseconds(1);
            gamerTimer.Start();
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            //Reset button
            if (reset == true && winner.Visibility == Visibility.Visible)
            {
                winner.Visibility = Visibility.Hidden;
                score1 = 0;
                score2 = 0;
                //Setting Ball position
                Canvas.SetLeft(Ball, ballLeft);
                Canvas.SetTop(Ball, ballTop);

                //Setting player1 position
                Canvas.SetLeft(player1, playerOneLeft);
                Canvas.SetTop(player1, playerTop);

                //Setting player2 position
                Canvas.SetLeft(player2, playerTwoLeft);
                Canvas.SetTop(player2, playerTop);

                speed = 3;
            }

            //player1 movement
            if (upOne == true && Canvas.GetTop(player1) > 5)
            {
                Canvas.SetTop(player1, Canvas.GetTop(player1) - playerSpeed);
                upOne = false;
            }

            if (downOne == true && Canvas.GetTop(player1) + (player1.Height + 50) < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(player1, Canvas.GetTop(player1) + playerSpeed);
                downOne = false;
            }

            //player2 movement
            if (upTwo == true && Canvas.GetTop(player2) > 5)
            {
                Canvas.SetTop(player2, Canvas.GetTop(player2) - playerSpeed);
                upTwo = false;
            }
            if (downTwo == true && Canvas.GetTop(player2) + (player2.Height + 50) < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(player2, Canvas.GetTop(player2) + playerSpeed);
                downTwo = false;
            }

            //Ball-physics
            if (direction == "left")
            {
                Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) - speed);
            }
            if (direction == "right")
            {
                Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) + speed);
            }
            if (direction == "upLeft")
            {
                Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) - speed);
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) - speed);
            }
            if (direction == "downLeft")
            {
                Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) - speed);
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) + speed);

            }
            if (direction == "upRight")
            {
                Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) + speed);
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) - speed);
            }
            if (direction == "downRight")
            {
                Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) + speed);
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) + speed);
            }
            
            if(reset == true)
            {
                score1 = 0;
                score2 = 0;

                //Setting Ball position
                Canvas.SetLeft(Ball, ballLeft);
                Canvas.SetTop(Ball, ballTop);

                //Setting player1 position
                Canvas.SetLeft(player1, playerOneLeft);
                Canvas.SetTop(player1, playerTop);

                //Setting player2 position
                Canvas.SetLeft(player2, playerTwoLeft);
                Canvas.SetTop(player2, playerTop);

                direction = "left";
            }

            //Hit Detection

            //Hit detectiong on top and bottom edges
            if (Canvas.GetTop(Ball) <= 1 || Canvas.GetTop(Ball) + Ball.Height + 25 >= Application.Current.MainWindow.Height)
            {
                ReverseDirection();
            }


            //Player1's paddle hit detection
            if (Canvas.GetLeft(Ball) <= Canvas.GetLeft(player1) + 25)
            {
                if (Canvas.GetTop(Ball) <= Canvas.GetTop(player1) + 35 && Canvas.GetTop(Ball) >= Canvas.GetTop(player1))
                {
                    paddleDirectionTop();
                    speed += 1;
                }
                else if (Canvas.GetTop(Ball) <= Canvas.GetTop(player1) + 65 && Canvas.GetTop(Ball) > Canvas.GetTop(player1) + 35)
                {
                    paddleDirectionMiddle();
                    speed += 1;
                }
                else if (Canvas.GetTop(Ball) <= Canvas.GetTop(player1) + 100 && Canvas.GetTop(Ball) > Canvas.GetTop(player1) + 65)
                {
                    paddleDirectionBottom();
                    speed += 1;
                }
            }


            //Player2's paddle hit detection
            if (Canvas.GetLeft(Ball) + 25 >= Canvas.GetLeft(player2))
            {
                if (Canvas.GetTop(Ball) <= Canvas.GetTop(player2) + 35 && Canvas.GetTop(Ball) >= Canvas.GetTop(player2))
                {
                    paddleDirectionTop();
                    speed += 1;
                }
                else if (Canvas.GetTop(Ball) <= Canvas.GetTop(player2) + 65 && Canvas.GetTop(Ball) >= Canvas.GetTop(player2) + 35)
                {
                    paddleDirectionMiddle();
                    speed += 1;
                }
                else if (Canvas.GetTop(Ball) <= Canvas.GetTop(player2) + 100 && Canvas.GetTop(Ball) >= Canvas.GetTop(player1) + 65)
                {
                    paddleDirectionBottom();
                    speed += 1;
                }
            }


            //Hit detection behind the paddles

            //player1 scores
            if (Canvas.GetLeft(Ball) >= 767)
            {
                score1 += 1;
                playerScore1.Content = score1;
                ballDirectionStart = true;
                GameReset();
            }

            //player2 scores
            if (Canvas.GetLeft(Ball) <= 33)
            {
                score2 += 1;
                playerScore2.Content = score2;
                ballDirectionStart = false;
                GameReset();

            }

            if (score1 == 7)
            {
                GameEnd1();
            }
            else if (score2 == 7)
            {
                GameEnd2();
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            //Keydown combos
            if (e.Key == Key.Down && Keyboard.IsKeyDown(Key.S))
            {
                downOne = true;
                downTwo = true;
            }

            else if (e.Key == Key.Up && Keyboard.IsKeyDown(Key.W))
            {
                upOne = true;
                upTwo = true;
            }

            else if (e.Key == Key.Up && Keyboard.IsKeyDown(Key.S))
            {
                downOne = true;
                upTwo = true;
            }

            else if (e.Key == Key.Down && Keyboard.IsKeyDown(Key.W))
            {
                upOne = true;
                downTwo = true;
            }

            //player1 keys down
            else if (e.Key == Key.S)
            {
                downOne = true;
            }
            else if (e.Key == Key.W)
            {
                upOne = true;
            }

            //player2 keys down
            else if (e.Key == Key.Down)
            {
                downTwo = true;
            }

            else if (e.Key == Key.Up)
            {
                upTwo = true;
            }

            else if (e.Key == Key.Space)
            {
                reset = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            //player1 keys up
            if (e.Key == Key.S)
            {
                downTwo = false;
            }
            if (e.Key == Key.W)
            {
                upTwo = false;
            }

            //player2 keys up
            if (e.Key == Key.Down)
            {
                downOne = false;
            }
            if (e.Key == Key.Up)
            {
                upOne = false;
            }

            if(e.Key == Key.Space)
            {
                reset = false;
            }



        }

        void ReverseDirection()
        {
            if (direction == "downRight")
            {
                direction = "upRight";
            }

            else if (direction == "upLeft")
            {
                direction = "downLeft";
            }

            else if (direction == "upRight")
            {
                direction = "downRight";
            }

            else if (direction == "downLeft")
            {
                direction = "upLeft";
            }

            else if (direction == "right")
            {
                direction = "left";
            }

            else if (direction == "left")
            {
                direction = "right";
            }



        }

        void paddleDirectionTop()
        {
            if (direction == "downRight")
            {
                direction = "upLeft";
            }
            else if (direction == "upRight")
            {
                direction = "upLeft";
            }
            else if (direction == "downLeft")
            {
                direction = "upRight";
            }
            else if (direction == "upLeft")
            {
                direction = "upRight";
            }
            else if (direction == "right")
            {
                direction = "upLeft";
            }
            else if (direction == "left")
            {
                direction = "upRight";
            }
        }

        void paddleDirectionMiddle()
        {
            if (direction == "downRight")
            {
                direction = "left";
            }
            else if (direction == "upRight")
            {
                direction = "left";
            }
            else if (direction == "downLeft")
            {
                direction = "upRight";
            }
            else if (direction == "upLeft")
            {
                direction = "right";
            }
            else if (direction == "right")
            {
                direction = "left";
            }
            else if (direction == "left")
            {
                direction = "right";
            }
        }
        void paddleDirectionBottom()
        {
            if (direction == "downRight")
            {
                direction = "downLeft";
            }
            else if (direction == "upRight")
            {
                direction = "downLeft";
            }
            else if (direction == "downLeft")
            {
                direction = "downRight";
            }
            else if (direction == "upLeft")
            {
                direction = "downRight";
            }
            else if (direction == "right")
            {
                direction = "downLeft";
            }
            else if (direction == "left")
            {
                direction = "downRight";
            }
        }

        private void GameReset()
        {
            //Setting Ball position
            Canvas.SetLeft(Ball, ballLeft);
            Canvas.SetTop(Ball, ballTop);

            //Setting player1 position
            Canvas.SetLeft(player1, playerOneLeft);
            Canvas.SetTop(player1, playerTop);

            //Setting player2 position
            Canvas.SetLeft(player2, playerTwoLeft);
            Canvas.SetTop(player2, playerTop);

            speed = 3;

            if (!ballDirectionStart)
            {
                direction = "left";
            }
            else
            {
                direction = "right";
            }
        }

        void GameEnd1()
        {
            speed = 0;
            winner.Content = "WINNER IS PLAYER 1";
            winner.Visibility = Visibility.Visible;
            
            

        }
        void GameEnd2()
        {
            speed = 0;
            winner.Content = "WINNER IS PLAYER 2";
            winner.Visibility = Visibility.Visible;
        }

    }
}