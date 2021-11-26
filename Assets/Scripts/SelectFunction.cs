using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFunction : MonoBehaviour
{
	public Button BtnPhLogin;
	public Button BtnChatFun;
	public Button BtnLoginBack;
	public Button BtnChatBack;

	public GameObject MainMenuPanel;
	public GameObject LoginPanel;
	public GameObject ChatPanel;

	// Start is called before the first frame update
	void Start()
    {
		MainMenuPanel.SetActive(true);
		ResetPanels();
		BtnPhLogin.onClick.AddListener(OnClickBtnPhlogin);
		BtnChatFun.onClick.AddListener(OnClickBtnChatFun);
		BtnLoginBack.onClick.AddListener(OnClickBtnBack);
		BtnChatBack.onClick.AddListener(OnClickBtnBack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnClickBtnPhlogin() {
		MainMenuPanel.SetActive(false);
		LoginPanel.SetActive(true);
		this.gameObject.GetComponent<PhNumberAuth>().txtdebug.text = "Debug Log";
		this.gameObject.GetComponent<PhNumberAuth>().Phonenumber.text = "";
		this.gameObject.GetComponent<PhNumberAuth>().VerifyOTP.text = "";
	}

	public void OnClickBtnChatFun() {
		MainMenuPanel.SetActive(false);
		ChatPanel.SetActive(true);
	}

	public void ResetPanels() {
		LoginPanel.SetActive(false);
		ChatPanel.SetActive(false);
	}

	public void OnClickBtnBack() {
		ResetPanels();
		MainMenuPanel.SetActive(true);
	}
}
