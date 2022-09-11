using System.Collections.Generic;
using UnityEngine;
using Axis.Abstractions;
using System.Linq;

public class PlayerInteraction : MonoBehaviour
{
    #region inspector_vars
    [SerializeField] private float interactionRange;
    [SerializeField] private LayerMask filterLayerMasks;
    #endregion

    #region local_vars
    private Collider2D[] nearbyColliders=new Collider2D[1],cachedColliders;
    private List<IInteractable> cachedInteractables=new List<IInteractable>();
    #endregion

    #region cached_vars
    private PlayerInputs _inputs;
    #endregion

    #region Events
    public delegate void PassInteractables(List<IInteractable> list);
    public static event PassInteractables GetNearbyInteractables;
    #endregion

    uint counter=0;
    private void Update()
    {
        ++counter;
        if (counter % 2 == 0) return;


        cachedColliders = nearbyColliders;
        nearbyColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, filterLayerMasks);
        
        if (nearbyColliders.Length==0) 
        {
            cachedInteractables.Clear();
            GetNearbyInteractables?.Invoke(cachedInteractables);
            return;
        }//if list is clears it, and ends the loop.

        if(nearbyColliders.SequenceEqual(cachedColliders)) return;
        cachedInteractables?.Clear(); //if array is not the same as in previous frame, clears lists, and checks again


        foreach (Collider2D col in nearbyColliders)
        {
            IInteractable obj = col.gameObject.GetComponent<IInteractable>();

            if (obj != null && obj.IsActive() && !cachedInteractables.Contains(obj))
            { 
                cachedInteractables.Add(obj);
            }

        }
        GetNearbyInteractables?.Invoke(cachedInteractables);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
