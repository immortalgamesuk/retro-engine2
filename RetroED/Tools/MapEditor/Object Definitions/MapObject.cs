﻿using System.Drawing;

namespace RetroED.Tools.MapEditor.Object_Definitions
{
    public class MapObject
    {
        public string Name, SpriteSheet;
        public int ID, SubType, X, Y, Width, Height, PivotX, PivotY, Flip;

        public MapObject()
        {

        }

        public MapObject(string name, int id, int sID, string sheet, int Sheetxpos, int Sheetypos, int width, int height)
        {
            Name = name;
            ID = id;
            SubType = sID;
            SpriteSheet = sheet;
            X = Sheetxpos;
            Y = Sheetypos;
            Width = width;
            Height = height;
        }

        public MapObject(string name, int id, int sID, string sheet, int Sheetxpos, int Sheetypos, int width, int height, int pivotX, int pivotY, int flip)
        {
            Name = name;
            ID = id;
            SubType = sID;
            SpriteSheet = sheet;
            X = Sheetxpos;
            Y = Sheetypos;
            Width = width;
            Height = height;
            PivotX = pivotX;
            PivotY = pivotY;
            Flip = flip;
        }

        public Bitmap RenderObject(int RSDKver, string DataPath)
        {
            Bitmap b = RetroED.Properties.Resources.OBJ;
            if (!System.IO.File.Exists(DataPath + SpriteSheet))
            {
                b = RetroED.Properties.Resources.OBJ;
                return b;
            }
            switch (RSDKver)
            {
                case 3:
                    RSDKvRS.gfx g = new RSDKvRS.gfx(DataPath + SpriteSheet, false);
                    b = CropImage(g.gfxImage, new Rectangle(X, Y, Width, Height));
                    b.MakeTransparent(Color.FromArgb(255, 0, 0, 0));
                        break;
                case 2:
                    b = new Bitmap(DataPath + SpriteSheet, false);
                    b = CropImage(b, new Rectangle(X, Y, Width, Height));
                    b.MakeTransparent(Color.FromArgb(255, 255, 0, 255));
                    break;
                case 1:
                    b = new Bitmap(DataPath + SpriteSheet, false);
                    b = CropImage(b, new Rectangle(X, Y, Width, Height));
                    b.MakeTransparent(Color.FromArgb(255, 255, 0, 255));
                    break;
                case 0:
                    b = new Bitmap(DataPath + SpriteSheet, false);
                    b = CropImage(b, new Rectangle(X, Y, Width, Height));
                    b.MakeTransparent(Color.FromArgb(255, 255, 0, 255));
                    break;
            }
            return b;
        }

        public Bitmap RenderObject(string DataPath, Color Transparent)
        {
            Bitmap b = (Bitmap)Image.FromFile(DataPath + SpriteSheet).Clone();
            b.MakeTransparent(Transparent);
            b = CropImage(b, new Rectangle(X, Y, Width, Height));
            if (Flip == 1) { b.RotateFlip(RotateFlipType.RotateNoneFlipX); }
            if (Flip == 2) { b.RotateFlip(RotateFlipType.RotateNoneFlipY); }
            if (Flip == 3) { b.RotateFlip(RotateFlipType.RotateNoneFlipXY); }
            return b;
        }

        Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

    }
}
