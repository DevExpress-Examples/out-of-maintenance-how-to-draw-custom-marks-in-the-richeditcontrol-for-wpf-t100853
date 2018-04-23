#region #customdrawmark
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Layout.Export;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomMarkDraw {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            richEditControl1.LoadDocument("Test.docx");           
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Document doc = richEditControl1.Document;
            // Create a custom mark at the caret position and attach arbitrary data to the mark.
            // In this example the data specifies the color that will be used to draw the mark.
            CustomMark m = doc.CustomMarks.Create(doc.Selection.Start, Brushes.Orange);
        }

        private void richEditControl1_CustomMarkDraw(object sender, DevExpress.Xpf.RichEdit.CustomMarkDrawEventArgs e) {
            if (!richEditControl1.IsLoaded)
                return;
            Canvas surface = richEditControl1.Template.FindName("Surface", richEditControl1) as Canvas;
            if (!surface.IsLoaded)
                return;
            GeneralTransform transform = surface.TransformToVisual(richEditControl1);
            RectangleGeometry clip = new RectangleGeometry(new Rect(transform.Transform(new Point(0, 0)), surface.RenderSize));
            richEditCanvas.Children.Clear();
            richEditCanvas.Clip = clip;
            foreach (CustomMarkVisualInfo info in e.VisualInfoCollection)
            {
                Document doc = richEditControl1.Document;
                CustomMark mark = doc.CustomMarks.GetByVisualInfo(info);
                // Get a brush associated with the mark.
                Brush curBrush = info.UserData as Brush;
                // Use a different brush to draw custom marks located above the caret.
                if (mark.Position < doc.Selection.Start) curBrush = Brushes.Green;
                Rectangle rect = new Rectangle();
                rect.Width = 2;
                rect.Height = info.Bounds.Height;
                rect.Fill = curBrush;
                Canvas.SetLeft(rect, info.Bounds.X);
                Canvas.SetTop(rect, info.Bounds.Y);
                richEditCanvas.Children.Add(rect);
            }
        }
    }
}
#endregion #customdrawmark