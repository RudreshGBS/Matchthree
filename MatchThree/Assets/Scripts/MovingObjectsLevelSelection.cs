using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectsLevelSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject Rocket;
    [SerializeField]
    private Transform RocketDestination;
    private Vector3 initPosRocket;
    [SerializeField]
    private GameObject Rocket2;
    [SerializeField]
    private Transform RocketDestination2;
    private Vector3 initPosRocket2;
    [SerializeField]
    private GameObject Star;
    [SerializeField]
    Transform StarTargetPos;
    private Vector3 initPosStar;
    [SerializeField]
    private GameObject Star2;
    [SerializeField]
    Transform StarTargetPos2;
    private Vector3 initPosStar2;

    [SerializeField]
    private GameObject Planet1;
    private Vector3 initPosPlanet1;

    [SerializeField]
    private GameObject Planet2;
    private Vector3 initPosPlanet2;

    [SerializeField]
    private GameObject UFO;
    private Vector3 initPosUFO;


    private void Start()
    {
        initPosRocket = Rocket.transform.position;
        initPosRocket2 = Rocket2.transform.position;
        initPosStar = Star.transform.position;
        initPosStar2 = Star2.transform.position;
        initPosPlanet1 = Planet1.transform.position;
        initPosPlanet2 = Planet2.transform.position;
        initPosUFO = UFO.transform.position;

        StartMovement();
    }

    void StartMovement()
    {
        Invoke("MoveRocket", 1f);
        Invoke("MoveStar", 3f);
        Invoke("MoveStar2", 3f);
        Invoke("MoveRocket2", 2f);
        MovePlanet1();
        MovePlanet2();
        MoveUFO();
    }

    void MoveRocket()
    {
        Rocket.SetActive(true);
        iTween.MoveTo(Rocket, iTween.Hash(
           "position", RocketDestination.position,
           "islocal", true,
           "easetype", iTween.EaseType.linear,
           "oncompletetarget", gameObject,
           "oncomplete", "ResetRocket",
           "time", 20
           ));
    }


    void ResetRocket()
    {
        Rocket.SetActive(false);
        Rocket.transform.position = initPosRocket;
        Invoke("MoveRocket", 5f);
    }

    void MoveRocket2()
    {
        Rocket2.SetActive(true);
        iTween.MoveTo(Rocket2, iTween.Hash(
           "position", RocketDestination2.position,
           "islocal", true,
           "easetype", iTween.EaseType.linear,
           "oncompletetarget", gameObject,
           "oncomplete", "ResetRocket2",
           "time", 20
           ));
    }

    void ResetRocket2()
    {
        Rocket2.SetActive(false);
        Rocket2.transform.position = initPosRocket2;
        Invoke("MoveRocket2", 10f);
    }
    void MoveStar()
    {
        Star.SetActive(true);
        iTween.MoveTo(Star, iTween.Hash(
            "position", StarTargetPos.position,
            "islocal", true,
            "easetype", iTween.EaseType.easeInOutCubic,
            "oncompletetarget", gameObject,
            "oncomplete", "ResetStar",
            "time", 5
            ));
    }
    void ResetStar()
    {
        Star.SetActive(false);
        Star.transform.position = initPosStar;
        Invoke("MoveStar", 10f);
    }
    void MoveStar2()
    {
        Star2.SetActive(true);
        iTween.MoveTo(Star2, iTween.Hash(
            "position", StarTargetPos2.position,
            "islocal", true,
            "easetype", iTween.EaseType.easeInOutCubic,
            "oncompletetarget", gameObject,
            "oncomplete", "ResetStar2",
            "time", 5
            ));
    }
    void ResetStar2()
    {
        Star2.SetActive(false);
        Star2.transform.position = initPosStar2;
        Invoke("MoveStar2", 10f);
    }

    void MovePlanet1()
    {
        iTween.MoveTo(Planet1, iTween.Hash(
        "x", 5,
        "islocal", true,
        "easetype", iTween.EaseType.linear,
        "oncompletetarget", gameObject,
        "oncomplete", "ResetPlanet1",
        "time", Random.Range(30f, 35f)
        ));
    }
    void ResetPlanet1()
    {
        Planet1.transform.position = initPosPlanet1;
        MovePlanet1();
    }
    void MoveUFO()
    {
        iTween.MoveTo(UFO, iTween.Hash(
        "x", 4, "y", 2,
        "islocal", true,
        "easetype", iTween.EaseType.linear,
        "oncompletetarget", gameObject,
        "oncomplete", "ResetUFO",
        "time", Random.Range(30f, 35f)
        ));
    }
    void ResetUFO()
    {
        UFO.transform.position = initPosUFO;
        MoveUFO();
    }
    void MovePlanet2()
    {
        iTween.MoveTo(Planet2, iTween.Hash(
        "x", 1,
        "islocal", true,
        "easetype", iTween.EaseType.linear,
        "oncompletetarget", gameObject,
        "oncomplete", "ResetPlanet2",
        "time", Random.Range(30f, 35f)
        ));
    }
    void ResetPlanet2()
    {
        Planet2.transform.position = initPosPlanet2;
        MovePlanet2();
    }
}
