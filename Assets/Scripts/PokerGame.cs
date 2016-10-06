using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.Networking;
using System.Linq;

public class Card
{
	public char value;
	public char suit;
}
		
public class Poker : MonoBehaviour 
{
	Player localPlayer = null;
	public Player winner;
	public GameObject card;
	string flop1;
	string flop2;
	string flop3;
	string turn;
	string river;
	public List<string> Deck = new List<string>();
	public List<Player> Players = new List<Player> ();
	public GameObject[] gPlayers;
	public int playerCount;
#region Sprite Declarations
	public Sprite c2;
	public Sprite c3;
	public Sprite c4;
	public Sprite c5;
	public Sprite c6;
	public Sprite c7;
	public Sprite c8;
	public Sprite c9;
	public Sprite cT;
	public Sprite cJ;
	public Sprite cQ;
	public Sprite cK;
	public Sprite cA;
	public Sprite s2;
	public Sprite s3;
	public Sprite s4;
	public Sprite s5;
	public Sprite s6;
	public Sprite s7;
	public Sprite s8;
	public Sprite s9;
	public Sprite sT;
	public Sprite sJ;
	public Sprite sQ;
	public Sprite sK;
	public Sprite sA;
	public Sprite h2;
	public Sprite h3;
	public Sprite h4;
	public Sprite h5;
	public Sprite h6;
	public Sprite h7;
	public Sprite h8;
	public Sprite h9;
	public Sprite hT;
	public Sprite hJ;
	public Sprite hQ;
	public Sprite hK;
	public Sprite hA;
	public Sprite d2;
	public Sprite d3;
	public Sprite d4;
	public Sprite d5;
	public Sprite d6;
	public Sprite d7;
	public Sprite d8;
	public Sprite d9;
	public Sprite dT;
	public Sprite dJ;
	public Sprite dQ;
	public Sprite dK;
	public Sprite dA;
	public Sprite faceDown;
	#endregion

	public class Player : NetworkBehaviour{

	public List<Card> hand = new List<Card> ();
	public string handRank;
	public int handValue;
	public Char handRankValue;
	public string kicker;
	public int credits;
	public int playerID;
	public string playerName;
	public string card1;
	public string card2;
	public int hearts;
	public int clubs;
	public int spades;
	public int diamonds;
	}



	void Start () 
	{

//		gPlayers = GameObject.FindGameObjectsWithTag ("Player");
//		foreach (GameObject player in gPlayers) 
//		{
//			Player pokerPlayer = new Player();
//			pokerPlayer.playerName = "Host";
//
//			Players.Add (pokerPlayer);
//		}
//
		winner = new Player ();
		Players = new List<Player> ();
		string cardsString = "As,2s,3s,4s,5s,6s,7s,8s,9s,Ts,Js,Qs,Ks,Ac,2c,3c,4c,5c,6c,7c,8c,9c,Tc,Jc,Qc,Kc,Ah,2h,3h,4h,5h,6h,7h,8h,9h,Th,Jh,Qh,Kh,Ad,2d,3d,4d,5d,6d,7d,8d,9d,Td,Jd,Qd,Kd";
		Deck.AddRange(cardsString.Split (','));

		}




	void tableManager()
	{
		Deal ();
		//Do this after all players have made their move
		Flop ();
		Turn ();
		River ();
		winner = CalculateHands ();
		winner.handValue = 0;
		Debug.Log ("Winner is : " + winner.playerName);
	}

	void Update()
	{
		
	}

