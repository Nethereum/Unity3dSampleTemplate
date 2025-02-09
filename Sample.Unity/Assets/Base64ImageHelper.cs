using UnityEngine;
using UnityEngine.UI;
using System;

public static class ImageHelper
{
    public static void ApplyBase64Image(Component target, string base64Image)
    {
        if (string.IsNullOrEmpty(base64Image) || target == null)
        {
            Debug.LogError("Invalid base64 image or target component is null.");
            return;
        }

        try
        {
            // Remove the "data:image/png;base64," prefix if present
            if (base64Image.Contains(","))
            {
                base64Image = base64Image.Split(',')[1];
            }

            // Convert Base64 to byte array
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Load byte array into Texture2D
            Texture2D texture = new Texture2D(2, 2);
            if (!texture.LoadImage(imageBytes))
            {
                Debug.LogError("Failed to load Base64 image.");
                return;
            }

            // Apply to UI Image
            if (target is Image uiImage)
            {
                uiImage.sprite = TextureToSprite(texture);
            }
            // Apply to Renderer (e.g., 3D object)
            else if (target is Renderer renderer)
            {
                renderer.material.mainTexture = texture;
            }
            // Apply to RawImage (UI)
            else if (target is RawImage rawImage)
            {
                rawImage.texture = texture;
            }
            else
            {
                Debug.LogWarning("Component type not supported for Base64 image application.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error applying Base64 image: {e.Message}");
        }
    }

    private static Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
