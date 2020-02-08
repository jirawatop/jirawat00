// This script is a Manager that controls the UI HUD (deaths, time, and orbs) for the 
// project. All HUD UI commands are issued through the static methods of this class

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
   
    private static UIManager current;

	public TextMeshProUGUI TokenText;			
	public TextMeshProUGUI timeText;		
	//public TextMeshProUGUI deathText;		
	public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI data_items;

    private void Awake()
	{
		
		if (current != null && current != this)
		{
			
			Destroy(gameObject);
			return;
		}

		
		current = this;
		DontDestroyOnLoad(gameObject);
	}


    public static void update_Data_Items()
    {
        if (current == null)
            return;
       // current.data_items.text = ;
    }


    public static void UpdateSave(int Tokken1)
    {
        if (current == null)
            return;
        current.data_items.text = Tokken1.ToString();
    }

    public static void UpdateTokenUI(int TokenCount)
	{
		
		if (current == null)
			return;

		
		current.TokenText.text = TokenCount.ToString()+"/"+3;
	}



	public static void UpdateTimeUI(float time)
	{
		if (current == null)
			return;

		
		int minutes = (int)(time / 60);
		float seconds = time % 60f;

		
		current.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
    /*
	public static void UpdateDeathUI(int deathCount)
	{
		//If there is no current UIManager, exit
		if (current == null)
			return;

		//update the player death count element
		//current.deathText.text = deathCount.ToString();
	}
    */
	public static void DisplayGameOverText()
	{
		
		if (current == null)
			return;

		
		current.gameOverText.enabled = true;
	}
}