	public Sprite getSprite(string Card)
	{
		switch (Card) 
		{
		case "As":
			return sA;
		case "2s":
			return s2;
		case "3s":
			return s3;
		case "4s":
			return s4;
		case "5s":
			return s5;
		case "6s":
			return s6;
		case "7s":
			return s7;
		case "8s":
			return s8;
		case "9s":
			return s9;
		case "Ts":
			return sT;
		case "Js":
			return sJ;
		case "Qs":
			return sQ;
		case "Ks":
			return sK;
		case "Ah":
			return hA;
		case "2h":
			return h2;
		case "3h":
			return h3;
		case "4h":
			return h4;
		case "5h":
			return h5;
		case "6h":
			return h6;
		case "7h":
			return h7;
		case "8h":
			return h8;
		case "9h":
			return h9;
		case "Th":
			return hT;
		case "Jh":
			return hJ;
		case "Qh":
			return hQ;
		case "Kh":
			return hK;
		case "Ac":
			return cA;
		case "2c":
			return c2;
		case "3c":
			return c3;
		case "4c":
			return c4;
		case "5c":
			return c5;
		case "6c":
			return c6;
		case "7c":
			return c7;
		case "8c":
			return c8;
		case "9c":
			return c9;
		case "Tc":
			return cT;
		case "Jc":
			return cJ;
		case "Qc":
			return cQ;
		case "Kc":
			return cK;
		case "Ad":
			return dA;
		case "2d":
			return d2;
		case "3d":
			return d3;
		case "4d":
			return d4;
		case "5d":
			return d5;
		case "6d":
			return d6;
		case "7d":
			return d7;
		case "8d":
			return d8;
		case "9d":
			return d9;
		case "Td":
			return dT;
		case "Jd":
			return dJ;
		case "Qd":
			return dQ;
		case "Kd":
			return dK;
		}
		return faceDown;
	}
	
	public void joinGame()
	{
		localPlayer = new Player ();
		localPlayer.playerName = PlayerPrefs.GetString ("Name", "Player");
		localPlayer.credits = PlayerPrefs.GetInt ("Credits", 500000);
		System.Random rnd = new System.Random(); 
		localPlayer.playerID = PlayerPrefs.GetInt ("ID", rnd.Next(100000,999999));
		playerCount++;
	}

	void OnApplicationQuit() 
	{
		PlayerPrefs.Save ();
		playerCount--;
	}

	void Deal()
	{
		foreach (Player player in Players) 
		{
			player.card1 = Deck [UnityEngine.Random.Range (0, Deck.Count)];
			Deck.Remove (player.card1);
			GameObject card1 = (GameObject)Instantiate (card, new Vector3 (-1.8f, -3.6f), Quaternion.identity);
			Sprite c1Sprite = getSprite (player.card1);
			card1.GetComponent<SpriteRenderer> ().sprite = c1Sprite;

			player.card2 = Deck [UnityEngine.Random.Range (0, Deck.Count)];
			Deck.Remove (player.card2);
			GameObject card2 = (GameObject)Instantiate (card, new Vector3 (-.6f, -3.6f), Quaternion.identity);
			Sprite c2Sprite = getSprite (player.card2);
			card2.GetComponent<SpriteRenderer> ().sprite = c2Sprite;

			//PM to player
			Debug.Log (player.playerName + " is dealt : " + player.card1 + player.card2);
		}
	}

	void Flop()
	{
		//Deal Flop
		flop1 = Deck[UnityEngine.Random.Range (0, Deck.Count)];
		Deck.Remove (flop1);
		GameObject gflop1 = (GameObject)Instantiate (card, new Vector3(-2.4f,0),Quaternion.identity);
		Sprite gf1Sprite = getSprite (flop1);
		gflop1.GetComponent<SpriteRenderer> ().sprite = gf1Sprite;

		flop2 = Deck[UnityEngine.Random.Range (0, Deck.Count)];
		Deck.Remove (flop2);
		GameObject gflop2 = (GameObject)Instantiate (card, new Vector3(-1.2f,0,-1),Quaternion.identity);
		Sprite gf2Sprite = getSprite (flop2);
		gflop2.GetComponent<SpriteRenderer> ().sprite = gf2Sprite;

		flop3 = Deck[UnityEngine.Random.Range (0, Deck.Count)];
		Deck.Remove (flop3);
		GameObject gflop3 = (GameObject)Instantiate (card, new Vector3(0f,0,-1),Quaternion.identity);
		Sprite gf3Sprite = getSprite (flop3);
		gflop3.GetComponent<SpriteRenderer> ().sprite = gf3Sprite;
		Debug.Log ("Flop: " + flop1 + flop2 + flop3);
		//Debug.Log ((52 - Deck.Count).ToString() + " cards have been dealt");
	}

	void Turn()
	{
		turn = Deck[UnityEngine.Random.Range (0, Deck.Count)];
		Deck.Remove (turn);
		GameObject gturn = (GameObject)Instantiate (card, new Vector3(1.2f,0,-1),Quaternion.identity);
		Sprite gtSprite = getSprite (turn);
		gturn.GetComponent<SpriteRenderer> ().sprite = gtSprite;
		Debug.Log ("Turn: " + turn);
	}
	
