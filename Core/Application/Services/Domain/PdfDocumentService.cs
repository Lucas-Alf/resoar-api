using System.Runtime.InteropServices;
using Application.Interfaces.Services.Domain;
using Docnet.Core;
using Docnet.Core.Models;
using SkiaSharp;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace Application.Services.Domain
{
    public class PdfDocumentService : IPdfDocumentService
    {
        /// <summary>
        /// Extract all the readable text present in the PDF file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ExtractText(byte[] file)
        {
            using (PdfDocument document = PdfDocument.Open(file))
            {
                string documentText = "";
                var textExtractor = NearestNeighbourWordExtractor.Instance;
                foreach (Page page in document.GetPages())
                {
                    documentText += (String.Join(" ", page.GetWords(textExtractor).Select(x => x.Text)) + " ");
                }

                // Replaces the broken words
                // Ex: "exem- ple" to "exemple"
                return documentText.Replace("- ", "").Trim();
            }
        }

        /// <summary>
        /// Generates a thumbnail image in webp format from a PDF file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public byte[] GenerateThumbnail(byte[] file)
        {
            using (var docNet = DocLib.Instance)
            using (var docReader = docNet.GetDocReader(file, new PageDimensions(1080, 1920)))
            using (var pageReader = docReader.GetPageReader(0))
            {
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();
                var rawBytes = pageReader.GetImage();

                using (var inStream = new MemoryStream(rawBytes))
                using (var imgStream = new SKManagedStream(inStream))
                using (var skData = SKData.Create(inStream))
                {
                    var bitmap = PixelArrayToBitmap(rawBytes, width, height);
                    var pixmap = bitmap.PeekPixels();
                    var options = new SKWebpEncoderOptions(compression: SKWebpEncoderCompression.Lossy, quality: 50);
                    var webp = pixmap.Encode(options);
                    return webp.ToArray();
                }
            }
        }

        /// <summary>
        /// Transform the pixel array in B-R-G-A format to a Bitmap with white background.
        /// </summary>
        /// <param name="pixelArray"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static SKBitmap PixelArrayToBitmap(byte[] pixelArray, int width, int height)
        {
            // Prevent garbage collector to move the pixelArray
            GCHandle gcHandle = GCHandle.Alloc(pixelArray, GCHandleType.Pinned);

            // The desired image size
            SKImageInfo info = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul);

            // Create the surface
            SKSurface surface = SKSurface.Create(info);

            // Get the canvas for drawing
            SKCanvas canvas = surface.Canvas;

            // Draw the white background
            canvas.Clear(SKColors.White);

            // Create a paint object so that drawing can happen at a higher resolution
            SKPaint paint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High
            };

            // The result bitmap
            SKBitmap bitmap = new SKBitmap();
            IntPtr ptr = gcHandle.AddrOfPinnedObject();
            int rowBytes = info.RowBytes;

            // Set pixels on the bitmap
            bitmap.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });

            // draw the source bitmap over the white background
            canvas.DrawBitmap(bitmap, info.Rect, paint);

            // create an image for saving/drawing
            canvas.Flush();
            SKImage finalImage = surface.Snapshot();

            return SKBitmap.FromImage(finalImage);
        }
    }
}