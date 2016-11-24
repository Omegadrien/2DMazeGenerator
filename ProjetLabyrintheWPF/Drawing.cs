using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetLabyrintheWPF
{
    class Drawing
    {   
        /// <summary>
        /// That function will draw a line into a canva.
        /// </summary>
        /// <param name="x1">Position X1</param>
        /// <param name="y1">Position Y1</param>
        /// <param name="x2">Position X2</param>
        /// <param name="y2">Position Y2</param>
        /// <param name="thickness">line's thickness</param>
        /// <param name="color">line's color</param>
        /// <param name="canvas">canva where the line will be displayed</param>
        public void DrawLine(int x1, int y1, int x2, int y2, int thickness, Color color, Canvas canvas)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            SolidColorBrush Brush = new SolidColorBrush(color);
            line.StrokeThickness = thickness;
            line.Stroke = Brush;
            canvas.Children.Add(line);
        }

        /// <summary>
        /// That function will draw an image into a canva
        /// </summary>
        /// <param name="path">Uri path of the source image to display</param>
        /// <param name="x">Position top-left X</param>
        /// <param name="y">Position top-left Y</param>
        /// <param name="length">Image's length</param>
        /// <param name="canvas">canva where the image will be displayed</param>
        public void DrawImage(Uri path, int x, int y, int length, Canvas canvas)
        {
            Image img = new Image();
            img.Source = new BitmapImage(path);
            img.Height = length;
            img.Width = length;
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
            canvas.Children.Add(img);
        }

        /// <summary>
        /// That function will draw a text into a canva
        /// </summary>
        /// <param name="x">Position top-left X</param>
        /// <param name="y">Position top-left Y</param>
        /// <param name="text">Some text</param>
        /// <param name="fontSize">Font size</param>
        /// <param name="color">text's color</param>
        /// <param name="canvas">canva where the text will be displayed</param>
        public void DrawText(int x, int y, string text, int fontSize, Color color, Canvas canvas)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = fontSize;
            textBlock.Foreground = new SolidColorBrush(color);
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            canvas.Children.Add(textBlock);
        }

        /// <summary>
        /// That function will draw a square into a canva
        /// </summary>
        /// <param name="x">Position top-left X</param>
        /// <param name="y">Position top-left Y</param>
        /// <param name="length">square's length</param>
        /// <param name="color">quare's color</param>
        /// <param name="canvas">canva where the square will be displayed</param>
        public void DrawSquare(int x, int y, int length, Color color, Canvas canvas)
        {
            Rectangle Rectangle = new Rectangle();
            Rectangle.Height = length;
            Rectangle.Width = length;
            SolidColorBrush brush = new SolidColorBrush(color);
            Rectangle.Fill = brush;
            Canvas.SetLeft(Rectangle, x);
            Canvas.SetTop(Rectangle, y);
            canvas.Children.Add(Rectangle);
        }
    }
}
