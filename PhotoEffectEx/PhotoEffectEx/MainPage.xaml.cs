using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace PhotoEffectEx
{
    public partial class MainPage : PhoneApplicationPage
    {

        ////①a 写真を読み込むOSの機能を呼び出すオブジェクトを作成します
        PhotoChooserTask task = new PhotoChooserTask();

        // 【処理０】 コンストラクター
        public MainPage()
        {
            InitializeComponent();
            ////⓪ 追加した写真などをすべて消しておきます（ネタバレ防止)
            PictureFrame.Visibility = System.Windows.Visibility.Collapsed;

            ////②-1 写真を読込終わった時の飛び先を指定します
            task.Completed += new EventHandler<PhotoResult>(task_Completed);

            ////②-2 トリミング機能を追加します
            task.PixelHeight = 440;
            task.PixelWidth = 440;

            ////②-3 カメラ撮影機能を追加します
            task.ShowCamera = true;
        }

        //////////////////////////////////////////////////////////
        // 【処理1-1】 画像選択機能呼び出し
        //////////////////////////////////////////////////////////
        private void btnLoad_Click(object sender, EventArgs e)
        {
            ////①b 写真を読み込む機能を呼び出します。
            task.Show();

        }

        //////////////////////////////////////////////////////////
        // 【処理1-2】 画像読み込み処理
        //////////////////////////////////////////////////////////
        void task_Completed(object sender, PhotoResult e)
        {

            ////③ 画像がなかったら終了
            if (e.TaskResult != TaskResult.OK)
                return;

            ////④ 選択した画像をimageコントロールに表示する
            BitmapImage bmp = new BitmapImage();
            bmp.SetSource(e.ChosenPhoto);
            imgPhoto.Source = bmp;

            ///////////////////////////////////////////////////////
            // 【処理２】 画像加工処理
            ///////////////////////////////////////////////////////

            ////⑤ 画像 読み込み後の処理
            PictureFrame.Visibility = System.Windows.Visibility.Visible;

            ////アニメーションの実行
            Storyboard1.Begin();

            ///////////////////////////////////////////////////////
            //// 参考：画像加工処理
            ///////////////////////////////////////////////////////

            // 画像処理例 ① 画像の白黒化
            //imgPhoto.Source = Effects.bw(bmp);　

            // 画像処理例 ① 画像の白黒化（８諧調化）
            //imgPhoto.Source = Effects.bw8(bmp);　

            // 画像処理例 ② 画像のセピア化
            imgPhoto.Source = Effects.sepia(bmp); 

            // 画像処理例 ③ コントラストを強く
            //imgPhoto.Source = Effects.Contrast(bmp, 50);  

        }

        ///////////////////////////////////////////////////////
        // 【処理3】 画像保存
        ///////////////////////////////////////////////////////
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (imgPhoto.Source == null)
            {
                MessageBox.Show("先に画像を選んでください");
                return;
            }
            ////⑥画像保存処理
            ////Grid から Jpeg 保存用ストリームを作成
            WriteableBitmap wp = new WriteableBitmap(PictureFrame, null);
            MemoryStream stream = new MemoryStream();
            wp.SaveJpeg(stream, wp.PixelWidth, wp.PixelHeight, 0, 100);

            ////PictureHub に保存する
            using (MediaLibrary lib = new MediaLibrary())
            {
                lib.SavePicture("PhotoEffectEx", stream.ToArray());
                MessageBox.Show("保存しました");
            }
        }

        ///////////////////////////////////////////////////////
        // 【処理4】 バージョン情報
        ///////////////////////////////////////////////////////
        private void version_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 1.0");
        }

        ///////////////////////////////////////////////////////
        // 【処理】 オブジェクトを移動する方法
        ///////////////////////////////////////////////////////
        private void image_ManipulationDelta(
          object sender, ManipulationDeltaEventArgs e)
        {
            //選択されたオブジェクトを取得します
            FrameworkElement obj = sender as FrameworkElement;

            //これまでの移動情報を取得します
            TranslateTransform tt;
            if (obj.RenderTransform as TranslateTransform == null)
                tt = new TranslateTransform();
            else
                tt = obj.RenderTransform as TranslateTransform;

            //移動情報をドラッグ分だけ移動します
            tt.X += e.DeltaManipulation.Translation.X;
            tt.Y += e.DeltaManipulation.Translation.Y;

            //移動情報を適用します
            obj.RenderTransform = tt;
        }
    }
}