using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class gamestate : MonoBehaviour {

	public AudioClip[] eggWhiteClips;
	public AudioClip[] sausageClips;
	public AudioClip[] mcdonaldsClips;
	public AudioClip[] genericFoodClips;

	public AudioClip[] drinkClips;

	public AudioClip[] hottubClips;
	public AudioClip[] showerClips;
	public AudioClip[] genericHygieneClips;

	public AudioClip[] jetskiClips;
	public AudioClip[] lionClips;
	public AudioClip[] snapchatClips;
	public AudioClip[] cocoaButterClips;

	public AudioClip[] blessUp;
	public AudioClip[] winClips;
	public AudioClip[] loseClips;

	public AudioMixerGroup output;
	public AudioSource mainSound;

	private float updateTimer = 0;
	private float moodTimer = 0;

	private GameState gameState;
	private Modifiers modifiers;
	private Mood[] moods;

	public Text foodText;
	public Text drinkText;
	public Text hyText;
	public Text activityText;

	public Text scoreText;
	public Text moodText;
	public Text lifeText;

	public Image healthBar;
	public Animator moodController;
	public SpriteRenderer facial;
	public Sprite[] faces;
	public Animator gameOverController;
	public SpriteRenderer[] bubbles;

	private class Modifiers {
		public int Correct;
		public float WrongMood;
		public float WrongStat;
		public int PositiveMajor;
		public int NegativeMajor;
		public int NegativeMinor;
		public int DecayThreshold;
		public int NeedThreshold;

		public Modifiers() {
			Correct = 1;
			WrongMood = 0.5f;
			WrongStat = 0.25f;
			PositiveMajor = 25;
			NegativeMajor = 8;
			NegativeMinor = 4;
			DecayThreshold = 40;
			NeedThreshold = 65;
		}
	}

	private class Mood {
		public int id;
		public string description;
	}

	private class Stat {
		public string Name;
		public int Value;
		public bool IsNeeded;
		public bool IsCritical;
	}

	private class GameState {
		public Mood mood;
		public int Life;
		public int Score;
		public Stat[] Stats;

		public GameState(Mood _mood) {
			Stats = new Stat[4];
			Stats[0] = new Stat();
			Stats[0].Name = "Food";
			Stats[0].Value = 100;
			Stats[0].IsNeeded = false;
			Stats[0].IsCritical = false;
			Stats[1] = new Stat();
			Stats[1].Name = "Drink";
			Stats[1].Value = 100;
			Stats[1].IsNeeded = false;
			Stats[1].IsCritical = false;
			Stats[2] = new Stat();
			Stats[2].Name = "Hygiene";
			Stats[2].Value = 100;
			Stats[2].IsNeeded = false;
			Stats[2].IsCritical = false;
			Stats[3] = new Stat();
			Stats[3].Name = "Activities";
			Stats[3].Value = 100;
			Stats[3].IsNeeded = false;
			Stats[3].IsCritical = false;

			Score = 0;
			Life = 100;
			mood = _mood;

		}
	}

	// Use this for initialization
	void Start () {
		moods = new Mood[4];
		modifiers = new Modifiers ();
		moods [0] = new Mood ();
		moods [0].id = 0;
		moods [0].description = "Happy";
		moods [1] = new Mood ();
		moods [1].id = 1;
		moods [1].description = "Neutral";
		moods [2] = new Mood ();
		moods [2].id = 2;
		moods [2].description = "Sad";
		moods [3] = new Mood ();
		moods [3].id = 3;
		moods [3].description = "Angry";

		gameState = new GameState (moods[0]);
		moodController.SetInteger ("Mood", 0);

		bubbles[0].enabled = false;
		bubbles[1].enabled = false;
		bubbles[2].enabled = false;
		bubbles[3].enabled = false;

		mainSound.outputAudioMixerGroup = output;
	}

	void setText() {
		this.activityText.text = "Activity :" + this.gameState.Stats [3].Value.ToString ();
		this.foodText.text = "Food :" + this.gameState.Stats [0].Value.ToString ();
		this.hyText.text = "Hygeine :" + this.gameState.Stats [2].Value.ToString ();
		this.drinkText.text = "Drink :" + this.gameState.Stats [1].Value.ToString ();

		this.scoreText.text = "Bless Up Points :" + this.gameState.Score.ToString();
		this.moodText.text = "Mood :" + this.gameState.mood.description;
		this.lifeText.text = "Life :" + this.gameState.Life.ToString();
	}

	// Update is called once per frame
	void Update () {
		this.setText ();
		this.updateTimer += Time.deltaTime;
		this.moodTimer += Time.deltaTime;

		if (this.updateTimer >= 1) {
			var randomState = Random.Range(0,4);
			var state = gameState.Stats [randomState];

			switch (state.Name) {
			case "Food":
				state.Value -= 2;
				break;
			case "Drink":
				state.Value -= 2;
				break;
			case "Hygiene":
				state.Value -= 1;
				break;
			case "Activities":
				state.Value -= 1;
				break;
			default:
				break;
			}

			if (state.Value < 0) {
				state.Value = 0;
			}

			NeedChanger ();
			LifeDecay ();


			// UPDATE UI


			this.updateTimer = 0;
		}

		if (this.moodTimer >= 6) {
			MoodChanger ();
			this.moodTimer = 0;
		}

		//Debug.Log (this.gameState.Stats [0].Value);
		//Debug.Log (this.gameState.mood.description);
	}

	private void MoodChanger() {
		int random = Random.Range(0,4);

		gameState.mood = moods [random];

		// update UI
		moodController.SetInteger("Mood", random);
		facial.sprite = faces [random];
	}

	private void NeedChanger() {
		int i = 0;
		foreach (var stat in gameState.Stats) 
		{
			if (stat.Value < modifiers.NeedThreshold) {
				stat.IsNeeded = true;
				bubbles[i].enabled = true;
			} else {
				stat.IsNeeded = false;
				bubbles[i].enabled = false;
			}
			i++;
		}
	}

	private void LifeDecay() {
		foreach (var stat in gameState.Stats) {
			if (stat.Value < modifiers.DecayThreshold) {
				gameState.Life -= 1;
				stat.IsCritical = true;
				if (gameState.Life <= 0) {
					GameOver ();
				}
			} else {
				stat.IsCritical = false;
			}
		}
		// update ui
		healthBar.rectTransform.localScale = new Vector3(1.0f, gameState.Life / 100.0f, 1.0f);
		if (gameState.Life <= 50 && gameState.Life > 25) {
			healthBar.color = Color.yellow;
		} else if (gameState.Life < 25) {
			healthBar.color = Color.red;
		} else {
			healthBar.color = Color.green;
		}
	}

	private void GameOver() {

		// update ui
		gameOverController.enabled = true;

		int randomClip = Random.Range (0, eggWhiteClips.Length);
		mainSound.clip = loseClips[randomClip];
		mainSound.Stop ();
		mainSound.Play ();

	}

	private void PlaySound(string type, int subtype) {
		int randomClip;

		if (type == "food") {
			switch (subtype) {
			case 0: 
				randomClip = Random.Range (0, eggWhiteClips.Length);
				mainSound.clip = eggWhiteClips [randomClip];
				break;
			case 1:
				randomClip = Random.Range (0, sausageClips.Length);
				mainSound.clip = sausageClips [randomClip];
				break;
			case 2:
				randomClip = Random.Range (0, mcdonaldsClips.Length);
				mainSound.clip = mcdonaldsClips [randomClip];
				break;
			case 3:
				randomClip = Random.Range (0, genericFoodClips.Length);
				mainSound.clip = genericFoodClips [randomClip];
				break;
			}
		} else if (type == "drink") {
			randomClip = Random.Range (0, drinkClips.Length);
			mainSound.clip = drinkClips [randomClip];
		} else if (type == "wash") {
			switch (subtype) {
			case 0: 
				randomClip = Random.Range (0, genericHygieneClips.Length);
				mainSound.clip = genericHygieneClips [randomClip];
				break;
			case 2:
				randomClip = Random.Range (0, hottubClips.Length);
				mainSound.clip = hottubClips [randomClip];
				break;
			case 1:
				randomClip = Random.Range (0, showerClips.Length);
				mainSound.clip = showerClips [randomClip];
				break;
			case 3:
				randomClip = Random.Range (0, genericHygieneClips.Length);
				mainSound.clip = genericHygieneClips [randomClip];
				break;
			}
		} else if (type == "activity") {
			switch (subtype) {
			case 3: 
				randomClip = Random.Range (0, jetskiClips.Length);
				mainSound.clip = jetskiClips [randomClip];
				break;
			case 2:
				randomClip = Random.Range (0, lionClips.Length);
				mainSound.clip = lionClips [randomClip];
				break;
			case 1:
				randomClip = Random.Range (0, cocoaButterClips.Length);
				mainSound.clip = cocoaButterClips [randomClip];
				break;
			case 0:
				randomClip = Random.Range (0, snapchatClips.Length);
				mainSound.clip = snapchatClips [randomClip];
				break;
			}
		} else if (type == "blessup") {
			randomClip = Random.Range (0, blessUp.Length);
			mainSound.clip = blessUp [randomClip];
		}

		mainSound.Stop ();
		mainSound.Play ();
	}

	public void FeedKhaled(int moodId) {
		var majorIncrease = 0;
		var majorDecrease = 1;
		var minorDecrease = 2;

		PlaySound ("food", moodId);
		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}

	public void DrinkKhaled(int moodId) {
		var majorIncrease = 1;
		var majorDecrease = 3;
		var minorDecrease = 0;

		PlaySound ("drink", moodId);
		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}

	public void WashKhaled(int moodId) {
		var majorIncrease = 2;
		var majorDecrease = 0;
		var minorDecrease = 3;

		PlaySound ("wash", moodId);
		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}



	public void ActivityKhaled(int moodId) {
		var majorIncrease = 3;
		var majorDecrease = 2;
		var minorDecrease = 1;

		PlaySound ("activity", moodId);
		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}

	private void ModifyStats(int moodId, int majorIncreaseVariable, int majorDecreaseVariable, int minorDecreaseVariable) {
		var majorIncrease = modifiers.PositiveMajor;
		var majorDecrease = modifiers.NegativeMajor;
		var minorDecrease = modifiers.NegativeMinor;

		if (gameState.Stats [majorIncreaseVariable].IsNeeded) {
			if (gameState.mood.description == moods [moodId].description) {
				majorIncrease = Mathf.RoundToInt(majorIncrease * modifiers.Correct);
			} else {
				majorIncrease = Mathf.RoundToInt(majorIncrease * modifiers.WrongMood);
			}
			if ((gameState.Stats [majorIncreaseVariable].Value + majorIncrease) >= modifiers.NeedThreshold) {
				gameState.Stats [majorIncreaseVariable].IsNeeded = false;
			}
		} else {
			majorIncrease = Mathf.RoundToInt(majorIncrease * modifiers.WrongStat);
		}


		if ((gameState.Stats [majorIncreaseVariable].Value + majorIncrease) >= 100) {
			gameState.Stats [majorIncreaseVariable].Value = 100;
		} else {
			gameState.Stats [majorIncreaseVariable].Value += majorIncrease;
		}

		if ((gameState.Stats [majorDecreaseVariable].Value - majorDecrease) <= 0) {
			gameState.Stats [majorDecreaseVariable].Value = 0;
		} else {
			gameState.Stats [majorDecreaseVariable].Value -= majorDecrease;
		}

		if ((gameState.Stats [minorDecreaseVariable].Value - minorDecrease) <= 0) {
			gameState.Stats [minorDecreaseVariable].Value = 0;
		} else {
			gameState.Stats [minorDecreaseVariable].Value -= minorDecrease;
		}

		int scoreChange = majorIncrease - majorDecrease - minorDecrease;

		gameState.Score = scoreChange;

		if (scoreChange > 0 && gameState.Score % 100 == 0) {
			PlaySound ("blessup", 0);
		}


		// UPDATE UI
	}

}
