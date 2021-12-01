using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuizManager : GameParent
{
	private int score = 0;

	public Image QuestionImage;
	public AudioClip introSound, wrongSound, CorrentSound;
	public Dropzone dropZoneAnswer;
	public LetterTile letter;
	public GameObject DropZonePanel, ShuffledLetterPanel;
	public CongratzUIButtonGroup congratzUI;
	public CongratzUIButtonGroup finishedUI;
	public List<QuizAlphabet> quizList = new List<QuizAlphabet> ();

	AudioSource source;
	List<Dropzone> dropZoneList = new List<Dropzone> ();
	List<LetterTile> answerShuffleList = new List<LetterTile> ();

	private bool[] numbers;

	public static InputState state;

	void Start ()
	{
		numbers = new bool[quizList.Count];

		source = GetComponent<AudioSource> ();
		source.PlayOneShot (introSound);
		InitAlphabets ();
	}

	public override void OnNextButtonClick ()
	{
		base.OnNextButtonClick ();
		congratzUI.OnActivatingUI (false);
	}

	public override void OnPrevButtonClick ()
	{
		base.OnPrevButtonClick ();
		congratzUI.OnActivatingUI (false);
	}

	protected override void InitAlphabets ()
	{
		int randomNum = Random.Range(0, quizList.Count - 1);

		if (numbers[randomNum])
		{
			while (numbers[randomNum])
			{
				randomNum = Random.Range(0, quizList.Count);
			}
		}
		else
			numbers[randomNum] = true;

		//getting a random word from the list initialized in unity inspector
		QuizAlphabet currentQuiz = quizList [randomNum];

		LetterTile temp;
		Dropzone dropzoneTemp;

		//setting the image
		QuestionImage.sprite = currentQuiz.objectImage;

		//traversing each character in the word and creating them
		foreach (char c in currentQuiz.name) {
			temp = Instantiate (letter) as LetterTile;              //creating the letter
			temp.alphabetLetter = char.ToUpper (c).ToString ();     //setting the letter
			temp.name = "Alphabet " + char.ToUpper (c).ToString (); //naming it in unity

			dropzoneTemp = Instantiate (dropZoneAnswer) as Dropzone;             //creating the drop area
			dropzoneTemp.partAnswer = char.ToUpper (c).ToString ().ToString ();  //setting the answer/correct letter

			//adding it to unity as a child of DropZonePanel and naming it
			dropzoneTemp.transform.SetParent (DropZonePanel.transform);          
			dropzoneTemp.transform.localScale = Vector3.one;
			dropzoneTemp.name = "Dropzone " + char.ToUpper (c).ToString ();

			//adding them to their respective lists
			dropZoneList.Add (dropzoneTemp);
			answerShuffleList.Add (temp);
		}

		//shuffling the letters
		answerShuffleList = FisherYatesCardDeckShuffle (answerShuffleList);

		//adding the shuffled letters into unity as children of ShuffledLetterPanel
		foreach (LetterTile l in answerShuffleList) {
			l.transform.SetParent (ShuffledLetterPanel.transform);
			l.transform.localScale = Vector3.one;
		}

		//source.PlayOneShot(currentQuiz.naratorSound);
	}

    #region Random List
	//=======================================================================================//
	//==============================Fisher_Yates_CardDeck_Shuffle============================//
	//=======================================================================================//
	/// With the Fisher-Yates shuffle, first implemented on computers by Durstenfeld in 1964, 
	///   we randomly sort elements. This is an accurate, effective shuffling method for all array types.
	public static List<LetterTile> FisherYatesCardDeckShuffle (List<LetterTile> aList)
	{

		System.Random _random = new System.Random ();

		LetterTile myGO;

		int n = aList.Count;
		for (int i = 0; i < n; i++) {
			// NextDouble returns a random number between 0 and 1.
			// ... It is equivalent to Math.random() in Java.
			int r = i + (int)(_random.NextDouble () * (n - i));
			myGO = aList [r];
			aList [r] = aList [i];
			aList [i] = myGO;
		}

		return aList;
	}
    #endregion

	private void dropallChildren ()
	{
		//clears both lists and removes the children of ShuffledLetterPanel and DropZonePanel

		answerShuffleList.Clear ();
		dropZoneList.Clear ();

		foreach (Transform t in ShuffledLetterPanel.transform)
			Destroy (t.gameObject);

		foreach (Transform t in DropZonePanel.transform)
			Destroy (t.gameObject);
	}

	public void PopupActive ()
	{
		//if ShuffledLetterPanel is empty then all the letters were placed correctly
		if (ShuffledLetterPanel.transform.childCount == 0 && state == InputState.isFree) {
			PlaySound (true);
			congratzUI.OnActivatingUI (true);
			dropallChildren ();

			score++;

			if(score == quizList.Count)
            {
				congratzUI.OnActivatingUI(false);
				finishedUI.OnActivatingUI(true);
            }
		}
	}

	public void PlaySound (bool a)
	{
		if (a)
			source.PlayOneShot (CorrentSound);
		else
			source.PlayOneShot (wrongSound);
	}
}

public enum InputState
{
	isHold,
	isFree
}
