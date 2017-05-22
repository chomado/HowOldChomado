using HowOldChomado.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace HowOldChomado.Views
{
    public partial class GamePage : ContentPage
    {
        private static SKTypeface NotoFont { get; }

        static GamePage()
        {
            var s = typeof(GamePage).GetTypeInfo().Assembly.GetManifestResourceStream("HowOldChomado.Fonts.NotoSansCJKjp-Regular.otf");
            NotoFont = SKTypeface.FromStream(new SKManagedStream(s));
        }

        public GamePage()
        {
            InitializeComponent();
            ((INotifyPropertyChanged)this.BindingContext).PropertyChanged += this.GamePage_PropertyChanged;
        }

        private void GamePage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.canvas.InvalidateSurface();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            ((ListView)sender).SelectedItem = null;
        }

        private void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var viewModel = (GamePageViewModel)this.BindingContext;
            var surface = e.Surface;
            var info = e.Info;
            var canvas = surface.Canvas;
            var textScale = info.Width / this.Width;

            canvas.Clear();

            var drawBitmapArea = (top: 0, left: 0, width: 0, height: 0);
            if (viewModel.Picture == null)
            {
                return;
            }

            var bitmap = SKBitmap.Decode(viewModel.Picture);
            if (bitmap.Width > info.Width || bitmap.Height > info.Height)
            {
                // サイズ調整の必要がある
                if (bitmap.Width <= bitmap.Height)
                {
                    // 縦長
                    var height = info.Height;
                    var width = (int)(bitmap.Width * ((double)info.Height / bitmap.Height));
                    drawBitmapArea = (top: 0, left: (info.Width - width) / 2, width: width, height: height);
                }
                else
                {
                    // 横長
                    var width = info.Width;
                    var height = (int)(bitmap.Height * ((double)info.Width / bitmap.Width));
                    drawBitmapArea = (top: (info.Height - height) / 2, left: 0, width: width, height: height);
                }
            }
            else
            {
                drawBitmapArea = (top: (info.Height - bitmap.Height) / 2,
                    left: (info.Width - bitmap.Width) / 2,
                    width: bitmap.Width,
                    height: bitmap.Height);
            }

            canvas.DrawBitmap(bitmap,
                new SKRect(drawBitmapArea.left, drawBitmapArea.top, drawBitmapArea.left + drawBitmapArea.width, drawBitmapArea.top + drawBitmapArea.height));

            var scale = (widthScale: (float)drawBitmapArea.width / bitmap.Width, heightScale: (float)drawBitmapArea.height / bitmap.Height);

            if (viewModel.FaceDetectionResults == null)
            {
                return;
            }

            foreach (var r in viewModel.FaceDetectionResults)
            {
                var faceArea = (left: r.FaceDetectionResult.Rectangle.Left * scale.widthScale + drawBitmapArea.left,
                    top: r.FaceDetectionResult.Rectangle.Top * scale.heightScale + drawBitmapArea.top,
                    right: (r.FaceDetectionResult.Rectangle.Left + r.FaceDetectionResult.Rectangle.Width) * scale.widthScale + drawBitmapArea.left,
                    bottom: (r.FaceDetectionResult.Rectangle.Top + r.FaceDetectionResult.Rectangle.Height) * scale.heightScale + drawBitmapArea.top);

                var color = r.IsWinner ? new SKColor(255, 255, 0) : new SKColor(0, 255, 255);
                canvas.DrawRect(new SKRect(faceArea.left, faceArea.top, faceArea.right, faceArea.bottom), new SKPaint
                {
                    StrokeWidth = 5,
                    IsStroke = true,
                    Color = color,
                });

                var textPaint = new SKPaint
                {
                    Typeface = NotoFont,
                    TextSize = (float)(Device.GetNamedSize(NamedSize.Medium, typeof(Label)) * textScale),
                    Color = color,
                };

                var messages = (name: r.Player?.DisplayName ?? "no name",
                    age: $"実年齢: {r.Player?.Age ?? 0}才",
                    score: $"判定結果: {r.FaceDetectionResult.Age}才({r.Diff}才)");

                var centerY = faceArea.top + (faceArea.bottom - faceArea.top) / 2;
                if (centerY > drawBitmapArea.height / 2)
                {
                    // 上側にテキストを描画する
                    canvas.DrawText(messages.name, faceArea.left, faceArea.top - textPaint.TextSize * 3 - 2, textPaint);
                    canvas.DrawText(messages.age, faceArea.left, faceArea.top - textPaint.TextSize * 2 - 2, textPaint);
                    canvas.DrawText(messages.score, faceArea.left, faceArea.top - textPaint.TextSize * 1 - 2, textPaint);
                }
                else
                {
                    // 下側にテキストを描画する
                    canvas.DrawText(messages.name, faceArea.left, faceArea.bottom + textPaint.TextSize * 1 - 2, textPaint);
                    canvas.DrawText(messages.age, faceArea.left, faceArea.bottom + textPaint.TextSize * 2 - 2, textPaint);
                    canvas.DrawText(messages.score, faceArea.left, faceArea.bottom + textPaint.TextSize * 3 - 2, textPaint);
                }
            }
        }
    }
}
