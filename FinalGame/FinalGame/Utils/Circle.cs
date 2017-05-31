using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TerrainTest.Utils
{
    public class Circle
    {
        Vector2 position;
        float radius;
        float radiusSquared;


        public Circle(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
            this.radiusSquared = radius * radius;
        }

        public float getX()
        {
            return this.position.X;
        }

        public float getY()
        {
            return this.position.Y;
        }

        public float getRadius()
        {
            return this.radius;
        }

        public float getRadiusSquared()
        {
            return this.radiusSquared;
        }


        public bool contains(Point point)
        {
            float x = point.X - getX();
            float y = point.Y - getY();

            if(x * x + y * y <= getRadiusSquared())
            {
                return true;
            }
            return false;
        }

    }
}
