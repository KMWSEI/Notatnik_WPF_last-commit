using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NotatnikWPF
{
    public class Fonts
    {
        public FontFamily Family { get; set; }

        public string FamilyName
        {
            get
            {
                return Family.ToString();
            }
        }

        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public TextDecorationCollection TextDecorations { get; set; }
        public double Size { get; set; }
        public Color Color { get; set; }
        public Brush Brush {
            get
            {
                return new SolidColorBrush(Color);
            } }
        public static Fonts Default
        {
            get
            {
                return new Fonts()
                {
                    Family = new FontFamily("Times New Roman"),
                    Style = FontStyles.Normal,
                    Weight = FontWeights.Normal,
                    TextDecorations = null,
                    Size = 12,
                    Color = Colors.Black
                };
            }
        }
        
        public void ChangeTo(Control control)
        {
            control.FontFamily = Family;
            control.FontStyle = Style;
            control.FontWeight = Weight;
            control.FontSize = Size;
            control.Foreground = Brush;
        }

        public void ChangeTo(TextBox textbox)
        {
            ChangeTo(textbox as Control);
            textbox.TextDecorations = TextDecorations;
        }

        public static Fonts ExtractFont(Control control)
        {
            Color color = Colors.Black;
            if (control.Foreground is SolidColorBrush)
                color = (control.Foreground as SolidColorBrush).Color;
            Fonts font = new Fonts()
            {
                Family = control.FontFamily,
                Style = control.FontStyle,
                Weight = control.FontWeight,
                TextDecorations = null,
                Size = control.FontSize,
                Color = color
            };
            if (control is TextBox) font.TextDecorations = (control as TextBox).TextDecorations;
            return font;
        }
        
        public static System.Drawing.Font ConvertToDrawingFont( Fonts font)
        {
            System.Drawing.FontStyle style = (font.Style == FontStyles.Italic) ? System.Drawing.FontStyle.Italic : System.Drawing.FontStyle.Regular;
            if (font.Weight == FontWeights.Bold) style |= System.Drawing.FontStyle.Bold;
            if (font.TextDecorations.Contains(System.Windows.TextDecorations.Underline[0])) style |= System.Drawing.FontStyle.Underline;
            if (font.TextDecorations.Contains(System.Windows.TextDecorations.Strikethrough[0])) style |= System.Drawing.FontStyle.Strikeout;
            System.Drawing.Font newFont = new System.Drawing.Font(font.FamilyName, (int)font.Size, style);
            return newFont;
        }

        public static Fonts ConvertFromDrawingFont(System.Drawing.Font SDFont, System.Drawing.Color SDColor)
        {
            Fonts font = new Fonts();
            font.Family = new FontFamily(SDFont.FontFamily.Name);
            font.Style = SDFont.Italic ? FontStyles.Italic : FontStyles.Normal;
            font.Weight = SDFont.Bold ? FontWeights.Bold : FontWeights.Regular;
            font.TextDecorations = new TextDecorationCollection();
            if (SDFont.Underline)
                font.TextDecorations.Add(System.Windows.TextDecorations.Underline);
            if (SDFont.Strikeout)
                font.TextDecorations.Add(System.Windows.TextDecorations.Strikethrough);
            font.Size = SDFont.Size;
            font.Color = Convert(SDColor);
            return font;
        }

        public static bool ChooseFont(ref Fonts font)
        {
            using (System.Windows.Forms.FontDialog FontDialog = new System.Windows.Forms.FontDialog())
            {
                FontDialog.ShowColor = true;
                FontDialog.ShowEffects = true;
                FontDialog.Font = ConvertToDrawingFont(font);
                bool result = FontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK;

                if (result) font = ConvertFromDrawingFont(FontDialog.Font, FontDialog.Color);

                return result;
            }
        }

        public static System.Drawing.Color Convert(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Color Convert(System.Drawing.Color color)
        {
            return new Color()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }
    }
}
