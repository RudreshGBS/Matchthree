using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    RectTransform rocket;
    public LevelSelectionManager levelSelectionManager;
    RectTransform[] rocketWaypoints;
    Vector3 rocketInitialPosition;
    float value = 0.0f;
    public bool isRocketLaunched = false;
    // Start is called before the first frame update
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

    public IEnumerator LaunchRocket()
    {
        isRocketLaunched = true;
        yield return new WaitForSeconds(10f);
        isRocketLaunched = false;
    }

    private void Update()
    {
        if (isRocketLaunched)
        {
            if (value <= 1 && rocketWaypoints != null)
            {
                value += Time.deltaTime / 3;
                iTween.PutOnPath(this.gameObject, rocketWaypoints, value);
            }
        }
    }
}
