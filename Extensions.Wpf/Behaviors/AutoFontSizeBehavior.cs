using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Extensions.Wpf.Behaviors
{
    public class AutoFontSizeBehavior : Behavior<TextBlock>
    {
        // TODO margin, padding, actual or not, enlarge as well
        protected override void OnAttached()
        {
            TextBlock textBlock = AssociatedObject;
            var panel = textBlock.Parent as Panel;
            if (panel != null)
            {
                textBlock.TextWrapping = TextWrapping.Wrap;
                Size size = new Size(panel.Width, panel.Height);
                //  textBlock.Measure(size);
                //Thickness margin = textBlock.Margin;
                Thickness padding = textBlock.Padding;
                var size1 = new Size(panel.Width - (padding.Left + padding.Right),
                    panel.Height - (padding.Top + padding.Bottom));
                //var size1 = new Size(padding.Left + padding.Right, padding.Top + padding.Bottom);
                //size -= size1;

                size = size1;
                if (double.IsNaN(size.Width))
                {
                     size.Width = 256;
               
                }
                Size desiredSize = textBlock.DesiredSize;
                Size renderSize = textBlock.RenderSize;
                var typeface = new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight,
                    textBlock.FontStretch);
                double fontSize = textBlock.FontSize;
                var i = fontSize;
                // Console.WriteLine("initial font size: " + fontSize);
                IEnumerable<WordSize> sizes;
                while (
                    (sizes =
                        MeasureWords(textBlock.Text, typeface, fontSize, textBlock.FlowDirection, textBlock.Foreground))
                        .Any(s => s.Size.Width > size.Width /*&& s.Size.Height > size.Height*/))
                {
                    fontSize--;
                    //  Console.WriteLine("reducing font size to: " + fontSize);
                }
                textBlock.FontSize = fontSize;
                if (textBlock.Text.ToLower() == "lana del rey")
                {
                }

                textBlock.Measure(size);
                var finalRect = new Rect(new Point(textBlock.Margin.Left, textBlock.Margin.Top), size);
                textBlock.Arrange(finalRect);
                while (textBlock.ActualHeight > size.Height)
                {
                    fontSize--;
                    textBlock.FontSize = fontSize;
                    textBlock.Measure(size);
                    finalRect = new Rect(new Point(textBlock.Margin.Left, textBlock.Margin.Top), size);
                    textBlock.Arrange(finalRect);
                }
                if (Math.Abs(i - textBlock.FontSize) > double.Epsilon   )
                {
                    Console.WriteLine("reduced font size from " + i + " to " + textBlock.FontSize);    
                }
                
            }
            base.OnAttached();
        }

        private static IEnumerable<WordSize> MeasureWords(string text, Typeface typeface, double fontSize,
            FlowDirection flowDirection, Brush foreground)
        {
            string pattern = @"\w+";
            pattern = @"[^ ]+";
            pattern = @"([^ ]* )*[^ ]+";
            pattern = @"([^ ]{1,3} )?[^ ]+";
            //pattern = @"[^ ]+( [^ ]+)?";

            var words = Regex.Matches(text, pattern).Cast<Match>().Select(s => s.Value);
            foreach (var word in words)
            {
                FormattedText formattedText = new FormattedText(word, CultureInfo.CurrentCulture,
                    flowDirection,
                    typeface,
                    fontSize, foreground);
                Size size = new Size(formattedText.Width, formattedText.Height);
                yield return new WordSize(word, size);
            }
        }

        #region Nested type: WordSize

        private struct WordSize
        {
            public WordSize(string word, Size size)
                : this()
            {
                Word = word;
                Size = size;
            }

            public string Word { get; private set; }

            public Size Size { get; private set; }

            public override string ToString()
            {
                return string.Format("Word: {0}, Size: {1}", Word, Size);
            }
        }

        #endregion
    }
}