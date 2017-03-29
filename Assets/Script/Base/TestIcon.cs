using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Drawing;
using System.IO;

public class TestIcon : MonoBehaviour
{
    public Icon localImage;

    public Stream stream ;

    byte[] bytes;

    Texture2D tt ;

    System.Drawing.Image loIma;

    public UnityEngine.UI.Image pp;

     IEnumerator GetIcon()
    {
        yield return null;
        localImage  =   System.Drawing.Icon.ExtractAssociatedIcon("C:/Users/plance/Pictures/screen.png") ;
        localImage.ToBitmap().GetHbitmap();
        localImage.Save(stream);
        stream.Write(bytes, 0, (int)stream.Length);
        tt.LoadImage(bytes);
        pp.material.mainTexture = tt as Texture;
    }


    public void Clicked()
    {
        //StartCoroutine(GetIcon());
        localImage = System.Drawing.Icon.ExtractAssociatedIcon("C:/Users/plance/Pictures/screen.png");
        localImage.ToBitmap().GetHbitmap();
        localImage.Save(stream);
        stream.Write(bytes, 0, (int)stream.Length);
        tt.LoadImage(bytes);
        pp.material.mainTexture = tt as Texture;
    }
}
