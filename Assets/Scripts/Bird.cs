using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject[] BirdPrefabs;

    //minimum count distract chain
    [SerializeField] private float removeBirdMinCount = 3;


    //the distance to judge chainaction
    [SerializeField] private float birdDistance = 1.4F;

    private GameObject firstBird;
    private GameObject lastBird;
    private string currentName;
    List<GameObject> removableBirdList = new List<GameObject>();




    // Start is called before the first frame update
    void Start()
    {
        TouchManager.Began += (info) =>
        {
            //get hit object on click point
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint), 
                Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;

                //judge and initailize the tag of hit object
                if (hitObj.tag == "Bird")
                {
                    firstBird = hitObj;
                    lastBird = hitObj;
                    currentName = hitObj.name;
                    removableBirdList = new List<GameObject>();
                    PushToBirdList(hitObj);
                }

                //Debug.Log("Hit object is " + hit.collider.gameObject.name);
            }

        };

        TouchManager.Moved += (info) =>
        {
            if(!firstBird)
            {
                return;
            }

            //get hit object on click point
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;

                //the case the hit object's tag is "bird" and the name is same and not different from last hit object and not still stored
                if (hitObj.tag == "Bird" && 
                hitObj.name == currentName &&
                hitObj != lastBird &&
                0 > removableBirdList.IndexOf(hitObj))
                {

                //judge distance
                    float distance = Vector2.Distance(hitObj.transform.position, lastBird.transform.position);
                    if (distance > birdDistance)
                    {
                        return;
                    }
                    lastBird = hitObj;
                    PushToBirdList(hitObj);
                }
            }
        };

        TouchManager.Ended += (info) =>
        {
            //
            int removeCount = removableBirdList.Count;
            if (removeCount >= removeBirdMinCount)
            {
                foreach (GameObject obj in removableBirdList)
                {
                    Destroy(obj);
                }
                StartCoroutine(DropBirds(removeCount));
            }


            foreach (GameObject obj in removableBirdList)
            {
                Changecolor(obj, 1.0F);
            };
            removableBirdList = new List<GameObject>();
            firstBird = null;
            lastBird = null;
        };
        StartCoroutine(DropBirds(100));
    }

    private void PushToBirdList(GameObject obj)
    {
        removableBirdList.Add(obj);
        Changecolor(obj, 0.5F);
    }

    private void Changecolor(GameObject obj, float transparency)
    {
        SpriteRenderer birdSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        birdSpriteRenderer.color = new Color(birdSpriteRenderer.color.r, 
            birdSpriteRenderer.color.g, 
            birdSpriteRenderer.color.b, 
            transparency);

    }


    IEnumerator DropBirds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //position birds appear
            Vector2 pos = new Vector2(Random.Range(-4.44F, 4.44F), 10.05F);
            //appear birds and registor ID
            int id = Random.Range(0, BirdPrefabs.Length);
            //generate birds
            GameObject bird = (GameObject)Instantiate(BirdPrefabs[id], 
                pos, 
                Quaternion.AngleAxis(Random.Range(-40,40), Vector3.forward));
            //bird's name
            bird.name = "Bird" + id;
            //wait during 0.05 second
            yield return new WaitForSeconds(0.5F);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
