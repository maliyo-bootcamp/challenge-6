using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRB;
    public float speed = 15.0f;
    public int minSwipeRecognition = 500;

    private bool isTraveling;
    private Vector3 nextCollisionPosition;
    private Vector3 travelDirection;
    private Vector2 swipePosLastFrame, swipePosCurrentFrame, currentSwipe;
    private float touchLimit = 0.5f;
    private Color solvedColor;


    private void Start()
    {
        solvedColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solvedColor;
    }


    private void FixedUpdate()
    {
        if (isTraveling)
        {
            playerRB.velocity = speed * travelDirection;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);

        int i = 0;
        while (i < hitColliders.Length)
        {
            GroundPieceController ground = hitColliders[i].transform.GetComponent<GroundPieceController>();
            if (ground && !ground.isColored)
            {
                ground.ChangeColor(solvedColor);
            }

            i++;
        }

        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isTraveling)
            return;

        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            Debug.Log("Mouse clicked");
            
            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                    return;
            
                currentSwipe.Normalize();

                Debug.Log("Current Swipe is normalized.");

            // UP & DOWN

            if (currentSwipe.x > -touchLimit && currentSwipe.x < touchLimit)            
            {
                SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
            }

            // LEFT & RIGHT 
            if (currentSwipe.y > -touchLimit && currentSwipe.y < touchLimit)
            {
                SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
            }
          }


            swipePosLastFrame = swipePosCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse released");
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }

        private void SetDestination (Vector3 direction)
        {
            travelDirection = direction;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, 100.0f))
            {
                nextCollisionPosition = hit.point;
            }

            isTraveling = true;
        }
}
