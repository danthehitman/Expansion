
using UnityEngine;


namespace Assets.Scripts.Common.WorldInteraction
{

    public class Raycaster
    {

        private Vector2 raycastDirection;
        private Vector2[] rays;
        private LayerMask layerMask;
        private float raycastLength;

        public Raycaster(Vector2 rayOne, Vector2 rayTwo, Vector2 dir, LayerMask mask, Vector2 parallelInset, Vector2 perpendicularInset, float checkLength = 0.0f)
        {
            raycastDirection = dir;
            //The offset stuff is about moving the starting point of the raycast inside of the collider to tell unity that we dont want
            //to be able to collide with ourselves.  Without the offset the raycast will collide with the entity.
            //It seems to work fine for me without these.  Not sure why. EDIT: Apparently because I had the correct LayerMask set.
            rays = new Vector2[] {
                rayOne + parallelInset + perpendicularInset,
                rayTwo - parallelInset + perpendicularInset,
            };
            //The addLength is to make sure that our starting raycast length is enough to get outside of the collider after the offset.
            raycastLength = perpendicularInset.magnitude + checkLength;
            layerMask = mask;
        }

        public float CastForDistance(Vector2 origin, float distance)
        {
            float minDistance = distance;
            foreach (var offset in rays)
            {
                RaycastHit2D hit = Raycast(origin + offset, raycastDirection, distance + raycastLength, layerMask);
                if (hit.collider != null)
                    minDistance = Mathf.Min(minDistance, hit.distance - raycastLength);
            }
            return minDistance;
        }

        public RaycastHitInfo CastForHit(Vector2 origin)
        {
            var hitInfo = new RaycastHitInfo();
            //foreach (var ray in rays)
            for (int i = 0; i < rays.Length; i++)
            {
                var ray = rays[i];
                RaycastHit2D hit = Raycast(origin + ray, raycastDirection, raycastLength, layerMask);
                if (hit.collider != null)
                {
                    if (i == 0)
                        hitInfo.ColliderOne = hit.collider;
                    else if (i == 1)
                        hitInfo.ColliderTwo = hit.collider;
                }
            }
            return hitInfo;

        }

        //private RaycastHit2D? DoRaycast(Vector2 origin, float distance = 0)
        //{
        //    RaycastHit2D? hit = null;
        //    foreach (var offset in offsetPoints)
        //    {
        //        hit = Raycast(origin + offset, raycastDirection, distance + addLength, layerMask);
        //        if (hit != null)
        //            break;
        //        //if (hit != null)
        //        //    {
        //        //        //MoveThroughPlatform mtp = hit.collider.GetComponent<MoveThroughPlatform>();
        //        //        //if (mtp == null || Vector2.Dot(raycastDirection, mtp.permitDirection) < mtp.dotLeeway)
        //        //        //{
        //        //        minDistance = Mathf.Min(minDistance, hit.distance - addLength);
        //        //        //}
        //        //    }

        //        //}
        //        //return minDistance;
        //    }
        //    return null;
        //}

        private RaycastHit2D Raycast(Vector2 start, Vector2 dir, float len, LayerMask mask)
        {
            //Debug.Log(string.Format("Raycast start {0} in {1} for {2}", start, dir, len));
            Debug.DrawLine(start, start + dir * len, Color.red);
            //return Physics2D.CircleCast(start, 0.5f, dir, len, mask);
            return Physics2D.Raycast(start, dir, len, mask);
        }

        public class RaycastHitInfo
        {
            public Collider2D ColliderOne { get; set; }
            public Collider2D ColliderTwo { get; set; }

            public bool IsColliding()
            {
                return ColliderOne != null || ColliderTwo != null;
            }

            public Collider2D GetFirstCollider()
            {
                return ColliderOne ?? ColliderTwo;
            }

        }
    }
}
