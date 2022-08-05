using UnityEngine;

public static class Extensions
{
    public static Bounds GetBounds(this GameObject gameObject)
    {
        var gameObjectRenderers = gameObject.GetComponentsInChildren<Renderer>();
        if (gameObjectRenderers.Length > 0)
        {
            var combinedBounds = gameObjectRenderers[0].bounds;

            foreach (var render in gameObjectRenderers)
                combinedBounds.Encapsulate(render.bounds);

            return combinedBounds;
        }

        return new Bounds();
    }

    public static Bounds GetBounds(this MonoBehaviour gameObject)
    {
        var gameObjectRenderers = gameObject.GetComponentsInChildren<Renderer>();
        if (gameObjectRenderers.Length > 0)
        {
            var combinedBounds = gameObjectRenderers[0].bounds;

            foreach (var render in gameObjectRenderers)
                combinedBounds.Encapsulate(render.bounds);

            return combinedBounds;
        }

        return new Bounds();
    }
}