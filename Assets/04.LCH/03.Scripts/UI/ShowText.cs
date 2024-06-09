using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ShowText : MonoBehaviour
{
    public Text textDisplay;
    public Text pressSpaceBar;

    public string[] texts;
    string temp;

    int index = 0;

    bool isTyping = false; // 텍스트 애니메이션이 진행 중인지 확인하기 위한 변수
    public float delay = 0.05f; // 글자 타이핑 속도(한 글자)

    private void Start()
    {
        NextText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping) // 텍스트 애니메이션이 끝났는지 확인
        {
            NextText();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    private void NextText()
    {
        if (index < texts.Length)
        {
            temp = texts[index];
            index++;

            pressSpaceBar.gameObject.SetActive(false);

            // 마지막 텍스트 X
            if (index < texts.Length)
            {
                // 특정 텍스트에서 카메라 무빙
                if (temp == "하지만 최근, 섬의 평화를 위협하는 어둠의 기운이 나타났습니다.")
                {
                    // 카메라 이동
                    Camera.main.transform.DOMove(new Vector3(8,0,-8), 1f);
                }
                else if(temp == "하늘의 수호자들은 몬스터의 침략에 맞서 싸우지만, 수가 점점 늘어나는 몬스터들에 의해 점점 밀려나게 됩니다.")
                {
                    Camera.main.transform.DOMove(new Vector3(16, 0, -8), 1f);
                }
                else if(temp == "결국, 몬스터의 공격을 받은 아에테리아 섬은 위기에 빠졌습니다.")
                {
                    Camera.main.transform.DOMove(new Vector3(24, 0, -8), 1f);
                }
                else if(temp == "그때, 섬을 구하기 위해 나타난 자들이 존재했습니다.")
                {
                    Camera.main.transform.DOMove(new Vector3(32, 0, -8), 1f);
                }
            }
            else // 마지막 텍스트 O
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex + 1);
            }

            StartCoroutine(Typing(temp));
        }
    }

    IEnumerator Typing(string text)
    {
        isTyping = true; 
        textDisplay.text = string.Empty; // 현재 텍스트를 삭제

        for (int i = 0; i < text.Length; i++)
        {
            textDisplay.text += text[i];

            // 다음 글자 애니메이션 속도
            yield return new WaitForSeconds(delay);
        }

        isTyping = false; // 텍스트 애니메이션 종료
        pressSpaceBar.gameObject.SetActive(true);

    }
}
