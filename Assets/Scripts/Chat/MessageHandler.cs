using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageHandler : MonoBehaviour
{
	public TextMeshProUGUI txtMessage;

	public User user;
    // Start is called before the first frame update
    void Start()
    {
		txtMessage.text = user.username + " : " + user.message;
    }

}