	void River()
	{
		river = Deck[UnityEngine.Random.Range (0, Deck.Count)];
		Deck.Remove (river);
		GameObject griver = (GameObject)Instantiate (card, new Vector3(2.4f,0,-1),Quaternion.identity);
		Sprite grSprite = getSprite (river);
		griver.GetComponent<SpriteRenderer> ().sprite = grSprite;
		Debug.Log ("River: " + river);
	}

	Card parseCard(string cardString)
	{
		Card _card = new Card ();
		_card.value = cardString [0];
		_card.suit = cardString[1];
		return _card;
	}

	string ReverseString(string s)
	{
		char[] arr = s.ToCharArray();
		Array.Reverse(arr);
		return new string(arr);
	}

	string isStraight(string s)
	{
		string sequentialCards = s.ToString().Replace ("AA", "A").Replace ("KK", "K").Replace ("QQ", "Q").Replace ("JJ", "J").Replace ("TT", "T").Replace ("99", "9").Replace ("88", "8").Replace ("77", "7").Replace ("66", "6").Replace ("55", "5").Replace ("44", "4").Replace ("33", "3").Replace ("22", "2");
		Regex regStraight = new Regex ("^5432A|65432|76543|87654|98765|T9876|JT987|QJT98|KQJT9|AKQJT$");
		Match match = regStraight.Match (sequentialCards);

		if (match.Success) 
		return match.Value.ToString ();
		else
			return "";
	}

	Dictionary<char,int> isFlush(List<Card> Hand)
	{
		//***********
		//Important update: track the highest valued card here so that the highest flush can win if multiple players have flush
		//**********
		int highValueInt = 0;
		char flushSuit;
		int hearts = 0;
		int spades = 0;
		int clubs = 0;
		int diamonds = 0;
		Dictionary<char, int> result = new Dictionary<char, int> ();
		foreach (Card card in Hand) {
			switch (card.suit) {
			case 'c':
				clubs++;
				break;
			case 'h':
				hearts++;
				break;
			case 's':
				spades++;
				break;
			case 'd':
				diamonds++;
				break;
			}
			//This does not work because you are using all of the values in the hand for "high value", you need to only find the highest value within the flush
			int thisValue = int.Parse (card.value.ToString ().Replace ("T", "10").Replace ("J", "11").Replace ("Q", "12").Replace ("K", "13").Replace ("A", "14"));
				if (thisValue > highValueInt) 
					{
					highValueInt = thisValue;
					}
				}
				if (hearts >= 5) {
					flushSuit = 'h';
				}
				if (clubs >= 5) {
					flushSuit = 'c';
				}
				if (spades >= 5) {
					flushSuit = 's';
				}
				if (diamonds >= 5) {
					flushSuit = 'd';
				} else {
					flushSuit = ' ';
				}


		result.Add (flushSuit, char.Parse (highValueInt.ToString ().Replace ("10", "T").Replace ("11", "J").Replace ("12", "Q").Replace ("13", "K").Replace ("14", "A")));

		return result;

	}

	string containsQuads(string s)
	{
		Regex quadRegex = new Regex (@"([a-zA-Z2-9])\1\1\1");
		Match match = quadRegex.Match (s);
		if (match.Success)
			return match.Value;
		else
			return "";
	}

	string containsTrips(string s)
	{
		Regex quadRegex = new Regex (@"([a-zA-Z2-9])\1\1");
		Match match = quadRegex.Match (s);
		if (match.Success)
			return match.Value;
		else
			return "";
	}

