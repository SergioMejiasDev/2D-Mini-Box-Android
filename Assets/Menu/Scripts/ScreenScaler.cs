using UnityEngine;

/// <summary>
/// Functions in charge of modifying the screen resolution.
/// </summary>
public static class ScreenScaler
{
    /// <summary>
    /// Function called to scale the screen to a 16:9 format.
    /// </summary>
    public static void ScaleScreen()
    {
        float targetAspect = 16.0f / 9.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowAspect / targetAspect;
        Camera camera = Camera.main;

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }

        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
