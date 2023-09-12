using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Num = System.Numerics;
using ImGuiNET;
using MonoProject.ImGuiComponent;
using System.Text;

namespace MonoProject.ImGuiComponent;

    static class ImGuiTools
    {
   
   public static byte[] CropByte(byte[] arr)
    {
        int i = arr.Length-1;
        while(arr[i] == 0) --i;
        byte[] narr = new byte[i+1];
        Array.Copy(arr, narr, i+1);
        return narr;
    }
    public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
		{
			var texture = new Texture2D(device, width, height);

			Color[] data = new Color[width * height];
			for(var pixel = 0; pixel < data.Length; pixel++)
			{
				data[pixel] = paint(pixel);
			}

			texture.SetData(data);

			return texture;
		}
}