using UnityEngine;
using System;

public class TimeManager : MonoBehaviour {
	public static TimeManager instance;

	public static TimeManager Instance {
		get {
			if(instance == null) {
				if(!(instance = FindObjectOfType<TimeManager>())) {
					instance = new GameObject("TimeManager").AddComponent<TimeManager>();
				}
			}

			return instance;
		}
	}

	public Action OnMinuteChanged;
    public Action OnHourChanged;

    public int Minute { get; private set; }
	public int Hour { get; private set; }
	public TimeTag timeTag { get; private set; }

	private float minuteToRealTime = 0.5f;
    private float timer;
	private bool isPaused = false;

	private void Awake() {
		Minute = 0;
		Hour = 8;
		timer = minuteToRealTime;

		DontDestroyOnLoad(gameObject);

		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	private void Update() {
		if(!isPaused) {
			timer -= Time.deltaTime;

			if(timer <= 0) {
				Minute++;
				OnMinuteChanged?.Invoke();
				if(Minute >= 60) {
					Hour++;
					if(Hour == 12) {
						ToggeTimeTag();
					} else if(Hour > 12) {
						Hour = 1;
						
					}
					
					OnHourChanged?.Invoke();
					Minute = 0;
				}

				timer = minuteToRealTime;
			}
		}
	}

	public void SetTime(int hour, int minute) {
		Hour = hour;
		Minute = minute;
	}

	private void ToggeTimeTag() {
		if(timeTag == TimeTag.AM) {
			timeTag = TimeTag.PM;
		} else {
			timeTag = TimeTag.AM;
		}
	}

	public float GetTimeAsFloat() {
		return Hour + (Minute / 60f);
	}

	public void PauseTimer() {
		isPaused = true;
	}

	public void ResumeTimer() {
		isPaused = false;
	}
}

public enum TimeTag {
	AM,
	PM
}