using UnityEngine;

public class AssetResolver 
{
    public T ResolveAsset<T>(AssetType assetType, UnityEngine.Object asset) where T : UnityEngine.Object
    {
        switch (assetType)
        {
            case AssetType.Sprite:
                if (asset is Texture2D texture)
                {
                    Sprite selectedSprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );

                    return selectedSprite as T;
                }
                break;

            case AssetType.Material:
                if (asset is Material material)
                {
                    return material as T;
                }
                break;

            case AssetType.Texture:
                if (asset is Texture2D tex)
                {
                    return tex as T;
                }
                break;

            case AssetType.Mesh:
                if (asset is Mesh mesh)
                {
                    return mesh as T;
                }
                break;
        }

        return asset as T;
    }
}