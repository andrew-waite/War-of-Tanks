using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TerrainTest.Utils;

namespace FinalGame.Utils
{
    class TerrainModifier
    {
       /* public static void DestroyTexture(Rectangle rectangle, Texture2D texture, Color[] textureColorMap, bool addTerrain)
        {
            int areaLeft = Math.Max(0, rectangle.Left);
            int areaRight = Math.Min(texture.Width, rectangle.Right);

            int areaTop = Math.Max(0, rectangle.Top);
            int areaBottom = Math.Min(texture.Height, rectangle.Bottom);
            for (int i = areaLeft; i < areaRight; i++)
            {
                for (int j = areaTop; j < areaBottom; j++)
                {
                    if (addTerrain)
                        textureColorMap[i + j * texture.Width] = Color.Brown;
                    else
                        textureColorMap[i + j * texture.Width] = Color.Transparent;
                }
            }

            texture.SetData(textureColorMap);
        }*/

        public static void DestroyCircle(Circle circle, Texture2D texture, Color[] textureColorMap)
        {
            int areaLeft = Math.Max(0, (int)(circle.getX() - circle.getRadius()));
            int areaRight = Math.Min(texture.Width, (int)(circle.getX() + circle.getRadius()));

            int areaTop = Math.Max(0, (int)(circle.getY() - circle.getRadius()));
            int areaBottom = Math.Min(texture.Height, (int)(circle.getY() + circle.getRadius()));

            for (int i = areaLeft; i < areaRight; i++)
            {
                for (int j = areaTop; j < areaBottom; j++)
                {
                    if (circle.contains(new Point(i, j)))
                    {
                        int dataIndex = i + j * texture.Width;

                        if (textureColorMap[dataIndex].A != 0)
                        {
                            textureColorMap[i + j * texture.Width] = Color.Transparent;
                        }
                    }
                }
            }

            texture.SetData(textureColorMap);
        }

        public static void RebuildTerrain(Circle circle, Texture2D texture, Color[] textureColorMap, Color rebuildColor)
        {
            int areaLeft = Math.Max(0, (int)(circle.getX() - circle.getRadius()));
            int areaRight = Math.Min(texture.Width, (int)(circle.getX() + circle.getRadius()));

            int areaTop = Math.Max(0, (int)(circle.getY() - circle.getRadius()));
            int areaBottom = Math.Min(texture.Height, (int)(circle.getY() + circle.getRadius()));

            for (int i = areaLeft; i < areaRight; i++)
            {
                for (int j = areaTop; j < areaBottom; j++)
                {
                    if (circle.contains(new Point(i, j)))
                    {
                        textureColorMap[i + j * texture.Width] = rebuildColor;
                    }
                }
            }

            texture.SetData(textureColorMap);
        }

        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            //Sourced from: http://stackoverflow.com/questions/7292870/per-pixel-collision-code-explanation
            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                            (y - rectangleA.Top) * rectangleA.Width];

                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }
}
