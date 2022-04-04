using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    /// <summary>
    /// The Rocket's Rect Transform
    /// </summary>
    RectTransform rocket;


    /// <summary>
    /// Reference of LevelSelectionManager will be here until LevelSelectionManager is not a Singleton Class
    /// </summary>
    public LevelSelectionManager levelSelectionManager;


    /// <summary>
    /// The Waypoints on which the rocket will move on
    /// </summary>
    RectTransform[] rocketWaypoints;


    /// <summary>
    /// Rocket's Initial Position when starting on starting of the scene
    /// This will always be the CurrentLevel rocketWaypoints[0]'s position.
    /// </summary>
    Vector3 rocketInitialPosition;

    /// <summary>
    /// Value is used as a percentage of path covered
    /// </summary>
    float value = 0.0f;


    /// <summary>
    /// An important Bool for triggering the Rocket Animation
    /// </summary>
    public bool isRocketLaunched = false;

    public TrailRenderer trailRenderer;
    private Vector3 prev;

    void Start()
    {
        rocket = GetComponent<RectTransform>();
        if (levelSelectionManager != null)
        {
            rocketInitialPosition = levelSelectionManager.CurrentLevelData.RocketWaypoints[0].position;
            rocket.position = rocketInitialPosition;
        }
        rocketWaypoints = levelSelectionManager.CurrentLevelData.RocketWaypoints;
    }
    /// <summary>
    /// ByDefault LaunchRocket is getting triggered on LevelSelection Start
    /// TODO: Needs to be controlled only after getting a level pass event
    /// </summary>
    public void LaunchRocket()
    {
        
        //levelSelectionManager.scroll.vertical = false;
        if (rocketWaypoints != null)
        {
            levelSelectionManager.Path.SetActive(false);
            levelSelectionManager.SetActiveLevelsButtonOnRocketLaunch(false);
            //isRocketLaunched = true;
            iTween.MoveTo(this.gameObject, iTween.Hash(
                       "path", rocketWaypoints,
                       "looktime", 0.1f, 
                       "lookahead",0.1f,
                       "time", 5f,
                       "easetype",iTween.EaseType.linear,
                       "oncompletetarget",this.gameObject,
                       "oncomplete","OnLaunchComplete"
                       ));
            StartCoroutine(RotateObject());
        }

    }

    IEnumerator RotateObject()
    {
        yield return new WaitForSeconds(0.1f);
        trailRenderer.enabled = true;
        while (true)
        {
            yield return new WaitForSeconds(0.001f);
            transform.up = transform.position - prev;
            prev = transform.position;
        }
    }

    private void OnLaunchComplete() 
    {
        //levelSelectionManager.scroll.vertical = true;

        trailRenderer.enabled = false;
        levelSelectionManager.SetActiveLevelsButtonOnRocketLaunch(true);
        levelSelectionManager.Path.SetActive(true);
        levelSelectionManager.ActivateLevels(GameDataStore.LastUnloackedLevel);
    }

    /// <summary>
    /// Update Misses out a couple frames which misses out on invoking the event hence used FixedUpdate
    /// When the rocket is launched, it increases the path's perecentage over time from 0 to 1 using iTween.PuthOnPath.
    /// checks if the rocket has reached the final waypoint then Invoke the Next Level and reset the rocket launch
    /// Also enabled/disables the Rocket Path and all the Active Levels Button Interaction when rocket launched
    /// 
    /// Speed of the Rocket can Be Controlled by Decreasing/Increasing the divisor value in Time.deltaTime / 4 ; 
    /// Higher the divisor value, Less the Speed of the Rocket
    /// </summary>
    private void FixedUpdate()
    {
        //if (isRocketLaunched)
        //{
            //if (value <= 1 && rocketWaypoints != null)
            //{
                //levelSelectionManager.Path.SetActive(false);
                //levelSelectionManager.SetActiveLevelsButtonOnRocketLaunch(false);
                
                //Rocket Animation
                //Speed Increase Decrease
                //value += Time.deltaTime / 4;
                //iTween.PutOnPath(this.gameObject, rocketWaypoints, value);
               

                //if (Vector3.Distance(this.transform.position, rocketWaypoints[rocketWaypoints.Length - 1].position) <= 0.1f)
                //{
                //    isRocketLaunched = false;
                //    levelSelectionManager.SetActiveLevelsButtonOnRocketLaunch(true);
                //    levelSelectionManager.Path.SetActive(true);
                //    levelSelectionManager.LoadNextLevelNow?.Invoke();
                //}
            //}
        //}
    }
}
