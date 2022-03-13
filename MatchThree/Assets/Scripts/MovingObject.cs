using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    private GameObject Planet1;
    [SerializeField]
    private GameObject Planet2;
    [SerializeField]
    private GameObject Planet3;
    [SerializeField]
    private GameObject Rocket;
    [SerializeField]
    private GameObject Star;
    [SerializeField]
    Transform StarTargetPos;
    [SerializeField]
    Transform RocketTargetPos;
    [SerializeField]
    GameObject playButton;
    [SerializeField]
    SpriteRenderer ElonGoatLogo;
    [SerializeField]
    MainMenuManger menuManger;

    private Vector3 initPosPlanet1;
    private Vector3 initPosPlanet2;
    private Vector3 initPosRocket;
    private Vector3 initPosStar;
    // Start is called before the first frame update
    void Start()
    {
        initPosPlanet1 = Planet1.transform.position;
        initPosPlanet2 = Planet2.transform.position;
        initPosRocket = Rocket.transform.position;
        initPosStar = Star.transform.position;
        StartSplashSceen();
        SoundManager.Instance.PlayMusic(SoundType.MainMenu);
    }

    private void StartMovement()
    {
        MovePlanet1();
        MovePlanet2(); 
        MovePlanet3();
        Invoke("MoveRocket", 1f);
        Invoke("MoveStar", 3f);
        MovePlayButton();
    }

    void StartSplashSceen() {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0,
            "to",1,
            "onupdate","SpriteColorUpdate",
            "oncompletetarget",gameObject,
            "oncomplete", "EndSplashSceen",
            "time", 2
            ));
    }

    void EndSplashSceen()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 1,
            "to", 0,
            "onupdate", "SpriteColorUpdate",
            "oncompletetarget", gameObject,
            "oncomplete", "StartMenu",
            "time", 2
            ));
    }
    private void StartMenu() 
    {
        StartMovement();
        menuManger.ConnectWallet();
    }
    void SpriteColorUpdate(float value)
    { 
        Color color = Color.white;
        color.a = value;
        ElonGoatLogo.color = color;
    }

    void MovePlanet1() {
        iTween.MoveTo(Planet1, iTween.Hash(
        "x", 5,
        "islocal", true,
        "easetype", iTween.EaseType.linear,
        "oncompletetarget", gameObject,
        "oncomplete", "ResetPlanet1",
        "time", Random.Range(30f,35f)
        ));
    }
    void ResetPlanet1() {
        Planet1.transform.position = initPosPlanet1;
        MovePlanet1();
    }
    void ResetRocket()
    {
        Rocket.SetActive(false);
        Rocket.transform.position = initPosRocket;
        Invoke("MoveRocket", 5f);
    }
    void ResetStar()
    {
        Star.SetActive(false);
        Star.transform.position = initPosStar;
        Invoke("MoveStar", 10f);
    }
    void MovePlanet2() 
    {
        iTween.RotateBy(Planet2, iTween.Hash(
        "z", 1f,
        "islocal", true,
        "looptype", iTween.LoopType.loop,
        "easetype", iTween.EaseType.linear,
        "time", 50f
        ));
    }
    void MovePlanet3()
    {
        iTween.RotateBy(Planet3, iTween.Hash(
        "z", 1f,
        "islocal", true,
        "looptype", iTween.LoopType.loop,
        "easetype", iTween.EaseType.linear,
        "time", 50f
        ));
    }
    void MoveRocket() {
        Rocket.SetActive(true);
        iTween.MoveTo(Rocket, iTween.Hash(
           "position", RocketTargetPos.position,
           "islocal", true,
           "easetype", iTween.EaseType.linear,
           "oncompletetarget", gameObject,
           "oncomplete", "ResetRocket",
           "time", 20
           )) ;
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
    void MovePlayButton() {
        iTween.ScaleTo(playButton, iTween.Hash(
            "scale",Vector3.one*1.1F,
            "islocal", true,
            "looptype", iTween.LoopType.pingPong, 
            "time", 1f
            ));
    }
}
