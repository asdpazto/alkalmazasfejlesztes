﻿using Robot;
using System;
using System.Collections.Generic;
using System.Text;

namespace RobotBrain.State
{
    public class FollowingWallOnRightState : StateBase
    {
        // This state requires a LineAndWallDetectorRobot.
        LineAndWallDetectorRobot Robot => this.Brain.Robot as LineAndWallDetectorRobot;

        public override void Tick()
        {
            base.Tick();

            AccelerateIfStopped();

            double distance = Robot.RightWallSensor.GetDistance();
            if (distance < LineAndWallDetectorRobot.WallSensorMaxDistance)
            {
                if (distance > 10.0)
                    Robot.Turn = 5.0;
                else
                    Robot.Turn = -5.0;
            }
            else
            {
                // Wall lost, turning
                Robot.Turn = 5;
            }
        }

        private void AccelerateIfStopped()
        {
            if (this.Brain.Robot.Speed < 1.0)
                this.Brain.Robot.Acceleration = 1.0;
            else
                this.Brain.Robot.Acceleration = 0.0;
        }
    }

}