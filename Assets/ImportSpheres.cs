using UnityEngine;

using System.Collections;

using System.Net;

using System.IO;



public class ImportSpheres : MonoBehaviour

{

    // The radius of our outer sphere



    const float radius = 0.8f;



    IEnumerator DownloadSpheres()

    {

        // Pull down the JSON from our web-service



        WWW w = new WWW(

          "http://apollonian.cloudapp.net/api/spheres/" +

          radius.ToString() + "/7"

        );

        yield return w;



        print("Waiting for sphere definitions\n");



        // Add a wait to make sure we have the definitions



        yield return new WaitForSeconds(1f);



        print("Received sphere definitions\n");



        // Extract the spheres from our JSON results



        ExtractSpheres(w.text);

    }



    void Start()

    {

        print("Started sphere import...\n");



        StartCoroutine(DownloadSpheres());

    }



    void ExtractSpheres(string json)

    {

        // Create a JSON object from the text stream



        JSONObject jo = new JSONObject(json);



        // Our outer object is an array



        if (jo.type != JSONObject.Type.ARRAY)

            return;



        // Set up some constant offsets for our geometry



        const float xoff = 1, yoff = 1, zoff = 1;



        // And some counters to measure our import/filtering



        int displayed = 0, filtered = 0;



        // Go through the list of objects in our array



        foreach (JSONObject item in jo.list)

        {

            // For each sphere object...



            if (item.type == JSONObject.Type.OBJECT)

            {

                // Gather center coordinates, radius and level



                float x = 0, y = 0, z = 0, r = 0;

                int level = 0;



                for (int i = 0; i < item.list.Count; i++)

                {

                    // First we get the value, then switch

                    // based on the key



                    var val = (JSONObject)item.list[i];

                    switch ((string)item.keys[i])

                    {

                        case "X":

                            x = (float)val.n;

                            break;

                        case "Y":

                            y = (float)val.n;

                            break;

                        case "Z":

                            z = (float)val.n;

                            break;

                        case "R":

                            r = (float)val.n;

                            break;

                        case "L":

                            level = (int)val.n;

                            break;

                    }

                }



                // Create a vector from our center point, to see

                // whether it's radius comes near the edge of the

                // outer sphere (if not, filter it, as it's

                // probably occluded)



                Vector3 v = new Vector3(x, y, z);

                if ((Vector3.Magnitude(v) + r) > radius * 0.99)

                {

                    // We're going to display this sphere



                    displayed++;



                    // Create a corresponding "game object" and

                    // transform it



                    var sphere =

                      GameObject.CreatePrimitive(PrimitiveType.Sphere);

                    sphere.transform.position =

                      new Vector3(x + xoff, y + yoff, z + zoff);

                    float d = 2 * r;

                    sphere.transform.localScale =

                      new Vector3(d, d, d);



                    // Set the object's color based on its level



                    UnityEngine.Color col = UnityEngine.Color.white;

                    switch (level)

                    {

                        case 1:

                            col = UnityEngine.Color.red;

                            break;

                        case 2:

                            col = UnityEngine.Color.yellow;

                            break;

                        case 3:

                            col = UnityEngine.Color.green;

                            break;

                        case 4:

                            col = UnityEngine.Color.cyan;

                            break;

                        case 5:

                            col = UnityEngine.Color.magenta;

                            break;

                        case 6:

                            col = UnityEngine.Color.blue;

                            break;

                        case 7:

                            col = UnityEngine.Color.grey;

                            break;

                    }

                    sphere.GetComponent<Renderer>().material.color = col;

                }

                else

                {

                    // We have filtered a sphere - add to the count



                    filtered++;

                }

            }

        }



        // Report the number of imported vs. filtered spheres



        print(

          "Displayed " + displayed.ToString() +

          " spheres, filtered " + filtered.ToString() +

          " others."

        );

    }



    void Update()

    {

    }

}