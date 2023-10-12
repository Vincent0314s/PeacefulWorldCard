using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualEffectManager
{
    public static GameObject CreateVisualEffect(VisualEffect effect, Vector3 pos, Quaternion angle)
    {
        return GameObject.Instantiate(GetVisualEffect(effect), pos, angle);
    }

    private static GameObject GetVisualEffect(VisualEffect visualeffect)
    {
        foreach (var item in BasicGameAssetManager.Instance.visualEffects)
        {
            if (item.effect == visualeffect)
            {
                return item.effectObject;
            }
        }
        return null;
    }
}
