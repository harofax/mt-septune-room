using System.Collections.Generic;
using UnityEngine;

public class GunkManager : MonoBehaviour
{
    private static List<GunkSurface> gunkSurfaceList = new List<GunkSurface>();
    static HashSet<Transform> fastSplatStorage = new HashSet<Transform>();

    private static Dictionary<GunkSurface, RenderTexture> GunkChunkStorage =
        new Dictionary<GunkSurface, RenderTexture>();

    //public Transform toothbrush;

    //public ToothBrushManager tbmanager;
    
    // Start is called before the first frame update
    void Start()
    {

        //InvokeRepeating(nameof(GetClosestSurface), 0, 1);

        //gunkSurfaces = FindObjectsOfType<GunkSurface>();

        //GenerateNoiseTexture();

    }

    

    // // Get index of closest gunksurface
    // public void FindClosestSurface ()
    // {
    //     GunkSurface bestTarget = null;
    //     float closestDistanceSqr = Mathf.Infinity;
    //     Vector3 currentPosition = toothbrush.position;
    //     foreach(GunkSurface potentialTarget in gunkSurfaces)
    //     {
    //         Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
    //         float dSqrToTarget = directionToTarget.sqrMagnitude;
    //         if(dSqrToTarget < closestDistanceSqr)
    //         {
    //             closestDistanceSqr = dSqrToTarget;
    //             bestTarget = potentialTarget;
    //         }
    //     }
    //
    //     if (bestTarget == null)
    //     {
    //         closestSurface = 0;
    //     }
    //     else
    //     {
    //         closestSurface = Array.IndexOf(gunkSurfaces, bestTarget);
    //     }
    // }

    // private void GetClosestSurface()
    // {
    //     FindClosestSurface();
    //     tbmanager.SetSplatMap(gunkSurfaces[closestSurface]);
    // }

    public static void AddGunkSurface(GunkSurface surface)
    {
        fastSplatStorage.Add(surface.transform);
        gunkSurfaceList.Add(surface);

        //gunkSurfaces.Add(surface.transform, surface);
    }

    public static void RemoveGunkSurface(GunkSurface surface)
    {
        fastSplatStorage.Remove(surface.transform);
        gunkSurfaceList.Remove(surface);
        // if (gunkSurfaces.ContainsKey(surface.transform))
        // {
        //     gunkSurfaces.Remove(surface.transform);
        // }
    }

    public static bool GetGunkSurface(Transform key) //GunkSurface
    {
        return fastSplatStorage.Contains(key);
        //if (!gunkSurfaces.ContainsKey(key)) return null;
        //return gunkSurfaces[key];
    }

    public static void SaveGunkSurface(GunkSurface surface)
    {
        RenderTexture splatCopy =
            new RenderTexture((int)surface.splatSize.x, (int)surface.splatSize.y, 0, RenderTextureFormat.ARGBHalf);
        
        Graphics.CopyTexture(surface.splatmap, splatCopy);
        
        GunkChunkStorage[surface] = splatCopy;
    }

    public static RenderTexture LoadGunkSurface(GunkSurface surface)
    {
        return GunkChunkStorage[surface];
    }

    public static GunkSurface GetGunkSurfaceObject(int index)
    {
        if (index > gunkSurfaceList.Capacity || index < 0)
        {
            return gunkSurfaceList[0];
        }

        return gunkSurfaceList[index];
    }

    public static bool Contains(Transform key)
    {
        return fastSplatStorage.Contains(key);
    }

}
