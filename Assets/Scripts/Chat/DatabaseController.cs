using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;
using System;

public class DatabaseController : MonoBehaviour {
	public static DatabaseController instance;
	public TMP_InputField _name;
	public TMP_InputField _message;

	public GameObject messagePrefab;
	public Transform msgContainer;

	public List<User> MessageList = new List<User>();

	public int MessageCount = 0;
	//public Dictionary<string, MessageHandler> messages = new Dictionary<string, MessageHandler>();

	//private IEnumerator coroutine;

	DatabaseReference reference;
	FirebaseDatabase database;

	private void Awake() {

	}
	// Start is called before the first frame update
	void Start() {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		database = FirebaseDatabase.GetInstance(app);
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		addListeners();
		StartCoroutine(ListenChatApply());
	}

	// Update is called once per frame
	void Update() {

	}

	public void SendMessage() {
		if (_message.text != string.Empty) {
			string name = _name.text;
			string message = _message.text;
			User user = new User(name, message);
			writeNewUser(user);
			
			_message.text = "";
		}
	}

	private void writeNewUser(User user) {
		//User user = new User(name, message);
		string json = JsonUtility.ToJson(user);
		string key = reference.Child("messages").Push().Key;
		reference.Child("messages").Child(key).SetRawJsonValueAsync(json);
	}

	public void CreateMessage(User user) {
		GameObject newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
		newMessage.transform.SetParent(msgContainer, false);
		MessageHandler newMessageHandler = newMessage.GetComponent<MessageHandler>();
		newMessageHandler.user = user;
	}

	//public void RetrieveData() {
	//	FirebaseDatabase reference = FirebaseDatabase.DefaultInstance;
	//	reference.GetReference("messages").GetValueAsync().ContinueWith(task => {
	//		if (task.IsFaulted) {
	//			// Handle the error...
	//		} else if (task.IsCompleted) {
	//			DataSnapshot snapshot = task.Result;
	//			// Do something with snapshot...
	//			Debug.Log("Get Snapshot");
	//			foreach (DataSnapshot ele in snapshot.Children) {
	//				//Debug.Log(ele);
	//				userList.Add(JsonUtility.FromJson<User>(ele.GetRawJsonValue()));

	//			}
	//			for (int i = 0; i < userList.Count; i++) {
	//				CreateMessage(userList[i]);
	//			}

	//		}
	//	});

	//}

	IEnumerator ListenChatApply() {
		while (true) {
			if (MessageList.Count > MessageCount) {
				CreateMessage(MessageList[MessageCount]);
				MessageCount++;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void addListeners() {
		reference = database.GetReference("messages");
		reference.ChildAdded += OnChildAdded;
		//reference.ChildChanged += OnChildChanged;
	}

	//call when sign out
	private void removeListeners() {
		reference = database.GetReference("messages");
		reference.ChildAdded -= OnChildAdded;
		reference.ChildChanged -= OnChildChanged;
	}

	private void OnChildChanged(object sender, ChildChangedEventArgs args) {
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		// Do something with the data in args.Snapshot
	}

	private void OnChildAdded(object sender, ChildChangedEventArgs args) {
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		var ele = JsonUtility.FromJson<User>(args.Snapshot.GetRawJsonValue());
		User user = new User(ele.username, ele.message);
		MessageList.Add(user);
		Debug.Log(user.username + " : " + user.message);
	}


}
