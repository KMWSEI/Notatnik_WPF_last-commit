using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace NotatnikWPF
{
    class Printing
    {
        private static FlowDocument createFlowDocument(string[] lines, double pageWidth)
        {
            FlowDocument fd = new FlowDocument();

            fd.Background = Brushes.White;
            fd.Foreground = Brushes.Black;

            fd.FontFamily = new FontFamily("Times New Roman");
            fd.FontStyle = FontStyles.Normal;
            fd.FontWeight = FontWeights.Normal;
            fd.FontSize = 12.0;

            fd.ColumnGap = 0;
            fd.ColumnWidth = pageWidth;

            foreach ( string line in lines)
            {
                Paragraph ph = new Paragraph(new Run(line));
                fd.Blocks.Add(ph);
            }
            return fd;
        }

        public static void PrintText(string[] lines)
        {
            PrintDialog printDialog = new PrintDialog();

            if(printDialog.ShowDialog() == true)
            {
                FlowDocument fd = createFlowDocument(lines, printDialog.PrintableAreaWidth);
                printDialog.PrintDocument((fd as IDocumentPaginatorSource).DocumentPaginator, fd.Name);
            }
        }

        public static void PrintText(string text)
        {
            string[] lines = text.Split('\n');
            for (int x = 0; x < lines.Length; x++)
                lines[x] = lines[x].TrimEnd('\r', ' ');
            PrintText(lines);
        }
    }
}
