using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    private string scene;

    private GameObject fadeCanvas;

    bool flag = false;
    
    void Start()
    {
        fadeCanvas = GameObject.Find("FadeCanvas");
        scene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        //シーンの名前ごとに変える(多分もっと効率よくできる)
        switch(scene)
        {
            case "Title":
                {
                    if (flag)
                        fadeCanvas.GetComponent<FadeControll>().IsFadeIn = true;

                    if (flag&&fadeCanvas.GetComponent<FadeControll>().FadeRange >= 1)
                    {
                        SceneManager.LoadScene("SampleScene");
                        flag = false;
                    }
                    break;
                }
            case "SampleScene":
                {
                    if (flag)
                        fadeCanvas.GetComponent<FadeControll>().IsFadeIn = true;

                    if (flag && fadeCanvas.GetComponent<FadeControll>().FadeRange >= 1) 
                    {
                        SceneManager.LoadScene("Result");
                        flag = false;
                    }
                    break;
                }
            case "Result":
                {
                    if (flag)
                        fadeCanvas.GetComponent<FadeControll>().IsFadeIn = true;

                    if (flag && fadeCanvas.GetComponent<FadeControll>().FadeRange >= 1)
                    {
                        SceneManager.LoadScene("Title");
                        flag = false;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }



    }

    //クリックしたら
    public void OnClick()
    {
        flag = true;
    }

    //スペースもしくは〇を押したら
    public void PushSpaceOrCircle()
    {
        flag = true;
    }

    public bool ChangeFlag
    {
        get
        {
            return flag;
        }
        set
        {
            flag = value;
        }
    }
}
