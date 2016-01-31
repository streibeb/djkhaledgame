using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gamestate : MonoBehaviour {

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
			var randomState = Random.Range(0,3);
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

		Debug.Log (this.gameState.Stats [0].Value);
		Debug.Log (this.gameState.mood.description);
	}

	private void MoodChanger() {
		int random = Random.Range(0,3);

		gameState.mood = moods [random];

		// update UI

	}

	private void NeedChanger() {
		foreach (var stat in gameState.Stats) {
			if (stat.Value < modifiers.NeedThreshold) {
				stat.IsNeeded = true;
			} else {
				stat.IsNeeded = false;
			}
		}
		// update UI
	}

	private void LifeDecay() {
		foreach (var stat in gameState.Stats) {
			if (stat.Value < modifiers.DecayThreshold) {
				gameState.Life -= 1;
				stat.IsCritical = true;
				if (gameState.Life == 0) {
					GameOver ();
				}
			} else {
				stat.IsCritical = false;
			}
		}
		// update ui
	}

	private void GameOver() {

		// update ui

	}

	public void FeedKhaled(int moodId) {
		var majorIncrease = 0;
		var majorDecrease = 1;
		var minorDecrease = 2;

		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}

	public void DrinkKhaled(int moodId) {
		var majorIncrease = 1;
		var majorDecrease = 3;
		var minorDecrease = 0;

		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}

	public void WashKhaled(int moodId) {
		var majorIncrease = 2;
		var majorDecrease = 0;
		var minorDecrease = 3;

		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}



	public void ActivityKhaled(int moodId) {
		var majorIncrease = 3;
		var majorDecrease = 2;
		var minorDecrease = 1;

		ModifyStats (moodId, majorIncrease, majorDecrease, minorDecrease);
	}

	private void ModifyStats(int moodId, int majorIncreaseVariable, int majorDecreaseVariable, int minorDecreaseVariable) {
		var majorIncrease = modifiers.PositiveMajor;
		var majorDecrease = modifiers.NegativeMajor;
		var minorDecrease = modifiers.NegativeMinor;

		if (gameState.Stats [majorIncreaseVariable].IsNeeded) {
			if (gameState.mood.description == moods [moodId].description) {
				majorIncrease *= modifiers.Correct;
			} else {
				majorIncrease *= Mathf.RoundToInt(modifiers.WrongMood);
			}
			if ((gameState.Stats [majorIncreaseVariable].Value + majorIncrease) >= modifiers.NeedThreshold) {
				gameState.Stats [majorIncreaseVariable].IsNeeded = false;
			}
		} else {
			majorIncrease *= Mathf.RoundToInt(modifiers.WrongStat);
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

		// UPDATE UI
	}

}
