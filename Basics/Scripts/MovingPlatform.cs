﻿using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class MovingPlatform : Platform
    {
        public Point StartPoint { get; set; }
        public Point StopPoint { get; set; }
        private int _speed = 1;
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = Math.Max(1, value);
                if (moveTimer != null) moveTimer.Interval = 100 / value;
            }
        }
        //the time paused between moving in milliseconds
        public int Pause { get; set; }

        private int progress = 0;
        private int wait = 0;

        private Point movement = new Point();

        [NonSerialized]
        Timer moveTimer;

        public MovingPlatform(char display, int length, Point start, Point stop, int speed, int pause, string name = "Moving Platform") : base(display, length, name)
        {
            StartPoint = start;
            StopPoint = stop;
            Speed = speed;
            Pause = pause;
        }

        public override void OnWake()
        {
            moveTimer = new Timer(Speed);
            moveTimer.Elapsed += MoveTimer_Elapsed;
            moveTimer.Start();
        }

        public override void OnCollision(GameObject go, Direction side)
        {
            go.Transform.Position += movement;
        }

        private void MoveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(wait == 0 && progress < 180)
            {
                progress++;
                Point newPos = StartPoint + (Math.Cos(AdvMath.DegreesToRadians(progress)) * (StopPoint - StartPoint));
                movement = newPos - Transform.Position;
                Transform.Position = newPos;
            } else if (wait < Pause && progress == 180)
            {
                wait++;
                movement = new Point();
            } else if (wait == Pause && progress > 0)
            {
                progress--;
                Point newPos = StartPoint + (Math.Cos(AdvMath.DegreesToRadians(progress)) * (StopPoint - StartPoint));
                movement = newPos - Transform.Position;
                Transform.Position = newPos;
            } else if (wait > 0 && progress == 0)
            {
                wait--;
                movement = new Point();
            }

            Debug.Log($"W: {wait}\nP: {progress}");
        }
    }
}