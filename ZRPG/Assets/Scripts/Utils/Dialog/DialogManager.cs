using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : Singleton<DialogManager>
{
    public GameObject panel_dialog;
    public Text text_speaker;
    public Image portrait;
    public Text text_sentence;

    int sentenceCounter;
    int contentCounter;
    Dialog currentDialog;

    //开始对话
    public void TriggerDialog(Dialog _dialog)
    {
        panel_dialog.GetComponent<Animator>().SetBool("isShow", true);
		//暂停游戏
		ToggleGamePause(true);

        currentDialog = _dialog;

        sentenceCounter = 0;
        contentCounter = 0;

        DisplayNextSentence();
    }

    //显示下一句
    public void DisplayNextSentence()
    {
        //如果已经是最后一句就结束对话
        if (sentenceCounter == currentDialog.sentencess.Length)
        {
            EndDialog();
        }
        else
        {
            //播放对话
            text_speaker.text = currentDialog.sentencess[sentenceCounter].speaker;
            text_sentence.text = currentDialog.sentencess[sentenceCounter].contents[contentCounter].text;
            portrait.sprite = currentDialog.sentencess[sentenceCounter].contents[contentCounter].portrait;

            contentCounter++;

            //是语句最后一句就进行下一段
            if (contentCounter == currentDialog.sentencess[sentenceCounter].contents.Length)
            {
                sentenceCounter++;
                contentCounter = 0;
            }
        }
    }

    //结束对话
    public void EndDialog()
    {
        panel_dialog.GetComponent<Animator>().SetBool("isShow", false);

		ToggleGamePause(false);

    }

	void ToggleGamePause(bool pause)
	{

	}
}
