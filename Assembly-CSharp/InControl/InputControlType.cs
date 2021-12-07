using System;

namespace InControl
{
	// Token: 0x0200006E RID: 110
	public enum InputControlType
	{
		// Token: 0x04000307 RID: 775
		None,
		// Token: 0x04000308 RID: 776
		LeftStickUp,
		// Token: 0x04000309 RID: 777
		LeftStickDown,
		// Token: 0x0400030A RID: 778
		LeftStickLeft,
		// Token: 0x0400030B RID: 779
		LeftStickRight,
		// Token: 0x0400030C RID: 780
		LeftStickButton,
		// Token: 0x0400030D RID: 781
		RightStickUp,
		// Token: 0x0400030E RID: 782
		RightStickDown,
		// Token: 0x0400030F RID: 783
		RightStickLeft,
		// Token: 0x04000310 RID: 784
		RightStickRight,
		// Token: 0x04000311 RID: 785
		RightStickButton,
		// Token: 0x04000312 RID: 786
		DPadUp,
		// Token: 0x04000313 RID: 787
		DPadDown,
		// Token: 0x04000314 RID: 788
		DPadLeft,
		// Token: 0x04000315 RID: 789
		DPadRight,
		// Token: 0x04000316 RID: 790
		LeftTrigger,
		// Token: 0x04000317 RID: 791
		RightTrigger,
		// Token: 0x04000318 RID: 792
		LeftBumper,
		// Token: 0x04000319 RID: 793
		RightBumper,
		// Token: 0x0400031A RID: 794
		Action1,
		// Token: 0x0400031B RID: 795
		Action2,
		// Token: 0x0400031C RID: 796
		Action3,
		// Token: 0x0400031D RID: 797
		Action4,
		// Token: 0x0400031E RID: 798
		Action5,
		// Token: 0x0400031F RID: 799
		Action6,
		// Token: 0x04000320 RID: 800
		Action7,
		// Token: 0x04000321 RID: 801
		Action8,
		// Token: 0x04000322 RID: 802
		Action9,
		// Token: 0x04000323 RID: 803
		Action10,
		// Token: 0x04000324 RID: 804
		Action11,
		// Token: 0x04000325 RID: 805
		Action12,
		// Token: 0x04000326 RID: 806
		Back = 100,
		// Token: 0x04000327 RID: 807
		Start,
		// Token: 0x04000328 RID: 808
		Select,
		// Token: 0x04000329 RID: 809
		System,
		// Token: 0x0400032A RID: 810
		Options,
		// Token: 0x0400032B RID: 811
		Pause,
		// Token: 0x0400032C RID: 812
		Menu,
		// Token: 0x0400032D RID: 813
		Share,
		// Token: 0x0400032E RID: 814
		Home,
		// Token: 0x0400032F RID: 815
		View,
		// Token: 0x04000330 RID: 816
		Power,
		// Token: 0x04000331 RID: 817
		Capture,
		// Token: 0x04000332 RID: 818
		Plus,
		// Token: 0x04000333 RID: 819
		Minus,
		// Token: 0x04000334 RID: 820
		PedalLeft = 150,
		// Token: 0x04000335 RID: 821
		PedalRight,
		// Token: 0x04000336 RID: 822
		PedalMiddle,
		// Token: 0x04000337 RID: 823
		GearUp,
		// Token: 0x04000338 RID: 824
		GearDown,
		// Token: 0x04000339 RID: 825
		Pitch = 200,
		// Token: 0x0400033A RID: 826
		Roll,
		// Token: 0x0400033B RID: 827
		Yaw,
		// Token: 0x0400033C RID: 828
		ThrottleUp,
		// Token: 0x0400033D RID: 829
		ThrottleDown,
		// Token: 0x0400033E RID: 830
		ThrottleLeft,
		// Token: 0x0400033F RID: 831
		ThrottleRight,
		// Token: 0x04000340 RID: 832
		POVUp,
		// Token: 0x04000341 RID: 833
		POVDown,
		// Token: 0x04000342 RID: 834
		POVLeft,
		// Token: 0x04000343 RID: 835
		POVRight,
		// Token: 0x04000344 RID: 836
		TiltX = 250,
		// Token: 0x04000345 RID: 837
		TiltY,
		// Token: 0x04000346 RID: 838
		TiltZ,
		// Token: 0x04000347 RID: 839
		ScrollWheel,
		// Token: 0x04000348 RID: 840
		[Obsolete("Use InputControlType.TouchPadButton instead.", true)]
		TouchPadTap,
		// Token: 0x04000349 RID: 841
		TouchPadButton,
		// Token: 0x0400034A RID: 842
		TouchPadXAxis,
		// Token: 0x0400034B RID: 843
		TouchPadYAxis,
		// Token: 0x0400034C RID: 844
		LeftSL,
		// Token: 0x0400034D RID: 845
		LeftSR,
		// Token: 0x0400034E RID: 846
		RightSL,
		// Token: 0x0400034F RID: 847
		RightSR,
		// Token: 0x04000350 RID: 848
		Command = 300,
		// Token: 0x04000351 RID: 849
		LeftStickX,
		// Token: 0x04000352 RID: 850
		LeftStickY,
		// Token: 0x04000353 RID: 851
		RightStickX,
		// Token: 0x04000354 RID: 852
		RightStickY,
		// Token: 0x04000355 RID: 853
		DPadX,
		// Token: 0x04000356 RID: 854
		DPadY,
		// Token: 0x04000357 RID: 855
		Analog0 = 400,
		// Token: 0x04000358 RID: 856
		Analog1,
		// Token: 0x04000359 RID: 857
		Analog2,
		// Token: 0x0400035A RID: 858
		Analog3,
		// Token: 0x0400035B RID: 859
		Analog4,
		// Token: 0x0400035C RID: 860
		Analog5,
		// Token: 0x0400035D RID: 861
		Analog6,
		// Token: 0x0400035E RID: 862
		Analog7,
		// Token: 0x0400035F RID: 863
		Analog8,
		// Token: 0x04000360 RID: 864
		Analog9,
		// Token: 0x04000361 RID: 865
		Analog10,
		// Token: 0x04000362 RID: 866
		Analog11,
		// Token: 0x04000363 RID: 867
		Analog12,
		// Token: 0x04000364 RID: 868
		Analog13,
		// Token: 0x04000365 RID: 869
		Analog14,
		// Token: 0x04000366 RID: 870
		Analog15,
		// Token: 0x04000367 RID: 871
		Analog16,
		// Token: 0x04000368 RID: 872
		Analog17,
		// Token: 0x04000369 RID: 873
		Analog18,
		// Token: 0x0400036A RID: 874
		Analog19,
		// Token: 0x0400036B RID: 875
		Button0 = 500,
		// Token: 0x0400036C RID: 876
		Button1,
		// Token: 0x0400036D RID: 877
		Button2,
		// Token: 0x0400036E RID: 878
		Button3,
		// Token: 0x0400036F RID: 879
		Button4,
		// Token: 0x04000370 RID: 880
		Button5,
		// Token: 0x04000371 RID: 881
		Button6,
		// Token: 0x04000372 RID: 882
		Button7,
		// Token: 0x04000373 RID: 883
		Button8,
		// Token: 0x04000374 RID: 884
		Button9,
		// Token: 0x04000375 RID: 885
		Button10,
		// Token: 0x04000376 RID: 886
		Button11,
		// Token: 0x04000377 RID: 887
		Button12,
		// Token: 0x04000378 RID: 888
		Button13,
		// Token: 0x04000379 RID: 889
		Button14,
		// Token: 0x0400037A RID: 890
		Button15,
		// Token: 0x0400037B RID: 891
		Button16,
		// Token: 0x0400037C RID: 892
		Button17,
		// Token: 0x0400037D RID: 893
		Button18,
		// Token: 0x0400037E RID: 894
		Button19,
		// Token: 0x0400037F RID: 895
		Count
	}
}
