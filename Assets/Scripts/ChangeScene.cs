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
        //�V�[���̖��O���Ƃɕς���(���������ƌ����悭�ł���)
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

    //�N���b�N������
    public void OnClick()
    {
        flag = true;
    }

    //�X�y�[�X�������́Z����������
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
