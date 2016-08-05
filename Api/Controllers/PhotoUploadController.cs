using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    public class PhotoUploadController : ApiController
    {
        [HttpPost]
        public IHttpActionResult PhotoUpload(MessageImageDto dto)
        {
            try
            {
                Image image;
                MemoryStream ms = new MemoryStream(dto.imageData);
                image = Image.FromStream(ms);
                Guid imageid = Guid.NewGuid();

                string uploadPath = HttpContext.Current.Server.MapPath("/Content/Photo/Original/");
                Bitmap bt1 = ResizeImage(image, Convert.ToInt32(Math.Ceiling(image.Width * 0.3)), Convert.ToInt32(Math.Ceiling(image.Height * 0.3)));
                bt1.Save(uploadPath + imageid + ".jpg");

                string thumbnailupload = HttpContext.Current.Server.MapPath("/Content/Photo/Thumbnail/");
                Bitmap bt = ResizeImage(image, 300, 300);
                bt.Save(thumbnailupload + imageid + ".jpg");

                var fileNameOrginal = "/Content/Photo/Original/" + imageid + ".jpg";
                var fileNameThumbnail = "/Content/Photo/Thumbnail/" + imageid + ".jpg";

                return Json(fileNameThumbnail);
            }
            catch (Exception)
            {
                return Json("test");
            }

        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
