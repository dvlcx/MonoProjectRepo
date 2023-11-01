using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;
using System.Numerics;
using ImGuiNET;
using System.Text;

namespace MonoProject;

    static class Tools
    {
   
    public static byte[] CropByte(byte[] arr)
    {
        if (arr.Length == 0) return arr;
        int i = arr.Length-1;
        if(arr[i] == 0)
        {
            while(arr[i] == 0)
            { 
                --i;
                if(i < 0) return Array.Empty<byte>();
            }
            
        }
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

    public static Microsoft.Xna.Framework.Vector3 ToXnaVector(System.Numerics.Vector3 vin)
    {
        return new Microsoft.Xna.Framework.Vector3(vin.X, vin.Y, vin.Z);
    }

    public static System.Numerics.Vector3 ToSystemVector(Microsoft.Xna.Framework.Vector3 vin)
    {
        return new System.Numerics.Vector3(vin.X, vin.Y, vin.Z);
    }
}  