	string containsPair(string s)
	{
		Regex quadRegex = new Regex (@"([a-zA-Z2-9])\1");
		Match match = quadRegex.Match (s);
		if (match.Success)
			return match.Value;
		else
			return "";
	}


	
	Player CalculateHands()
	{
		foreach (Player player in Players) {
			player.handRank = "High Card";

			List<Card> Hand = new List<Card> ();
			Hand.Add (parseCard (flop1));
			Hand.Add (parseCard (flop2));
			Hand.Add (parseCard (flop3));
			Hand.Add (parseCard (turn));
			Hand.Add (parseCard (river));
			Hand.Add (parseCard (player.card1));
			Hand.Add (parseCard (player.card2));


			//Count suits for flushes and replace AKQJT with integers for sorting for straights
			List<int> cardValues = new List<int> ();
			foreach (Card card in Hand) {
				switch (card.suit) {
				case 'c':
					player.clubs++;
					break;
				case 'h':
					player.hearts++;
					break;
				case 's':
					player.spades++;
					break;
				case 'd':
					player.diamonds++;
					break;
				}
					
				switch (card.value) {
				case 'T':
					cardValues.Add (10);
					break;
				case 'J':
					cardValues.Add (11);
					break;
				case 'Q':
					cardValues.Add (12);
					break;
				case 'K':
					cardValues.Add (13);
					break;
				case 'A':
					cardValues.Add (14);
					cardValues.Add (1);
					break;
				default:
					cardValues.Add (int.Parse (card.value.ToString ()));
					break;
				}
			}

			cardValues.Sort ();
			StringBuilder sb = new StringBuilder ();

			//Convert 1,10,11,12,13,14 back to A,T,J,Q,K,A
			foreach (int value in cardValues) {
				switch (value) {
				case 10:
					sb.Append ('T');
					break;
				case 11:
					sb.Append ('J');
					break;
				case 12:
					sb.Append ('Q');
					break;
				case 13:
					sb.Append ('K');
					break;
				case 14:
					sb.Append ('A');
					break;
				case 1:
					sb.Append ('A');
					break;
				default:
					sb.Append (char.Parse (value.ToString ()));
					break;
				}
			}

			//sb contains a ascending list of cards A23456789TJQKA for each player's hand at this point

			//Remove low aces 
			string noLowAces = ReverseString(sb.ToString ());;
			if (noLowAces.Length > 7) 
			{
				noLowAces = noLowAces.Remove (7,noLowAces.Length - 7);
			}

			//Remove the quads first

			string quads = containsQuads(noLowAces);
			if (quads != "") 
			{
				player.handRank = "Four of a Kind";
				noLowAces = noLowAces.Replace (quads, "");

				player.handRankValue = char.Parse(quads.Substring (0, 1));
				player.kicker = noLowAces.Substring (0, 1);
			}
			string trips = containsTrips (noLowAces);
				if (trips != "")
				{
					if (player.handRank != "Four of a Kind") 
					{
						player.handRank = "Three of a Kind";
						noLowAces = noLowAces.Replace (trips, "");
					player.handRankValue = char.Parse(trips.Substring (0, 1));
						//player.kicker = noLowAces.Substring (0, 1);
					}
				}
			string pair = containsPair (noLowAces);
			if (pair != "")
			{
				if (player.handRank != "Four of a Kind") 
				{
					//Does the hand contain both TooK and a pair? = full house
					if (player.handRank == "Three of a Kind") {
						player.handRank = "Full House";
						//player.kicker = noLowAces.Substring (0, 1);
						noLowAces = noLowAces.Replace (pair, "");
					} 
					else
					{
						player.handRank = "Pair";
						noLowAces = noLowAces.Replace (pair, "");
						player.handRankValue = char.Parse(pair.Substring (0, 1));

						string pair2 = containsPair (noLowAces);
						if (pair2 != "") 
						{
							player.handRank = "Two Pair";
							noLowAces = noLowAces.Replace (pair2, "");
						}

					}
				}
			}
			//From the remaining card string, get the leftmost (highest) value for the kicker
			player.kicker = noLowAces.Substring (0, 1);

			//Check for a straight using fresh set of descending values (including low aces this time)
			string straightString = isStraight (ReverseString (sb.ToString ()));
			Dictionary<char,int> flushInfo = isFlush (Hand);

			char flushSuit = flushInfo.Keys.First ();
			int flushValue = flushInfo.Values.First ();
			if (flushSuit != ' ' && player.handRank != "Full House" && player.handRank != "Four of a Kind") 
			{
				//THIS DOES NOT WORK, IMAGINE A HAND 2c,3c,4c,5c,6c,Ks,As
				//As will be the highCard instead of 6c
				//As should be the kicker
				player.handRankValue = char.Parse(ReverseString (sb.ToString ()).Substring(0,1));
				player.handRank = "Flush";
			}



			if (straightString != "") {
				Debug.Log (player.playerName + " has a straight: " + isStraight (ReverseString (sb.ToString ())));
				player.handRank = "Straight";
				//Check for straight flush
				if (flushSuit != ' ') {
					int straightFlushCounter = 0;
					StringBuilder sb2 = new StringBuilder ();

					//For each value in the straight, see if it has the flush suit
					foreach (char _char in straightString) {
						//Lookup the suit using Hand (List<Card>)
						foreach (Card _card in Hand) {
							//If a card in the deck has the flush suit and a value in the straight, add it to a string and count it
							if (_card.suit == flushSuit && _card.value == _char) {
								straightFlushCounter++;
								sb2.Append (_card.value + _card.suit);
							}
						}
					}

					if (straightFlushCounter >= 5)
						player.handRank = "Straight-Flush";
					if (straightString == "AKQJT" && straightFlushCounter >= 5)
						player.handRank = "Royal-Flush";
				}
			}
			if (player.handRank == "High Card")
				Debug.Log (player.playerName + " has " + player.handRank + " of " + player.handRankValue);
			else
			Debug.Log (player.playerName + " has " + player.handRank);
		}

		//List<int> handRanks = new List<int> ();

		winner.handValue = 0;
		foreach (Player player in Players) 
		{
			
			switch (player.handRank) 
			{
			case "Royal Flush":
				player.handValue = 10;
				winner = player;
				break;
			case "Straight Flush":
				player.handValue = 9;
				if (winner.handValue < 9)
					winner = player;
				break;
			case "Four of a Kind":
				player.handValue = 8;
				if (winner.handValue < 8)
					winner = player;
				break;
			case "Full House":
				player.handValue = 7;
				if (winner.handValue < 7)
					winner = player;
				break;
			case "Flush":
				player.handValue = 6;
				if (winner.handValue < 6)
					winner = player;
				break;
			case "Straight":
				player.handValue = 5;
				if (winner.handValue < 5)
					winner = player;
				break;
			case "Three of a Kind":
				player.handValue = 4;
				if (winner.handValue < 4)
					winner = player;
				break;
			case "Two Pair":
				player.handValue = 3;
				if (winner.handValue < 3)
					winner = player;
				break;
			case "Pair":
				player.handValue = 2;
				if (winner.handValue < 2)
					winner = player;
//				//If the currently evaluated player has the same handValue as the winner, but a higher highCard the player becomes the new winner. Kickers will also be evaluated if the highCard is equal.
//				else if (winner.handValue == 2 && int.Parse (player.handRankValue.ToString ()) > int.Parse (winner.handRankValue.ToString ()) || int.Parse (winner.handValue.ToString ()) == 9 && int.Parse (player.handRankValue.ToString ()) == int.Parse (winner.handRankValue.ToString ()) && int.Parse (player.kicker) > int.Parse (winner.kicker)) {
//					winner = player;
//				}
				break;
			case "High Card":
				player.handValue = 1;
				break;
			}

			//handRanks.Add (player.handValue);
		}

		foreach (Player player in Players) 
		{
			if (player.handValue > winner.handValue) 
			{
				winner = player;
			} 
			else if (player.handValue == winner.handValue) 
			{
				//If this player has an equal hand value and higher "high" card, make them the winner
				if (int.Parse (player.handRankValue.ToString ().Replace ("T", "10").Replace ("J", "11").Replace ("Q", "12").Replace ("K", "13").Replace ("A", "14")) > int.Parse (winner.handRankValue.ToString ().Replace ("T", "10").Replace ("J", "11").Replace ("Q", "12").Replace ("K", "13").Replace ("A", "14"))) {
					winner = player;
				} else if (int.Parse (player.handRankValue.ToString ().Replace ("T", "10").Replace ("J", "11").Replace ("Q", "12").Replace ("K", "13").Replace ("A", "14")) == int.Parse (winner.handRankValue.ToString ().Replace ("T", "10").Replace ("J", "11").Replace ("Q", "12").Replace ("K", "13").Replace ("A", "14"))) {
					//Determine Kicker

				}
			}
		}

		return winner;
		}

	}