using System;
using System.Windows.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhotoEffectEx
{
    public class Effects
    {

        /////////////////////////////////////////////////////////// 
        // 白黒化エフェクト
        /////////////////////////////////////////////////////////// 

        public static WriteableBitmap bw(BitmapImage bmp)
        {
            WriteableBitmap wp = new WriteableBitmap(bmp);
            int[] result = new int[wp.Pixels.Length];

            for (int pixel = 0; pixel < wp.Pixels.Length; pixel++)
            {
                uint color1 = (uint)wp.Pixels[pixel];

                uint A = (color1 >> 24);
                uint R = ((color1 >> 16) & 0xFF);
                uint G = ((color1 >> 8) & 0xFF);
                uint B = ((color1) & 0xFF);

                uint Gr = (306 * R + 601 * G + 117 * B) >> 10;
                result[pixel] = (int)((A << 24) | (Gr << 16) | (Gr << 8) | Gr);
            }

            Buffer.BlockCopy(result, 0, wp.Pixels, 0, result.Length * 4);
            return wp;
        }

        public static WriteableBitmap bw8(BitmapImage bmp)
        {
            WriteableBitmap wp = new WriteableBitmap(bmp);
            int[] result = new int[wp.Pixels.Length];

            for (int pixel = 0; pixel < wp.Pixels.Length; pixel++)
            {
                uint color1 = (uint)wp.Pixels[pixel];

                uint A = (color1 >> 24);
                uint R = ((color1 >> 16) & 0xFF);
                uint G = ((color1 >> 8) & 0xFF);
                uint B = ((color1) & 0xFF);

                uint Gr = (306 * R + 601 * G + 117 * B) >> 10;
                if (Gr < 255) Gr = (Gr >> 5) << 5;

                result[pixel] = (int)((A << 24) | (Gr << 16) | (Gr << 8) | Gr);
            }

            Buffer.BlockCopy(result, 0, wp.Pixels, 0, result.Length * 4);
            return wp;
        }


        /////////////////////////////////////////////////////////// 
        //セピア調化エフェクト
        /////////////////////////////////////////////////////////// 

        public static WriteableBitmap sepia(BitmapImage bmp)
        {
            WriteableBitmap wp = new WriteableBitmap(bmp);
            int[] result = new int[wp.Pixels.Length];

            for (int pixel = 0; pixel < wp.Pixels.Length; pixel++)
            {
                uint color1 = (uint)wp.Pixels[pixel];

                uint A = (color1 >> 24);
                uint R = ((color1 >> 16) & 0xFF);
                uint G = ((color1 >> 8) & 0xFF);
                uint B = ((color1) & 0xFF);

                uint Gr = (306 * R + 601 * G + 117 * B) >> 10;
                R = (Gr * 240) >> 8;
                G = (Gr * 200) >> 8;
                B = (Gr * 145) >> 8;

                result[pixel] = (int)((A << 24) | (R << 16) | (G << 8) | B);
            }

            Buffer.BlockCopy(result, 0, wp.Pixels, 0, result.Length * 4);
            return wp;
        }


        /////////////////////////////////////////////////////////// 
        // コントラスト調整 エフェクト
        /////////////////////////////////////////////////////////// 

        public static WriteableBitmap Contrast(BitmapImage bmp, int contrast)
        {
            WriteableBitmap wp = new WriteableBitmap(bmp);
            int[] result = new int[wp.Pixels.Length];

            for (int pixel = 0; pixel < wp.Pixels.Length; pixel++)
            {

                int color1 = (int)wp.Pixels[pixel];

                int A1 = (color1 >> 24);
                int R1 = ((color1 >> 16) & 0xFF);
                int G1 = ((color1 >> 8) & 0xFF);
                int B1 = ((color1) & 0xFF);

                int A = A1;
                int R = 0;
                int G = 0;
                int B = 0;

                int BR = 0;

                // コントラスト
                int bank = 255 * 1024 / (255 - 2 * contrast);

                if (R1 > 255 - contrast - BR) R = 255;
                else if (R1 < contrast - BR) R = 0;
                else R = ((R1 - contrast + BR) * bank) >> 10;

                if (G1 > 255 - contrast - BR) G = 255;
                else if (G1 < contrast - BR) G = 0;
                else G = ((G1 - contrast + BR) * bank) >> 10;

                if (B1 > 255 - contrast - BR) B = 255;
                else if (B1 <= contrast - BR) B = 0;
                else B = ((B1 - contrast + BR) * bank) >> 10;

                result[pixel] =
                    ((int)A << 24) | ((int)R << 16) | ((int)G << 8) | (int)B;

            }

            Buffer.BlockCopy(result, 0, wp.Pixels, 0, result.Length * 4);
            return wp;
        }
    }
}
