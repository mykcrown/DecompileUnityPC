// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CreditsScreen : GameScreen
{
	public Transform BackButtonAnchor;

	public GameObject BackButtonPrefab;

	private CreditsScene scene;

	private CreditsData credits = new CreditsData();

	public override void Start()
	{
		base.Start();
		this.setupCredits();
		this.scene = base.uiAdapter.GetUIScene<CreditsScene>();
		this.scene.Init(this.credits);
		base.addBackButtonForMenuScreen(this.BackButtonAnchor, this.BackButtonPrefab);
	}

	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		base.audioManager.PlayMusic(SoundKey.credits_music);
	}

	private void setupCredits()
	{
		this.credits.AddHeading("Wavedash Staff", 15f);
		this.credits.AddNameCredit("Aaron Krasnov", "Lead Product Manager", 8f);
		this.credits.AddNameCredit("Adam \"Strong Badam\" Oliver", "Game Designer", 8f);
		this.credits.AddNameCredit("Alejandro Rodriguez", "Lead Animator", 8f);
		this.credits.AddNameCredit("Alex \"zillix\" Erlandson", "Technical Director", 8f);
		this.credits.AddNameCredit("Anthony \"Llama Juice\" Chester", "Environment Artist", 8f);
		this.credits.AddNameCredit("April Ratto", "Office Manager", 8f);
		this.credits.AddNameCredit("Carson \"Pixel\" Allen", "Community Support Representative", 8f);
		this.credits.AddNameCredit("Chris \"clockwood\" Lockwood", "Gameplay Engineer", 8f);
		this.credits.AddNameCredit("Chris Rogers", "Technical Artist", 8f);
		this.credits.AddNameCredit("Cody \"Lucky\" Black", "Environment Artist", 8f);
		this.credits.AddNameCredit("Edison Luong", "Senior Gameplay Engineer", 8f);
		this.credits.AddNameCredit("Eleine Sun", "Esports Event Manager", 8f);
		this.credits.AddNameCredit("Eric Whelpley", "Senior Server Engineer", 8f);
		this.credits.AddNameCredit("Ethan Gomes", "Office Manager", 8f);
		this.credits.AddNameCredit("Grace Shen", "Senior Brand Manager", 8f);
		this.credits.AddNameCredit("Janelle Hitz", "UI/UX Designer", 8f);
		this.credits.AddNameCredit("Jason Rice", "Creative Director", 8f);
		this.credits.AddNameCredit("Jordan \"Virum\" Campbell", "Animator", 8f);
		this.credits.AddNameCredit("Josh Singh", "Art Director", 8f);
		this.credits.AddNameCredit("Justin Prazen", "Technical Artist", 8f);
		this.credits.AddNameCredit("Kalen Chock", "Senior Concept Artist", 8f);
		this.credits.AddNameCredit("Kara \"karakancel\" Klaczynski", "Office Manager and Human Resources", 8f);
		this.credits.AddNameCredit("Kienan Lafferty", "Senior Concept Artist", 8f);
		this.credits.AddNameCredit("Kris Orpilla", "Lead VFX Artist", 8f);
		this.credits.AddNameCredit("Marissa \"Ambehr\" Huculak", "Animator", 8f);
		this.credits.AddNameCredit("Matt \"Scav\" Fairchild", "Chief Executive Officer", 8f);
		this.credits.AddNameCredit("Matthew \"Vontre\" Siegel", "Senior Gameplay Engineer", 8f);
		this.credits.AddNameCredit("Meb Byrne", "Office Manager", 8f);
		this.credits.AddNameCredit("Michael \"M2tM\" Hamilton", "Senior Gameplay Engineer", 8f);
		this.credits.AddNameCredit("Michael Maurino", "Senior Concept Artist", 8f);
		this.credits.AddNameCredit("Taylor \"Warchamp7\" Giampaolo", "QA Analyst", 8f);
		this.credits.AddNameCredit("Ted \"TedAjax\" Dobyns", "Senior Gameplay Engineer", 8f);
		this.credits.AddNameCredit("Thomas Moyles", "Project Manager", 8f);
		this.credits.AddNameCredit("Tim Hume", "Game Systems Engineer", 8f);
		this.credits.AddNameCredit("Twain Martin", "Lead Server Engineer", 8f);
		this.credits.AddNameCredit("Wesley \"SmashGizmo\" Ruttle", "Lead Game Designer", 8f);
		this.credits.AddNameCredit("Will \"Reno\" Hsiao", "Community Manager", 8f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Contract Engineering Support", 15f);
		this.credits.AddMessageCredit("Karl Orosz", 8f);
		this.credits.AddMessageCredit("Tony Schuster", 8f);
		this.credits.AddMessageCredit("Ian Hill", 8f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Contract Art & Animation Support", 15f);
		this.credits.AddMessageCredit("Alyssa Herman", 8f);
		this.credits.AddMessageCredit("Andrew Tran", 8f);
		this.credits.AddMessageCredit("Brandi Hall", 8f);
		this.credits.AddMessageCredit("Chamille Miles", 8f);
		this.credits.AddMessageCredit("David Chiariello", 8f);
		this.credits.AddMessageCredit("German Reverso", 8f);
		this.credits.AddMessageCredit("Jaleh Afshar", 8f);
		this.credits.AddMessageCredit("Justin Wong", 8f);
		this.credits.AddMessageCredit("Megan Thompkins", 8f);
		this.credits.AddMessageCredit("Melissa Yabumoto", 8f);
		this.credits.AddMessageCredit("Nicole Echeverria", 8f);
		this.credits.AddMessageCredit("Rachel Corey", 8f);
		this.credits.AddMessageCredit("Sudi Rouhi", 8f);
		this.credits.AddMessageCredit("Worth \"mutatedjellyfish\" Dayley", 8f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Additional Art & Animation Support", 15f);
		this.credits.AddMessageCredit("Omnom Workshop", 16f);
		this.credits.AddMessageCredit("SuperGenius", 8f);
		this.credits.AddNameCredit("Naomi Fish", "Animation Director / Animator", 6f);
		this.credits.AddNameCredit("Rob Bekuhrs", "Animator", 4f);
		this.credits.AddNameCredit("Cole Olsen", "Animator", 4f);
		this.credits.AddNameCredit("Dennis Rivera", "Animator", 4f);
		this.credits.AddNameCredit("Don Fergus", "Animator / Rigger", 4f);
		this.credits.AddNameCredit("Andrew Hagel", "Animator", 4f);
		this.credits.AddNameCredit("Robert Firestone", "Animator", 4f);
		this.credits.AddNameCredit("Chris Gortz", "Animator", 4f);
		this.credits.AddNameCredit("Julius Jockusch", "Animator", 4f);
		this.credits.AddNameCredit("Brice Anderson", "VFX", 4f);
		this.credits.AddNameCredit("Daryn Olsen", "VFX", 4f);
		this.credits.AddNameCredit("Traci Cook", "Senior Producer", 4f);
		this.credits.AddNameCredit("Ian Chapman", "Producer", 4f);
		this.credits.AddNameCredit("Greg Savoia", "VP of Production", 4f);
		this.credits.AddNameCredit("Damon Redmond", "Executive Producer", 16f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Music", 15f);
		this.credits.AddMessageCredit("Gareth Coker", 8f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Voice Over", 15f);
		this.credits.AddNameCredit("Luke Barnett", "Recording Producer", 8f);
		this.credits.AddNameCredit("Vince Masciale", "Recording Producer", 8f);
		this.credits.AddNameCredit("Terry Berland", "Casting Director", 8f);
		this.credits.AddNameCredit("Mariana Novak", "Announcer", 8f);
		this.credits.AddNameCredit("Cherise Booth", "Ashani", 8f);
		this.credits.AddNameCredit("Jason Spisak", "Kidd", 8f);
		this.credits.AddNameCredit("Kari Wahlgren", "Xana", 8f);
		this.credits.AddNameCredit("Stephanie Sheh", "Zhurong", 8f);
		this.credits.AddNameCredit("John Wusah", "Weishan", 8f);
		this.credits.AddNameCredit("Steve Blum", "Raymer", 8f);
		this.credits.AddNameCredit("Julianne Buscher", "Afi & Galu", 8f);
		this.credits.AddNameCredit("Cristina Vee", "Ezzie", 8f);
		this.credits.AddSpace(5f);
		this.credits.AddMessageCredit("Recording Production by Lone Suspect Productions", 8f);
		this.credits.AddMessageCredit("Casting by Berland Casting", 8f);
		this.credits.AddMessageCredit("Recorded at Icemen Audio in Burbank, CA", 8f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Junior QA Testers", 15f);
		this.credits.AddMessageCredit("Adilyn", 8f);
		this.credits.AddMessageCredit("Duncan", 8f);
		this.credits.AddMessageCredit("Ronan", 8f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Community Council", 10f);
		this.credits.AddMessageCredit("Adam \"Strong Badam\" Oliver", 4f);
		this.credits.AddMessageCredit("Arda \"Ishi\" Aysu", 4f);
		this.credits.AddMessageCredit("Charles \"Cactuar\" Meighen", 4f);
		this.credits.AddMessageCredit("Hugo \"HugS\" Gonzalez", 4f);
		this.credits.AddMessageCredit("Sesh Evans", 4f);
		this.credits.AddMessageCredit("Taylor \"Warchamp7\" Giampaolo", 4f);
		this.credits.AddMessageCredit("VGBootCamp", 4f);
		this.credits.AddSpace(20f);
		this.credits.AddHeading("Special Thanks", 10f);
		this.credits.AddMessageCredit("Aefore", 4f);
		this.credits.AddMessageCredit("Cripsy4000", 4f);
		this.credits.AddMessageCredit("Hylian", 4f);
		this.credits.AddMessageCredit("Imogen", 4f);
		this.credits.AddMessageCredit("Juanpi", 4f);
		this.credits.AddMessageCredit("lessthanpi", 4f);
		this.credits.AddMessageCredit("Liberation", 4f);
		this.credits.AddMessageCredit("Londo Leo", 4f);
		this.credits.AddMessageCredit("Metafist", 4f);
		this.credits.AddMessageCredit("Pooch", 4f);
		this.credits.AddMessageCredit("Ruby", 4f);
		this.credits.AddMessageCredit("WaveParadigm", 4f);
		this.credits.AddMessageCredit("The Combat Lab (Shakes, Arikie, Wonderbop)", 8f);
		this.credits.AddMessageCredit("Lorin", 4f);
		this.credits.AddMessageCredit("Hero", 4f);
		this.credits.AddMessageCredit("Mike Giampaolo", 4f);
		this.credits.AddMessageCredit("Michele Giampaolo", 4f);
		this.credits.AddMessageCredit("Aaron Keech", 4f);
		this.credits.AddMessageCredit("Cora", 4f);
		this.credits.AddMessageCredit("Sarah", 4f);
		this.credits.AddMessageCredit("Sylvia Lewin", 4f);
		this.credits.AddMessageCredit("Jaleh Afshar", 4f);
		this.credits.AddMessageCredit("David Chiariello", 4f);
		this.credits.AddMessageCredit("Shelaya Dobyns", 4f);
		this.credits.AddMessageCredit("Jaclyn \"Jai\" Durante", 4f);
		this.credits.AddMessageCredit("Susan Lee", 4f);
		this.credits.AddMessageCredit("CJ Scaduto", 4f);
		this.credits.AddSpace(10f);
		this.credits.AddMessageCredit("In loving memory of Adam Chester", 8f);
	}

	public override void OnCancelPressed()
	{
		this.GoToPreviousScreen();
	}

	public override void UpdateRightStick(float x, float y)
	{
		if (this.scene != null)
		{
			this.scene.UpdateRightStick(x, y);
		}
	}
}
