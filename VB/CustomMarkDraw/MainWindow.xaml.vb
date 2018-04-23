#Region "#customdrawmark"
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Layout.Export
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Shapes

Namespace CustomMarkDraw
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            richEditControl1.LoadDocument("Test.docx")
        End Sub

        Private Sub barButtonItem1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            Dim doc As Document = richEditControl1.Document
            ' Create a custom mark at the caret position and attach arbitrary data to the mark.
            ' In this example the data specifies the color that will be used to draw the mark.
            Dim m As CustomMark = doc.CustomMarks.Create(doc.Selection.Start, Brushes.Orange)
        End Sub

        Private Sub richEditControl1_CustomMarkDraw(ByVal sender As Object, ByVal e As DevExpress.Xpf.RichEdit.CustomMarkDrawEventArgs)
            If Not richEditControl1.IsLoaded Then
                Return
            End If
            Dim surface As Canvas = TryCast(richEditControl1.Template.FindName("Surface", richEditControl1), Canvas)
            If Not surface.IsLoaded Then
                Return
            End If
            Dim transform As GeneralTransform = surface.TransformToVisual(richEditControl1)

            Dim clip_Renamed As New RectangleGeometry(New Rect(transform.Transform(New Point(0, 0)), surface.RenderSize))
            richEditCanvas.Children.Clear()
            richEditCanvas.Clip = clip_Renamed
            For Each info As CustomMarkVisualInfo In e.VisualInfoCollection
                Dim doc As Document = richEditControl1.Document
                Dim mark As CustomMark = doc.CustomMarks.GetByVisualInfo(info)
                ' Get a brush associated with the mark.
                Dim curBrush As Brush = TryCast(info.UserData, Brush)
                ' Use a different brush to draw custom marks located above the caret.
                If mark.Position < doc.Selection.Start Then
                    curBrush = Brushes.Green
                End If
                Dim rect As New Rectangle()
                rect.Width = 2
                rect.Height = info.Bounds.Height
                rect.Fill = curBrush
                Canvas.SetLeft(rect, info.Bounds.X)
                Canvas.SetTop(rect, info.Bounds.Y)
                richEditCanvas.Children.Add(rect)
            Next info
        End Sub
    End Class
End Namespace
#End Region ' #customdrawmark