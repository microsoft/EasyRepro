using System;
using System.Drawing;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static class RelativePositions
    {
        public static Func<Point> Above(this IWebElement outer, IWebElement inner) =>
            () =>
            {
                int x = outer.Size.Width / 2;
                int y = (inner.Location.Y - outer.Location.Y) / 2;
                return new Point(x, y);
            };

        public static Func<Point> Below(this IWebElement outer, IWebElement inner) =>
            () =>
            {
                int x = outer.Size.Width / 2;
                var outerEnd = outer.Location + outer.Size;
                var innerEnd = inner.Location + inner.Size;
                int dBelow = outerEnd.Y - innerEnd.Y;
                int y = innerEnd.Y + dBelow / 2;
                return new Point(x, y);
            };

        public static Func<Point> LeftTo(this IWebElement outer, IWebElement inner) =>
            () =>
            {
                int x = (inner.Location.X - outer.Location.X) / 2;
                int y = outer.Size.Height / 2;
                return new Point(x, y);
            };

        public static Func<Point> RightTo(this IWebElement outer, IWebElement inner) =>
            () =>
            {
                var outerEnd = outer.Location + outer.Size;
                var innerEnd = inner.Location + inner.Size;
                int dRight = outerEnd.X - innerEnd.X;
                int x = innerEnd.X + dRight / 2;
                int y = outer.Size.Height / 2;
                return new Point(x, y);
            };
    }
}