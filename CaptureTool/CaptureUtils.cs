using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CaptureTool {
    class CaptureUtils {

        public static void SaveImage(string fileName, int quality) {
            //画像ファイルを読み込む
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fileName);

            //EncoderParameterオブジェクトを1つ格納できる
            //EncoderParametersクラスの新しいインスタンスを初期化
            //ここでは品質のみ指定するため1つだけ用意する
            System.Drawing.Imaging.EncoderParameters eps =
                new System.Drawing.Imaging.EncoderParameters(1);
            //品質を指定
            System.Drawing.Imaging.EncoderParameter ep = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
            //EncoderParametersにセットする
            eps.Param[0] = ep;

            //イメージエンコーダに関する情報を取得する
            System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

            //新しいファイルの拡張子を取得する
            string ext = ici.FilenameExtension.Split(';')[0];
            ext = System.IO.Path.GetExtension(ext).ToLower();
            //保存するファイル名を決定（拡張子を変える）
            string saveName = System.IO.Path.ChangeExtension(fileName, ext);

            //保存する
            bmp.Save(saveName, ici, eps);

            bmp.Dispose();
            eps.Dispose();
        }

        public static void SaveAsJpeg(Bitmap bmp, string path, int quality) {

            //EncoderParameterオブジェクトを1つ格納できる
            //EncoderParametersクラスの新しいインスタンスを初期化
            //ここでは品質のみ指定するため1つだけ用意する
            System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters(1);
            //品質を指定
            System.Drawing.Imaging.EncoderParameter ep = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
            //EncoderParametersにセットする
            eps.Param[0] = ep;

            //イメージエンコーダに関する情報を取得する
            System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

            //保存する
            bmp.Save(path, ici, eps);
            eps.Dispose();
        }

        //MimeTypeで指定されたImageCodecInfoを探して返す
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mineType) {
            //GDI+ に組み込まれたイメージ エンコーダに関する情報をすべて取得
            System.Drawing.Imaging.ImageCodecInfo[] encs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            //指定されたMimeTypeを探して見つかれば返す
            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs) {
                if (enc.MimeType == mineType) {
                    return enc;
                }
            }
            return null;
        }

        //ImageFormatで指定されたImageCodecInfoを探して返す
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(System.Drawing.Imaging.ImageFormat f) {
            System.Drawing.Imaging.ImageCodecInfo[] encs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs) {
                if (enc.FormatID == f.Guid) {
                    return enc;
                }
            }
            return null;
        }

    }
}
