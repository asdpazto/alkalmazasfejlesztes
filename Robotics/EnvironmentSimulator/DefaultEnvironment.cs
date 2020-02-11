﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Environment
{
    public class DefaultEnvironment : IEnvironment
    {
        private readonly Map map;

        public DefaultEnvironment(Map map)
        {
            this.map = map;
        }

        public event OnTickDelegate OnTick;

        public void Tick()
        {
            OnTick?.Invoke();
        }

        public IEnumerable<int> Scan(Point p1, Point p2)
        {
            return map.GetValuesAlongLine(
                (int)Math.Round(p1.X),
                (int)Math.Round(p1.Y),
                (int)Math.Round(p2.X),
                (int)Math.Round(p2.Y));
        }

        public Point GetLocationOfRelativePoint(LocOri baseLocOri, double relativeDirection, double distance)
        {
            // Note: direction 0.0 is upwards, X and Y coordinates are increasing rightwards and downwards.
            return new Point()
            {
                X = baseLocOri.Location.X + (int)Math.Round(Math.Sin(Helpers.ToRad(baseLocOri.Orientation + relativeDirection)) * distance),
                Y = baseLocOri.Location.Y - (int)Math.Round(Math.Cos(Helpers.ToRad(baseLocOri.Orientation + relativeDirection)) * distance)
            };
        }

        public IEnumerable<int> ScanRelative(LocOri baseLocOri,
            double relativeDirection1, double distance1,
            double relativeDirection2, double distance2)
        {
            return Scan(
                GetLocationOfRelativePoint(baseLocOri, relativeDirection1, distance1),
                GetLocationOfRelativePoint(baseLocOri, relativeDirection2, distance2)
                );
        }
    }
}
